using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Ranged weapon class.
    /// </summary>
    public sealed class RangedWeapon : Weapon
    {
        /// <summary>
        /// Weapon stats.
        /// </summary>
        [SerializeField] private RangedWeaponStats stats;

        /// <summary>
        /// Muzzle of the weapon.
        /// </summary>
        [SerializeField] private Transform muzzle;

        /// <summary>
        /// Trail renderer prefab.
        /// </summary>
        [SerializeField] private TrailRenderer trailRendererPrefab;

        /// <summary>
        /// Number of bullets in the magazine.
        /// </summary>
        private int _magazineCount;

        /// <summary>
        /// Is the weapon currently reloading.
        /// </summary>
        private bool _reloading;

        /// <summary>
        /// Recoil factor.
        /// </summary>
        private float _recoilFactor;

        /// <summary>
        /// Current fire mode.
        /// </summary>
        private FireModes _currentFireMode;

        /// <summary>
        /// Dictionary of fire mode strategies.
        /// </summary>
        private Dictionary<FireModes, IFireModeStrategy> _fireModeStrategies;

        private ObjectPool<TrailRenderer> _trailRendererPool;

        private void Start()
        {
            // Initialize the dictionary of fire mode strategies
            _fireModeStrategies = new Dictionary<FireModes, IFireModeStrategy>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, IFireModeStrategy.GetFireModeStrategy(mode));

            // Set the default fire mode and magazine count.
            _currentFireMode = _fireModeStrategies.First().Key;
            _magazineCount = stats.MagazineSize;

            // Initialize the trail renderer pool.
            _trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
        }

        /// <summary>
        /// Create a bullet trail renderer.
        /// </summary>
        /// <returns></returns>
        private TrailRenderer CreateTrail()
        {
            // Instantiate a trail renderer and set its properties.
            var trail = Instantiate(trailRendererPrefab);
            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
        }

        /// <summary>
        /// Cycle through the allowed fire modes.
        /// </summary>
        public void SwitchFireMode()
        {
            // End the previous attack if it's still running
            EndAttack();

            // Set the new fire mode
            var indexOf = (Array.IndexOf(stats.FireModes, _currentFireMode) + 1) % stats.FireModes.Length;
            _currentFireMode = stats.FireModes[indexOf];
        }

        /// <summary>
        /// Start attacking.
        /// </summary>
        public override void BeginAttack()
        {
            // If the weapon is reloading, don't start attacking.
            if (_reloading) return;

            base.BeginAttack();

            // Reset the recoil factor.
            _recoilFactor = 0;
        }

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator OnAttack()
        {
            // Find a fire mode strategy and wait for it to finish, else show an error.
            if (_fireModeStrategies.TryGetValue(_currentFireMode, out var strategy))
                yield return strategy.Fire(stats, FireBullet);
            else
                Debug.LogError($"No fire mode set for {stats.WeaponName}", stats);
        }

        /// <summary>
        /// Fire the bullet.
        /// </summary>
        private void FireBullet()
        {
            var spreadOffset = new Vector3(Random.Range(-stats.Spread, stats.Spread), Random.Range(-stats.Spread, stats.Spread), 0); // Add some spread to the bullet.
            var recoilOffset = new Vector3(0, Random.Range(0, stats.Recoil), 0); // Add some recoil to the bullet.
            _recoilFactor = Mathf.Clamp01(_recoilFactor + 0.1f); // Increase the recoil factor.

            // Get the direction of the bullet.
            var fireDirection = muzzle.forward;
            fireDirection = (fireDirection + spreadOffset).normalized;
            fireDirection = (fireDirection + recoilOffset * _recoilFactor).normalized;

            // Raycast to check if the bullet hits something. If it does, play the trail to that point, else play the trail to the miss distance.
            StartCoroutine(Physics.Raycast(muzzle.position, fireDirection, out var hit, stats.MissDistance)
                ? PlayTrail(muzzle.position, hit.point)
                : PlayTrail(muzzle.position, muzzle.position + fireDirection * stats.MissDistance));

            // Decrease the magazine count and reload if it's empty.
            if (--_magazineCount != 0) return;

            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);
            StartCoroutine(ReloadCoroutine());
        }

        /// <summary>
        /// Coroutine for playing the trail.
        /// </summary>
        /// <param name="startPos">Start position of the trail</param>
        /// <param name="endPos">End position of the trail</param>
        private IEnumerator PlayTrail(Vector3 startPos, Vector3 endPos)
        {
            // Get a trail renderer from the pool and set its position.
            var trail = _trailRendererPool.Get();
            trail.transform.position = startPos;
            trail.emitting = true;
            trail.gameObject.SetActive(true);

            // Play the trail from the start position to the end position.
            var distance = Vector3.Distance(startPos, endPos);
            var remainingDistance = distance;
            while (remainingDistance > 0)
            {
                trail.transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01((distance - remainingDistance) / distance));
                remainingDistance -= stats.TrailSpeed * Time.deltaTime;

                yield return null;
            }

            trail.transform.position = endPos;

            // Wait for the trail to finish emitting and then release it back to the pool.
            yield return new WaitForSeconds(trail.time);

            trail.emitting = false;
            trail.gameObject.SetActive(false);
            _trailRendererPool.Release(trail);
        }

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private IEnumerator ReloadCoroutine()
        {
            // Wait for the reload time and then refill the magazine.
            _reloading = true;
            yield return new WaitForSeconds(stats.ReloadTime);
            _magazineCount = stats.MagazineSize;
            _reloading = false;

            if (Attacking)
                BeginAttack();
        }
    }
}