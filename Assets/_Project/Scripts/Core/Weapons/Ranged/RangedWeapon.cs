using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using Unity.Mathematics;
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
        /// Bullet impact prefab.
        /// </summary>
        [SerializeField] private GameObject bulletImpactPrefab;
        
        public RangedWeaponStats Stats => stats;

        /// <summary>
        /// Property to access current number of bullets in the magazine.
        /// </summary>
        public int CurrentAmmo { get; private set; }

        /// <summary>
        /// Property to access the maximum number of bullets for the Gun.
        /// </summary>
        public int MaxAmmo { get; private set; }

        ///<summary>
        /// Property to check if the weapon is currently reloading.
        /// </summary>
        public bool IsReloading => _reloading;

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
        private Dictionary<FireModes, FiringPin> _fireModeStrategies;

        /// <summary>
        /// Object pool for trail renderers.
        /// </summary>
        private ObjectPool<TrailRenderer> _trailRendererPool;

        /// <summary>
        /// Object pool for bullet impacts.
        /// </summary>
        private ObjectPool<GameObject> _bulletImpactPool;

        /// <summary>
        /// Layer mask for the opponent.
        /// </summary>
        private LayerMask _opponentLayer;

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private Coroutine _reloadCoroutine;

        private void Start()
        {
            // Initialize the dictionary of fire mode strategies
            _fireModeStrategies = new Dictionary<FireModes, FiringPin>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, FiringPin.GetFiringPin(mode));

            // Set the default fire mode and magazine count.
            _currentFireMode = _fireModeStrategies.First().Key;
            InitializeAmo();

            // Initialize the trail renderer pool.
            _trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
            _bulletImpactPool = new ObjectPool<GameObject>(CreateBulletImpact);
        }

        internal void InitializeAmo()
        {
            CurrentAmmo = stats.MagazineSize;
            MaxAmmo = stats.MaxBulletCount;
        }
        
        private GameObject CreateBulletImpact()
        {
            var impact = Instantiate(bulletImpactPrefab);
            impact.SetActive(false);

            return impact;
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

        public override void OnPickup(PlayerController currentPlayerController)
        {
            base.OnPickup(currentPlayerController);
            _opponentLayer = ~currentPlayerController.FriendlyLayer;
        }

        public override void OnDrop()
        {
            base.OnDrop();
            _opponentLayer = 0;
        }

        public override void OnEquip()
        {
            base.OnEquip();
            
            if(CurrentAmmo == 0)
                Reload();
        }


        public override void OnUnequip()
        {
            base.OnUnequip();
            
            if(IsReloading)
            {
                StopCoroutine(_reloadCoroutine);
                _reloading = false;
            }
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

        public override string WeaponID  => stats.WeaponID;

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

        protected override float GetDamage()
        {
            // TODO: Implement era specific damage calculation
            return stats.Damage;
        }

        /// <summary>
        /// Fire the bullet.
        /// </summary>
        private void FireBullet()
        {
            if (CurrentAmmo == 0)
                return;

            var spreadOffset = new Vector3(Random.Range(-stats.Spread, stats.Spread),
                Random.Range(-stats.Spread, stats.Spread), 0); // Add some spread to the bullet.
            var recoilOffset = new Vector3(0, Random.Range(0, stats.Recoil), 0); // Add some recoil to the bullet.
            _recoilFactor = Mathf.Clamp01(_recoilFactor + 0.1f); // Increase the recoil factor.

            // Get the direction of the bullet.
            var fireDirection = muzzle.forward;
            fireDirection = (fireDirection + spreadOffset).normalized;
            fireDirection = (fireDirection + recoilOffset * _recoilFactor).normalized;

            // Raycast to check if the bullet hits something. If it does, play the trail to that point, else play the trail to the miss distance.
            if (Physics.Raycast(muzzle.position, fireDirection, out var hit, stats.MissDistance, _opponentLayer, QueryTriggerInteraction.Ignore))
            {
                StartCoroutine(PlayTrail(muzzle.position, hit.point));
                OnBulletImpact(hit.point, hit.normal);
                if (hit.transform.TryGetComponent<IDamageable>(out var damageable))
                    damageable.TakeDamage(this, GetDamage());
            }
            else
            {
                StartCoroutine(PlayTrail(muzzle.position, muzzle.position + fireDirection * stats.MissDistance));
            }

            // Decrease the magazine count and reload if it's empty.
            if (--CurrentAmmo != 0)
                return;

            Reload();
        }

        /// <summary>
        /// Reload the weapon.
        /// </summary>
        public void Reload()
        {
            if (CurrentAmmo == stats.MagazineSize || _reloading || MaxAmmo == 0)
                return;
            
            if(AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);
            
            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
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
                trail.transform.position = Vector3.Lerp(startPos, endPos,
                    Mathf.Clamp01((distance - remainingDistance) / distance));
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
        /// Function to show the impact of the bullet.
        /// </summary>
        /// <param name="impactPoint">the point at which the bullet is impacted</param>
        /// <param name="impactNormal"></param>
        private async void OnBulletImpact(Vector3 impactPoint, Vector3 impactNormal)
        {
            var impact = _bulletImpactPool.Get();
            impact.transform.position = impactPoint + impactNormal * 0.01f;
            impact.transform.rotation = Quaternion.LookRotation(impactNormal);
            impact.SetActive(true);

            await Task.Delay(3000);

            impact.SetActive(false);
            _bulletImpactPool.Release(impact);
        }

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private IEnumerator ReloadCoroutine()
        {
            if (MaxAmmo == 0)
                yield break;

            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            // Wait for the reload time and then refill the magazine.
            _reloading = true;
            yield return new WaitForSeconds(stats.ReloadTime);

            CurrentAmmo = math.min(stats.MagazineSize, MaxAmmo);
            MaxAmmo -= CurrentAmmo;

            _reloading = false;

            if (Attacking)
                BeginAttack();
        }

        /// <summary>
        /// Function to Add ammo to the weapon when the ammo is picked up.
        /// </summary>
        /// <param name="ammo">the ammo to be added</param>
        public void AddAmmo(int ammo)
        {
            MaxAmmo += ammo;
        }
    }
}