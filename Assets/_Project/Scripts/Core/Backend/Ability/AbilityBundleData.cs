using System;
using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Ability
{
    [CreateAssetMenu(fileName = "Ability Bundle Data", menuName = "Bundle Data/Ability")]
    public class AbilityBundleData : PrefabBundleData
    {
        [SerializeField] private AbilityData[] abilityData;
#if UNITY_EDITOR
        protected override void InternalConfigureBundle()
        {
            foreach (var data in abilityData)
            {
                data.Configure();
                AddToAssetBundle(BundleName, data.AbilityPrefab);
                AddToAssetBundle(BundleName, data.AbilityPickup);
            }
        }

        [ContextMenu("Configure Ability Bundle Data")]
        private void ConfigureAbilityBundleData()
        {
            foreach (var data in abilityData)
            {
                data.Configure();
            }
        }
#endif

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

        /// <summary>
        /// Gets the ability prefab.
        /// This method loads the ability prefab asynchronously and invokes the callback with the loaded prefab.
        /// </summary>
        /// <param name="type">type of ability</param>
        /// <param name="onComplete">callback to get the prefab</param>
        public IEnumerator GetAbilityPrefab(AbilityType type, Action<Weapons.Abilities.Ability> onComplete)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            yield return LoadPrefabAsync(data.PrefabPath, onComplete);
        }

        /// <summary>
        /// Gets the ability pickup prefab.
        /// This method loads the ability pickup prefab asynchronously and invokes the callback with the loaded prefab.
        /// </summary>
        /// <param name="type">type of ability pickup</param>
        /// <param name="onComplete">callback to get the prefab</param>
        /// <returns></returns>
        public IEnumerator GetAbilityPickupPrefab(AbilityType type, Action<AbilityPickup> onComplete)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
            {
                onComplete?.Invoke(null);
                yield break;
            }

            // Get the ability pickup
            yield return LoadPrefabAsync(data.PickupPrefabPath, onComplete);
        }
    }
}