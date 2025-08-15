using System.Threading.Tasks;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Weapon
{
    [CreateAssetMenu(fileName = "Weapon Bundle Data", menuName = "Bundle Data/ Weapon")]
    public class WeaponBundleData : PrefabBundleData
    {
        [SerializeField] private WeaponData[] weaponData;
#if UNITY_EDITOR
        protected override void InternalConfigureBundle()
        {
            foreach (var data in weaponData)
            {
                data.Configure();
                AddToAssetBundle(BundleName, data.WeaponPrefab);
            }
        }

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

        public async Task<Weapons.Weapon> LoadWeapon(string weaponID)
        {
            var data = GetWeaponData(weaponID);
            if (data != null)
                return await LoadPrefabAsync<Weapons.Weapon>(data.PrefabPath);

            Debug.LogWarning($"Weapon with ID {weaponID} not found in the bundle data!");
            return null;
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