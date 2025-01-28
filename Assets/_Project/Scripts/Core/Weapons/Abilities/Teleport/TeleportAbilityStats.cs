using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Teleport
{
    [CreateAssetMenu(fileName = "TAS_LevelName", menuName = "Stats/Ability/Teleport", order = 0)]
    public class TeleportAbilityStats : AbilityStats
    {
        /// <summary>
        /// Travel distance of the teleport ability.
        /// </summary>
        public float Distance => distance;

        /// <summary>
        /// The travel distance of the teleport ability.
        /// </summary>
        [SerializeField] private float distance = 7f;
    }
}
