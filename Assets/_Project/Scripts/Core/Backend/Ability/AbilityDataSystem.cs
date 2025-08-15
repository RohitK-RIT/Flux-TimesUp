using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Asset_Bundle;
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
        /// The ability bundle data.
        /// This is used to load the ability data from an asset bundle.
        /// </summary>
        [SerializeField] private AbilityBundleData abilityBundleData;

        private IEnumerator Start()
        {
            // Load the ability bundle data asynchronously.
            yield return AssetBundleSystem.Instance.LoadBundleAsync(abilityBundleData.BundleName);
        }

        /// <summary>
        /// Gets the ability prefab.
        /// </summary>
        /// <param name="type">type of ability</param>
        /// <param name="onComplete">callback for the loaded prefab</param>
        /// <returns>player ability prefab</returns>
        public IEnumerator GetAbilityPrefab(AbilityType type, Action<Weapons.Abilities.Ability> onComplete)
        {
            yield return abilityBundleData.GetAbilityPrefab(type, onComplete);
        }

        /// <summary>
        /// Gets the ability pickup prefab.
        /// </summary>
        /// <param name="type">type of ability</param>
        /// <param name="onComplete">callback for the loaded prefab</param>
        /// <returns>ability pickup prefab</returns>
        public IEnumerator GetAbilityPickupPrefab(AbilityType type, Action<AbilityPickup> onComplete)
        {
            yield return abilityBundleData.GetAbilityPickupPrefab(type, onComplete);
        }

        /// <summary>
        /// Gets the ability data.
        /// </summary>
        /// <param name="type">type of the ability</param>
        /// <returns>ability data of the specified type</returns>
        public AbilityData GetAbilityData(AbilityType type)
        {
            return abilityBundleData.GetAbilityData(type);
        }
    }
}