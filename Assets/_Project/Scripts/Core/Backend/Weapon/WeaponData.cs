using System;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Weapon
{
    [Serializable]
    public class WeaponData
    {
        public string PrefabPath => prefabPath;

#if UNITY_EDITOR
        /// <summary>
        /// The weapon prefab that will be instantiated in the game.
        /// </summary>
        [SerializeField] internal Weapons.Weapon weaponPrefab;
#endif

        /// <summary>
        /// The icon representing the weapon, used for UI purposes.
        /// </summary>
        [SerializeField] internal Sprite icon;

        /// <summary>
        /// The stats associated with the weapon, including damage, range, and other attributes.
        /// </summary>
        [SerializeField] internal WeaponStats weaponStats;

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