using System;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    /// <summary>
    /// Base scriptable object for weapon stats.
    /// </summary>
    [Serializable]
    public abstract class WeaponStats : ScriptableObject
    {
        /// <summary>
        /// Weapon ID.
        /// </summary>
        public string WeaponID => weaponID;

        /// <summary>
        /// Weapon type.
        /// </summary>
        public WeaponType WeaponType => weaponType;

        /// <summary>
        /// Weapon name.
        /// </summary>
        public string WeaponName => weaponName;

        /// <summary>
        /// Damage of the weapon.
        /// </summary>
        public float Damage => damage;

        /// <summary>
        /// Range of the weapon.
        /// </summary>
        public float Range => range;

        /// <summary>
        /// Attack speed of the weapon.
        /// </summary>
        public float AttackSpeed => attackSpeed;
        
        /// <summary>
        /// Time stability effect of the weapon.
        /// </summary>
        public float TimeStabilityEffect => timeStabilityEffect;
        
        /// <summary>
        /// Weapon era.
        /// </summary>
        public WeaponEra WeaponEra => weaponEra;

        [Header("Weapon Stats")] [SerializeField]
        private string weaponID;

        [SerializeField] private WeaponType weaponType;
        [SerializeField] private string weaponName;
        [SerializeField] private float damage;
        [SerializeField] private float range;
        [SerializeField] private float attackSpeed;
        [SerializeField] private WeaponEra weaponEra;
        [SerializeField] private float timeStabilityEffect = 3f;
    }
}