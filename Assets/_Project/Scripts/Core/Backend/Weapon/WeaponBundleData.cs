using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend.Weapon
{
    [CreateAssetMenu(fileName = "Weapon Bundle Data", menuName = "Bundle Data/ Weapon")]
    public class WeaponBundleData : PrefabBundleData
    {
        [SerializeField] private WeaponData[] weaponData;
#if UNITY_EDITOR
        /// <summary>
        /// Configures the weapon bundle.
        /// This method is used in the Unity Editor to set up the asset bundle for each weapon data.
        /// It iterates through each weapon data, calls its Configure method to set the prefab path,
        /// and adds the weapon prefab to the asset bundle with the specified bundle name.
        /// This is essential for ensuring that the weapon prefabs are correctly included in the asset bundle
        /// when building the game.
        /// </summary>
        protected override void InternalConfigureBundle()
        {
            foreach (var data in weaponData)
            {
                data.Configure();
                AddToAssetBundle(BundleName, data.WeaponPrefab);
            }
        }

        /// <summary>
        /// Configures the weapon bundle data.
        /// This method is used in the Unity Editor to set up the prefab paths for each weapon data.
        /// It iterates through each weapon data and calls its Configure method to set the prefab path.
        /// </summary>
        [ContextMenu("Configure Weapon Bundle Data")]
        private void ConfigureWeaponBundleData()
        {
            foreach (var data in weaponData)
            {
                data.Configure();
            }
        }
#endif

        public WeaponData GetWeaponData(string weaponID)
        {
            foreach (var data in weaponData)
                if (data.Stats.WeaponID == weaponID)
                    return data;

            Debug.LogWarning($"Weapon with ID {weaponID} not found in the bundle data!");
            return null;
        }

        public IEnumerator LoadWeapon(string weaponID, Action<Weapons.Weapon> onComplete)
        {
            var data = GetWeaponData(weaponID);
            if (data.Equals(null))
            {
                Debug.LogWarning($"Weapon with ID {weaponID} not found in the bundle data!");
                onComplete?.Invoke(null);
                yield break;
            }

            yield return LoadPrefabAsync(data.PrefabPath, onComplete);
        }

        public string GetRandomWeaponID()
        {
            if (weaponData.Length != 0)
                return weaponData[Random.Range(0, weaponData.Length)].Stats.WeaponID;

            Debug.LogWarning("Weapon database is empty!");
            return null;
        }
    }
}