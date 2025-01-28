using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Heal
{
    [CreateAssetMenu(fileName = "HAS_LevelName", menuName = "Stats/Ability/Heal", order = 0)]
    public class HealAbilityStats : AbilityStats
    {
        /// <summary>
        /// The amount of health restored by the ability.
        /// </summary>
        public int HealValue => healValue;

        /// <summary>
        /// The amount of health restored by the ability.   
        /// </summary>
        [SerializeField] private int healValue = 50;
    }
}