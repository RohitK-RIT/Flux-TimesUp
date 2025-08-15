using System.Threading.Tasks;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Weapon
{
    [CreateAssetMenu(fileName = "Weapon Bundle Data", menuName = "Bundle Data/ Weapon")]
    public class WeaponBundleData : BundleData
    {
        [SerializeField] private WeaponData[] weaponData;
#if UNITY_EDITOR
        protected override void InternalConfigureBundle()
        {
            foreach (var data in weaponData)
            {
                AddToAssetBundle(BundleName, data.weaponPrefab);
            }
        }

        [ContextMenu("Configure Weapon Data System")]
        private void ConfigureWeaponDataSystem()
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
            {
                if (data.weaponStats.WeaponID == weaponID)
                {
                    return data;
                }
            }

            Debug.LogWarning($"Weapon with ID {weaponID} not found in the bundle data!");
            return null;
        }

        public async Task<Weapons.Weapon> LoadWeapon(string weaponID)
        {
            foreach (var data in weaponData)
            {
                if (data.weaponStats.WeaponID != weaponID)
                    continue;

                var prefab = await AssetBundleSystem.Instance.LoadAssetAsync<GameObject>(BundleName, data.PrefabPath);
                if (prefab && prefab.TryGetComponent<Weapons.Weapon>(out var weapon))
                    return weapon;
            }

            return null;
        }

        public string GetRandomWeaponID()
        {
            if (weaponData.Length != 0)
                return weaponData[Random.Range(0, weaponData.Length)].weaponStats.WeaponID;

            Debug.LogWarning("Weapon database is empty!");
            return null;
        }
    }
}