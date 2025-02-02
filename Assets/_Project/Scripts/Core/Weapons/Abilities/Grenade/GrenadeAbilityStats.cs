using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Grenade
{
    [CreateAssetMenu(fileName = "GAS_LevelName", menuName = "Stats/Ability/Grenade", order = 0)]
    public class GrenadeAbilityStats : AbilityStats
    {
        ///<summary>
        /// The delay before the grenade explodes.
        /// </summary>
        public float Delay => delay;
        
        ///<summary>
        /// The delay before the grenade explodes.
        /// </summary>
        [SerializeField] private float delay = 3f;
        
        /// <summary>
        /// The amount of damage dealt by the ability.
        /// </summary>
        public int Damage => damage;
        
        /// <summary>
        /// The amount of damage dealt by the ability.
        /// </summary>
        [SerializeField] private int damage = 90;
        
        /// <summary>
        /// The throwing force of the grenade.
        /// </summary>
        public float Force => force;
        
        /// <summary>
        /// The throwing force of the grenade.
        /// </summary>
        [SerializeField] private float force = 700f;
        
        ///<summary>
        /// The Throwing Range of the grenade.
        /// </summary>
        public float Range => range;
        
        ///<summary>
        /// The Throwing Range of the grenade.
        /// </summary>
        [SerializeField] private float range = 10f;
      
        /// <summary>
        /// The effect radius of the grenade.
        /// </summary>
        public float Radius => radius;
        
        /// <summary>
        /// The effect radius of the grenade.
        /// </summary>
        [SerializeField] private float radius = 4f;
    }
}
