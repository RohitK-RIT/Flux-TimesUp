using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Loadout;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Hand_Controller
{
    /// <summary>
    /// Manages the player's weapons and abilities, allowing for weapon switching and ability usage.
    /// </summary>
    public class HandController : CharacterComponent
    {
        public event Action OnItemSwitched;
        public event Action<int> OnAmmoPicked;
        public event Action<AbilityType> OnAbilityPicked;

        /// <summary>
        /// The parent transform for the weapons.
        /// </summary>
        [SerializeField] private Transform weaponParent;

        ///<summary>
        /// Property to access the weapons.
        /// </summary>
        public Weapon[] Weapons => weapons;

        /// <summary>
        /// Array of all available weapons.
        /// </summary>
        [SerializeField] private Weapon[] weapons;

        /// <summary>
        /// Gets or sets the current weapon. Deactivates the previous weapon and activates the new one.
        /// </summary>
        public IHandItem CurrentItem
        {
            get => _currentItem;
            private set
            {
                if (_currentItem is not null)
                {
                    _currentItem.OnUnequip();
                    if (_currentItem is not Ability)
                    {
                        _currentItem.gameObject.SetActive(false);
                    }
                }

                _currentItem = value;

                if (_currentItem is not null)
                {
                    _currentItem.gameObject.SetActive(true);
                    _currentItem.OnEquip();
                }

                OnItemSwitched?.Invoke();
            }
        }

        /// <summary>
        /// Property to get the current ability.
        /// </summary>
        public Ability CurrentAbility
        {
            get => _currentAbility;
            private set
            {
                if (value is null)
                    return;

                if (_currentAbility is not null)
                {
                    _currentAbility.OnDrop();
                    Destroy(_currentAbility.gameObject);
                }

                _currentAbility = value;
                _currentAbility.OnPickup(PlayerController);
                OnAbilityPicked?.Invoke(value.Type);
            }
        }

        [SerializeField] private bool hasPreMadeLoadout;

        /// <summary>
        /// The index of the current weapon.
        /// </summary>
        private int _currentWeaponIndex;

        /// <summary>
        /// The currently equipped weapon.  
        /// </summary>
        private IHandItem _currentItem;

        /// <summary>
        /// The currently equipped ability.
        /// </summary>
        private Ability _currentAbility;

        private static readonly List<string> PlayerWeaponIDs = new()
        {
            "Pistol3",
            "Sword1"
        };

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);


            if (!hasPreMadeLoadout)
                LoadWeapon(PlayerWeaponIDs);

            // The player controller has picked up all the weapons
            foreach (var weapon in weapons)
                weapon?.OnPickup(PlayerController);

            CurrentItem = weapons[_currentWeaponIndex];
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
                //CurrentAbility.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("Ability Prefab not found");
            }
        }

        /// <summary>
        /// Loads a weapon by its ID.
        /// </summary>
        /// <param name="weaponIDs">The ID of the weapon to load.</param>
        private void LoadWeapon(List<string> weaponIDs)
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
            for (var i = 0; i < weaponIDs.Count; i++)
            {
                var weapon = InstantiateWeapon(weaponIDs[i]);
                weapon.gameObject.SetActive(false);

                weapons[i] = weapon; // Add weapon to the array
            }

            _currentWeaponIndex = 0; // Set the initial index to 0
        }

        // Method to instantiate a weapon prefab based on weapon ID
        private Weapon InstantiateWeapon(string weaponID)
        {
            var weaponPrefab = WeaponDataSystem.Instance.GetWeaponPrefab(weaponID);
            if (weaponPrefab)
            {
                return Instantiate(weaponPrefab, weaponParent);
            }

            Debug.LogError($"Weapon with ID {weaponID} not found in the database!");
            return null;
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

            CurrentItem = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Switches the weapon by a delta value.
        /// </summary>
        /// <param name="slotIndex">The value with which the weapon switches</param>
        public void SwitchWeaponUsingHotkey(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= weapons.Length)
            {
                Debug.LogWarning($"Invalid slot index {slotIndex}.");
                return;
            }

            if (_currentWeaponIndex == slotIndex)
            {
                Debug.Log($"Already using weapon in slot {slotIndex}");
                return;
            }

            _currentWeaponIndex = slotIndex;
            CurrentItem = weapons[_currentWeaponIndex];
            Debug.Log($"[Hotkey] Switched to weapon slot: {_currentWeaponIndex}");
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

            if (CurrentAbility.IsCooldownActive || CurrentAbility.isAbilityActive)
                return;

            CurrentItem = CurrentAbility;

            StartCoroutine(HandleWeaponSwitch(CurrentAbility));
        }

        /// <summary>
        /// Reloads the current weapon.
        /// </summary>
        public void ReloadWeapon()
        {
            if (_currentItem is RangedWeapon rangedWeapon)
                rangedWeapon.OnReload();
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
            CurrentItem = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Begins the attack with the current weapon.
        /// </summary>
        public void BeginAttack()
        {
            CurrentItem.BeginUse();
        }

        /// <summary>
        /// Ends the attack with the current weapon.
        /// </summary>
        public void EndAttack()
        {
            CurrentItem.EndUse();
        }

        public bool OnItemPicked(IPickable pickable)
        {
            switch (pickable)
            {
                case AmmoPickup ammoPickup:
                    OnAmmoCollected(ammoPickup.Ammo);
                    return true;
                case AbilityPickup abilityPickup:
                    return OnAbilitySwitched(abilityPickup.Type);
                case Weapon weapon:
                    return OnWeaponSwitched(weapon);
                default:
                    return false;
            }
        }

        private bool OnAbilitySwitched(AbilityType type)
        {
            if (type == AbilityType.None)
            {
                Debug.LogError("Ability type is None");
                return false;
            }

            if (CurrentAbility?.Type == type)
                return false;

            // Destroy the current ability
            if (CurrentAbility)
                Destroy(CurrentAbility.gameObject);

            // Load the new ability
            LoadAbility(type);

            return true;
        }

        private bool OnWeaponSwitched(Weapon newWeapon)
        {
            if (!newWeapon)
            {
                Debug.LogError("New weapon is null");
                return false;
            }

            var weaponIndex = newWeapon switch
            {
                RangedWeapon => 0,
                MeleeWeapon => 1,
                _ => -1
            };

            if (weaponIndex < 0 || weaponIndex >= weapons.Length)
            {
                Debug.LogError($"Invalid weapon index {weaponIndex} for weapon {newWeapon.WeaponID}");
                return false;
            }

            if (Weapons[weaponIndex])
                DropItem(weaponIndex);

            newWeapon.transform.SetParent(weaponParent);
            Weapons[weaponIndex] = newWeapon;
            newWeapon.OnPickup(PlayerController);

            if (CurrentItem is null)
            {
                CurrentItem = newWeapon;
                _currentWeaponIndex = weaponIndex;
            }
            else
                newWeapon.gameObject.SetActive(false);

            return true;
        }

        private void OnAmmoCollected(int amount)
        {
            OnAmmoPicked?.Invoke(amount);
        }

        public void DropItem()
        {
            // Check if the current item is a weapon
            if (CurrentItem is not Weapon weaponToDrop)
                return;

            // Drop the current item
            DropItem(_currentWeaponIndex);

            SwitchWeapon(1);
            weaponToDrop.gameObject.SetActive(true);
        }

        private void DropItem(int index)
        {
            var weaponToDrop = Weapons[index];

            Weapons[index] = null;
            weaponToDrop.transform.SetParent(null);
            var body = PlayerController.MovementController.Body;
            weaponToDrop.transform.position = body.position + body.forward;
            weaponToDrop.transform.rotation = body.rotation;
            weaponToDrop.OnDrop();
        }
    }
}