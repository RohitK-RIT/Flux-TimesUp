using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Character.IK_Points;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    /// <summary>
    /// Manages the player's weapons and abilities, allowing for weapon switching and ability usage.
    /// </summary>
    public class WeaponController : CharacterComponent
    {

        /// <summary>
        /// The parent transform for the weapons.
        /// </summary>
        [SerializeField] private Transform weaponParent;

        /// <summary>
        /// The currently equipped weapon.
        /// </summary>
        [SerializeField] private Weapon currentWeapon;
        
        ///<summary>
        /// Property to access the weapons.
        /// </summary>
        public Weapon[] Weapons => weapons;
        
        /// <summary>
        /// Array of all available weapons.
        /// </summary>
        [SerializeField] private Weapon[] weapons;

        private IKPoints ikPoints;
        /// <summary>
        /// Gets or sets the current weapon. Deactivates the previous weapon and activates the new one.
        /// </summary>
        public Weapon CurrentWeapon
        {
            get => currentWeapon;
            private set
            {
                if (!value)
                {
                    Debug.LogError("Weapon not found");
                    return;
                }

                if (currentWeapon)
                {
                    currentWeapon.OnUnequip();
                    currentWeapon.gameObject.SetActive(false);
                }

                currentWeapon = value;

                currentWeapon.gameObject.SetActive(true);
                currentWeapon.OnEquip();
            }
        }

        /// <summary>
        /// Gets the current ability.
        /// </summary>
        public Ability CurrentAbility { get; private set; }

        public static WeaponController Instance { get; set; }

        [SerializeField] private bool hasPreMadeLoadout;

        /// <summary>
        /// The index of the current weapon.
        /// </summary>
        private int _currentWeaponIndex;
        
        void Awake()
        {
            ikPoints = GetComponent<IKPoints>(); // Fetch the singleton instance of WeaponController
        }

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);
            
            // Fetch selected weapons from WeaponDataSystem
            if (!hasPreMadeLoadout)
            {
                var selectedLoadoutWeaponIDs = WeaponDataSystem.Instance.GetSelectedWeapons();
                if (selectedLoadoutWeaponIDs is { Count: > 0 })
                {
                    LoadWeapon(selectedLoadoutWeaponIDs);
                }
                else
                {
                    Debug.LogError("No selected weapons found in WeaponDataSystem");
                }
            }
            
            // The player controller has picked up all the weapons
            foreach (var weapon in weapons)
                weapon?.OnPickup(PlayerController);
        }

        /// <summary>
        /// Loads an ability by its type.
        /// </summary>
        /// <param name="abilityType">type of the ability</param>
        private void LoadAbility(AbilityType abilityType)
        {
            // Check if the player has no ability
            if (abilityType == AbilityType.None)
                return;

            // Get the ability prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPrefab(abilityType);
            // Check if the ability prefab is not null
            if (abilityPrefab)
            {
                // Instantiate the ability prefab
                CurrentAbility = Instantiate(abilityPrefab, weaponParent);
                CurrentAbility.gameObject.SetActive(false);
                CurrentAbility.OnPickup(PlayerController);
            }
            else
            {
                Debug.LogError("Ability Prefab not found");
            }
        }

        /// <summary>
        /// Loads a weapon by its ID.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon to load.</param>
        internal void LoadWeapon(List<string> weaponIDs)
        {
            // Validate input
            if (weaponIDs == null || weaponIDs.Count == 0)
            {
                Debug.LogError("No weapon IDs provided!");
                return;
            }

            // Initialize the weapons array
            weapons = new Weapon[weaponIDs.Count];

            // Instantiate all weapons but only activate the first one
            for (int i = 0; i < weaponIDs.Count; i++)
            {
                var weapon = InstantiateWeapon(weaponIDs[i]);

                if (i == 0 & currentWeapon== null)
                {
                    // Equip and activate the first weapon
                    currentWeapon = weapon;
                    currentWeapon.gameObject.SetActive(true);
                    currentWeapon.OnEquip();
                }
                else
                {
                    // Deactivate all other weapons
                    weapon.gameObject.SetActive(false);
                }

                weapons[i] = weapon; // Add weapon to the array
            }

            _currentWeaponIndex = 0; // Set the initial index to 0

        }
        
        // Method to instantiate a weapon prefab based on weapon ID
        private Weapon InstantiateWeapon(string weaponID)
        {
            Weapon weaponPrefab = WeaponDataSystem.Instance.GetWeaponPrefab(weaponID);
            if (weaponPrefab != null)
            {
                return Instantiate(weaponPrefab, weaponParent);
            }
            else
            {
                Debug.LogError($"Weapon with ID {weaponID} not found in the database!");
                return null;
            }
        }

        /// <summary>
        /// Switches the weapon by a delta value.
        /// </summary>
        /// <param name="delta">The value with which the weapon switches</param>
        public void SwitchWeapon(int delta)
        {
            Debug.LogWarning("Switching weapon");
            _currentWeaponIndex += delta;
            if (_currentWeaponIndex < 0)
                _currentWeaponIndex = weapons.Length - 1;
            else if (_currentWeaponIndex >= weapons.Length)
                _currentWeaponIndex = 0;
            
            CurrentWeapon = weapons[_currentWeaponIndex];
            ikPoints.UpdateIKPoints();
        }

        /// <summary>
        /// Equips the current ability and starts the weapon switch coroutine.
        /// </summary>
        public void OnAbilityEquipped()
        {
            if (!CurrentAbility)
            {
                Debug.LogError("Ability not found");
                return;
            }

            CurrentWeapon = CurrentAbility;

            StartCoroutine(HandleWeaponSwitch(CurrentAbility));
        }

        /// <summary>
        /// Reloads the current weapon.
        /// </summary>
        public void ReloadWeapon()
        {
            if (currentWeapon is RangedWeapon rangedWeapon)
                rangedWeapon.Reload();
        }

        /// <summary>
        /// Handles the weapon switch when an ability is used.
        /// </summary>
        /// <param name="ability">The ability being used.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator HandleWeaponSwitch(Ability ability)
        {
            // Handle weapon switch
            yield return new WaitUntil(() => ability.Used);
            CurrentWeapon = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Begins the attack with the current weapon.
        /// </summary>
        public void BeginAttack()
        {
            CurrentWeapon.BeginAttack();
        }

        /// <summary>
        /// Ends the attack with the current weapon.
        /// </summary>
        public void EndAttack()
        {
            CurrentWeapon.EndAttack();
        }
        
        /// <summary>
        /// Switches the ability to the specified type.
        /// </summary>
        /// <param name="abilityType">type of the ability</param>
        public void SwitchAbility(AbilityType abilityType)
        {
            if (abilityType == AbilityType.None)
            {
                Debug.LogError("Ability type is None");
                return;
            }

            // Destroy the current ability
            if (CurrentAbility)
                Destroy(CurrentAbility.gameObject);

            // Load the new ability
            LoadAbility(abilityType);
        }
    }
}