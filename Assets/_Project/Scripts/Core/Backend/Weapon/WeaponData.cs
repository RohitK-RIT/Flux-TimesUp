using System;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Weapon
{
    [Serializable]
    public class WeaponData
    {
        /// <summary>
        /// The icon representing the weapon, used for UI purposes.
        /// </summary>
        public Sprite Icon => icon;

        /// <summary>
        /// The stats associated with the weapon, including damage, range, and other attributes.
        /// </summary>
        public WeaponStats Stats => weaponStats;

        /// <summary>
        /// The path to the prefab in the project, used for loading the prefab at runtime.
        /// </summary>
        public string PrefabPath => prefabPath;

#if UNITY_EDITOR
        public Weapons.Weapon WeaponPrefab => weaponPrefab;

        /// <summary>
        /// The weapon prefab that will be instantiated in the game.
        /// </summary>
        [SerializeField] private Weapons.Weapon weaponPrefab;
#endif

        /// <summary>
        /// The icon representing the weapon, used for UI purposes.
        /// </summary>
        [SerializeField] private Sprite icon;

        /// <summary>
        /// The stats associated with the weapon, including damage, range, and other attributes.
        /// </summary>
        [SerializeField] private WeaponStats weaponStats;

        /// <summary>
        /// The path to the prefab in the project, used for loading the prefab at runtime.
        /// </summary>
        [SerializeField, HideInInspector] private string prefabPath;

#if UNITY_EDITOR
        public void Configure()
        {
            prefabPath = UnityEditor.AssetDatabase.GetAssetPath(weaponPrefab);
        }
#endif
    }
}