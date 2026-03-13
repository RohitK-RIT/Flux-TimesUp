using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    /// <summary>
    /// Melee weapon stats.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "MWS_WeaponName", menuName = "Weapons/Stats/Melee", order = 1)]
    public class MeleeWeaponStats : WeaponStats
    {
        public float AttackFOV => attackFOV;
        
        [SerializeField] private float attackFOV;
    }
}