using System;
using UnityEngine;
using _Project.Scripts.Core.Weapons.Abilities;

namespace _Project.Scripts.Core.Backend.Ability
{
    [Serializable]
    public class AbilityData
    {
        /// <summary>
        /// The name of the ability.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The type of the ability.
        /// </summary>
        public AbilityType Type => type;

        /// <summary>
        /// The description of the ability.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// The icon of the ability.
        /// </summary>
        public Sprite Icon => icon;

#if UNITY_EDITOR
        /// <summary>
        /// The ability prefab.
        /// </summary>
        public Weapons.Abilities.Ability AbilityPrefab => abilityPrefab;

        /// <summary>
        /// The ability pickup prefab.
        /// </summary>
        public AbilityPickup AbilityPickup => abilityPickup;
#endif

        /// <summary>
        /// The path to the ability prefab in the project, used for loading the prefab at runtime.
        /// </summary>
        public string PrefabPath => prefabPath;

        /// <summary>
        /// The path to the ability pickup prefab in the project, used for loading the prefab at runtime.
        /// </summary>
        public string PickupPrefabPath => pickupPrefabPath;

        [SerializeField] private string name;
        [SerializeField] private AbilityType type;
        [SerializeField, Multiline] private string description;
        [SerializeField] private Sprite icon;

#if UNITY_EDITOR
        [Space] [SerializeField] private Weapons.Abilities.Ability abilityPrefab;
        [SerializeField] private AbilityPickup abilityPickup;
#endif
        [SerializeField, HideInInspector] private string prefabPath;
        [SerializeField, HideInInspector] private string pickupPrefabPath;

#if UNITY_EDITOR
        public void Configure()
        {
            prefabPath = UnityEditor.AssetDatabase.GetAssetPath(abilityPrefab);
            pickupPrefabPath = UnityEditor.AssetDatabase.GetAssetPath(abilityPickup);
        }
#endif
    }
}