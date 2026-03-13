using System.Linq;
using UnityEngine;
using _Project.Scripts.Core.Weapons.Abilities;

namespace _Project.Scripts.Core.Backend.Ability
{
    /// <summary>
    /// System for managing ability data.
    /// </summary>
    public class AbilityDataSystem : BaseSystem<AbilityDataSystem>
    {
        protected override bool IsPersistent => true;

        /// <summary>
        /// The ability data.
        /// </summary>
        [SerializeField] private AbilityData[] abilityData;

        /// <summary>
        /// Gets the ability prefab.
        /// </summary>
        /// <param name="type">type of ability</param>
        /// <returns>player ability prefab</returns>
        public Weapons.Abilities.Ability GetAbilityPrefab(AbilityType type)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
                return null;

            // Get the ability prefab
            var prefab = data.AbilityPrefab;
            // If the prefab is not null, return the prefab
            if (prefab)
                return prefab;

            // Log an error if the prefab is not found
            Debug.LogError($"Ability prefab for {type} not found.");
            return null;
        }

        public AbilityPickup GetAbilityPickupPrefab(AbilityType type)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
                return null;

            // Get the ability pickup
            var pickup = data.AbilityPickup;
            // If the pickup is not null, return the pickup
            if (pickup)
                return pickup;

            // Log an error if the pickup is not found
            Debug.LogError($"Ability pickup for {type} not found.");
            return null;
        }

        /// <summary>
        /// Gets the ability data.
        /// </summary>
        /// <param name="type">type of the ability</param>
        /// <returns>ability data of the specified type</returns>
        public AbilityData GetAbilityData(AbilityType type)
        {
            // Get the ability data
            var data = abilityData.FirstOrDefault(data => data.Type == type);
            // If the data is not null, return the data
            if (data != null)
                return data;

            // Log an error if the data is not found
            Debug.LogError($"AbilityData object for {type} not found.");
            return null;
        }
    }
}