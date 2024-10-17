using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Ranged weapon stats.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "RWS_WeaponName", menuName = "Weapons/Weapon Stats/Ranged", order = 0)]
    public class RangedWeaponStats : WeaponStats
    {
        /// <summary>
        /// Allowed fire modes of the weapon.
        /// </summary>
        public FireModes[] FireModes => fireModes;

        /// <summary>
        /// Reload time of the weapon.
        /// </summary>
        public float ReloadTime => reloadTime;

        /// <summary>
        /// Spread of the weapon.
        /// </summary>
        public float Spread => spread;

        /// <summary>
        /// Recoil of the weapon.
        /// </summary>
        public float Recoil => recoil;

        /// <summary>
        /// Magazine size of the weapon.
        /// </summary>
        public int MagazineSize => magazineSize;

        /// <summary>
        /// Burst amount of the weapon.
        /// </summary>
        public int BurstAmount => burstAmount;

        /// <summary>
        /// Burst duration of the weapon.
        /// </summary>
        public float BurstDuration => burstDuration;

        /// <summary>
        /// Trail speed of the weapon.
        /// </summary>
        public float TrailSpeed => trailSpeed;

        /// <summary>
        /// Miss distance of the weapon.
        /// </summary>
        public float MissDistance => missDistance;

        [Header("Ranged Weapon Stats")] [SerializeField]
        private FireModes[] fireModes = { Ranged.FireModes.Auto };

        [SerializeField] private float reloadTime;
        [SerializeField] private float spread;
        [SerializeField] private float recoil;
        [SerializeField] private int magazineSize;
        [SerializeField] private int burstAmount;
        [SerializeField] private float burstDuration;
        [SerializeField] private float trailSpeed = 100f;
        [SerializeField] private float missDistance = 100f;
    }
}