using System;
using System.Collections;
using _Project.Scripts.Core.Backend.Asset_Bundle;
using UnityEngine;

namespace _Project.Scripts.Core.Backend.Weapon
{
    // It fetches weapon prefabs based on weapon IDs and stores the selected weapons for gameplay.
    public class WeaponDataSystem : BaseSystem<WeaponDataSystem>
    {
        protected override bool IsPersistent => true;

        public bool Initialized { get; private set; }

        /// <summary>
        /// The weapon bundle data that contains all the weapon data and configurations.
        /// This is used to load and manage the weapon data in the game.
        /// </summary>
        [SerializeField] private WeaponBundleData weaponBundleData;

        private IEnumerator Start()
        {
            // Load the weapon bundle asynchronously.
            yield return AssetBundleSystem.Instance.LoadBundleAsync(weaponBundleData.BundleName, success => Initialized = success);
        }

        public IEnumerator GetWeaponAsync(string weaponID, Action<Weapons.Weapon> onComplete)
        {
            yield return weaponBundleData.LoadWeapon(weaponID, onComplete); // Load the weapon prefab from the bundle
        }

        // Fetches the weapon icon based on the provided WeaponID
        public Sprite GetWeaponIcon(string weaponID)
        {
            var weaponData = weaponBundleData.GetWeaponData(weaponID);
            return weaponData?.Icon; // Return the icon from the weapon data
        }

        //Sets the selected weapons by copying the provided list of weapon IDs.
        public void SetSelectedWeapons(string[] selectedWeapons) { }

        //Retrieves the info of a weapon based on its ID.
        public WeaponData GetWeaponInfo(string weaponID)
        {
            return weaponBundleData.GetWeaponData(weaponID);
        }

        public string GetRandomWeaponID()
        {
            return weaponBundleData.GetRandomWeaponID();
        }
    }
}