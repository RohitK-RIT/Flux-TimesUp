using System.Collections.Generic;
using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Loadout
{
    // It fetches weapon prefabs based on weapon IDs and stores the selected weapons for gameplay.
    public class WeaponDataSystem : BaseSystem<WeaponDataSystem>
    {
        // Array that holds the weapon database, which contains all available weapon data.
        [SerializeField] internal WeaponData[] weaponDatabase;
        
        // List to store the IDs of the selected weapons for the player.
        private List<string> _selectedWeapons;
        
        // Fetches the weapon prefab based on the provided WeaponID
        public Weapon GetWeaponPrefab(string weaponID)
        {
            foreach (var weaponData in weaponDatabase) // Iterate through weapon database
            {
                if (weaponData.weaponStats.WeaponID == weaponID) // Check the WeaponID property
                {
                    return weaponData.weaponPrefab; // Return the prefab from the current weapon data
                }
                
            }

            Debug.LogWarning($"Weapon with ID {weaponID} not found in the database!");
            return null;
        }
        
        // Fetches the weapon icon based on the provided WeaponID
        public Sprite GetWeaponIcon(string weaponID)
        {
            foreach (var weaponData in weaponDatabase) // Iterate through weapon database
            {
                if (weaponData.weaponStats.WeaponID == weaponID) // Check the WeaponID property
                {
                    return weaponData.icon; // Return the icon from the current weapon data
                }
                
            }
            Debug.LogWarning($"Weapon with ID {weaponID} not found in the database!");
            return null;
        }

        protected override bool IsPersistent => true;
        
        //Sets the selected weapons by copying the provided list of weapon IDs.
        public void SetSelectedWeapons(string[] selectedWeapons)
        {
            _selectedWeapons = new List<string>(selectedWeapons);
        }

        //Retrieves the list of selected weapons.
        public List<string> GetSelectedWeapons()
        {
            return _selectedWeapons;
        }
        
        //Retrieves the info of a weapon based on its ID.
        
        public WeaponData GetWeaponInfo(string weaponID)
        {
            foreach (var weaponData in weaponDatabase) // Iterate through weapon database
            {
                if (weaponData.weaponStats.WeaponID == weaponID) // Check the WeaponID property
                {
                    return weaponData; // Return the weapon stats from the current weapon data
                }
                
            }
            Debug.LogWarning($"Weapon with ID {weaponID} not found in the database!");
            return null;
        }
    }
}
