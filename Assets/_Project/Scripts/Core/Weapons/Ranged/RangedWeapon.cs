using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Character.Hand_Controller;
using _Project.Scripts.Core.Player_Controllers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Ranged weapon class.
    /// </summary>
    public sealed class RangedWeapon : Weapon, IHandItem
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
        public bool IsReloading => _reloadCoroutine != null;

        public bool IsFiring => _fireCoroutine != null;

        public bool IsTriggerPulled { get; private set; }

        /// <summary>
        /// Current fire mode.
        /// </summary>
        private FireModes CurrentFireMode
        {
            get => _currentFireMode;
            set
            {
                if (_fireModeStrategies.TryGetValue(value, out var firingPin))
                    _currentFiringPin = firingPin;
            }
        }

        public override string WeaponID => stats.WeaponID;

        /// <summary>
        /// Dictionary of fire mode strategies.
        /// </summary>
        private Dictionary<FireModes, FiringPin> _fireModeStrategies;

        /// <summary>
        /// Current firing pin implementation.
        /// </summary>
        private FiringPin _currentFiringPin;

        /// <summary>
        /// Current fire mode enum.
        /// </summary>
        private FireModes _currentFireMode;


        /// <summary>
        /// Object pool for projectiles.
        /// </summary>
        private ObjectPool<Projectile> _projectilePool;

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private Coroutine _reloadCoroutine;

        /// <summary>
        /// Coroutine for firing.
        /// </summary>
        private Coroutine _fireCoroutine;

        private void Start()
        {
            // Initialize the dictionary of fire mode strategies
            _fireModeStrategies = new Dictionary<FireModes, FiringPin>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, FiringPin.GetFiringPin(mode));

            // Set the default fire mode and magazine count.
            CurrentFireMode = _fireModeStrategies.First().Key;
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

        private void OnDestroy()
        {
            // Dispose of the projectile pool.
            _projectilePool?.Dispose();
        }

        public override void OnPickup(PlayerController playerController)
        {
            base.OnPickup(playerController);

            playerController.HandController.OnAmmoPicked += AddAmmo;
        }

        public override void OnDrop()
        {
            base.OnDrop();

            if (IsReloading)
                StopReloading();

            CurrentPlayerController.HandController.OnAmmoPicked -= AddAmmo;
        }

        public override void OnEquip()
        {
            base.OnEquip();

            if (CurrentAmmo == 0)
                StartReloading();
        }


        public override void OnUnequip()
        {
            base.OnUnequip();

            if (IsReloading)
                StopReloading();
        }

        /// <summary>
        /// Cycle through the allowed fire modes.
        /// </summary>
        public void OnSwitchFireMode()
        {
            // End the previous attack if it's still running
            if (IsFiring)
                StopCoroutine(_fireCoroutine);

            // Set the new fire mode
            var indexOf = (Array.IndexOf(stats.FireModes, CurrentFireMode) + 1) % stats.FireModes.Length;
            CurrentFireMode = stats.FireModes[indexOf];
        }

        /// <summary>
        /// Start attacking.
        /// </summary>
        public override void BeginUse()
        {
            // If the weapon is reloading, don't start attacking.
            if (IsReloading) return;

            IsTriggerPulled = true;
            StartFiring();
        }

        public override void EndUse()
        {
            IsTriggerPulled = false;
            StopFiring();
        }

        public void OnReload()
        {
            StopFiring();
            StartReloading();
        }

        private void StartFiring()
        {
            if (IsFiring)
                return;

            if (_currentFiringPin != null)
                _fireCoroutine = StartCoroutine(_currentFiringPin.Fire(stats, FireProjectile));
        }

        private void StopFiring()
        {
            if (!IsFiring)
                return;

            StopCoroutine(_fireCoroutine);
            _fireCoroutine = null;
        }

        private void StartReloading()
        {
            if (CurrentAmmo == stats.MagazineSize || MaxAmmo == 0)
                return;

            if (IsReloading)
                return;

            _reloadCoroutine = StartCoroutine(ReloadCoroutine());
        }

        private void StopReloading()
        {
            if (!IsReloading)
                return;

            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }

        public override IDamageable.DamageInfo GetDamageInfo()
        {
            return new IDamageable.DamageInfo(stats.Damage, this);
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

            if (--CurrentAmmo > 0)
                return;

            StopFiring();
            StartReloading();
        }

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private IEnumerator ReloadCoroutine()
        {
            // Wait for the reload time and then refill the magazine.
            yield return new WaitForSeconds(stats.ReloadTime);

            CurrentAmmo = math.min(stats.MagazineSize, MaxAmmo);
            MaxAmmo -= CurrentAmmo;

            _reloadCoroutine = null;
            if (IsTriggerPulled)
                StartFiring();
        }

        /// <summary>
        /// Function to Add ammo to the weapon when the ammo is picked up.
        /// </summary>
        /// <param name="ammo">the ammo to be added</param>
        private void AddAmmo(int ammo)
        {
            if (stats.WeaponType != WeaponType.Primary && !Equipped)
                return;

            MaxAmmo += ammo;
        }
    }
}