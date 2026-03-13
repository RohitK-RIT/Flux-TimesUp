using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public abstract class AbilityStats : ScriptableObject
    {
        /// <summary>
        /// Cooldown time between ability usage.
        /// </summary>
        public float Cooldown => cooldown;
        
        /// <summary>
        /// Cooldown time between attacks.
        /// </summary>
        [SerializeField] private float cooldown = 5f;
    }
}