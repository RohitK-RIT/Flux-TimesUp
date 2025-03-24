using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.TSM_Freeze_Ability
{
    [CreateAssetMenu(fileName = "TsmFreezeAS_LevelName", menuName = "Stats/Ability/TSM Freeze", order = 0)]
    public class TsmFreezeAbilityStats : AbilityStats
    {
        /// <summary>
        /// Property to access the duration of the freeze effect.
        /// </summary>
        public float FreezeDuration => freezeDuration;
        
        /// <summary>
        /// The duration of the freeze effect.
        /// </summary>
        [SerializeField] private float freezeDuration = 5f;
        
    }
}
