using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

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
        /// Object pool for projectiles.
        /// </summary>
        private ObjectPool<Projectile> _projectilePool;

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

            // Initialize the projectile pool.
            _projectilePool = new ObjectPool<Projectile>(CreateProjectile);
        }

        internal void InitializeAmo()
        {
            CurrentAmmo = stats.MagazineSize;
            MaxAmmo = stats.MaxBulletCount;
        }

        /// <summary>
        /// Function to crate a projectile.
        /// </summary>
        /// <returns>projectile instance</returns>
        private Projectile CreateProjectile()
        {
            var projectile = Instantiate(stats.ProjectilePrefab);
            projectile.gameObject.SetActive(false);

            return projectile;
        }

        public override void OnEquip()
        {
            base.OnEquip();

            if (CurrentAmmo == 0)
                Reload();
        }


        public override void OnUnequip()
        {
            base.OnUnequip();

            if (IsReloading)
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

        public override string WeaponID => stats.WeaponID;

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
                yield return strategy.Fire(stats, FireProjectile);
            else
                Debug.LogError($"No fire mode set for {stats.WeaponName}", stats);
        }

        public override float GetDamage()
        {
            // TODO: Implement era specific damage calculation
            return stats.Damage;
        }

        /// <summary>
        /// Fire a projectile from the weapon.
        /// </summary>
        private void FireProjectile()
        {
            if (CurrentAmmo <= 0)
                return;

            var projectile = _projectilePool.Get();
            projectile.transform.position = muzzle.position;
            projectile.transform.rotation = muzzle.rotation;
            projectile.Initialize(this);
            projectile.gameObject.SetActive(true);
            projectile.OnHit += theProjectile => { _projectilePool.Release(theProjectile); };

            if (--CurrentAmmo <= 0)
                Reload();
        }

        /// <summary>
        /// Reload the weapon.
        /// </summary>
        public void Reload()
        {
            if (CurrentAmmo == stats.MagazineSize || _reloading || MaxAmmo == 0)
                return;

            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
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