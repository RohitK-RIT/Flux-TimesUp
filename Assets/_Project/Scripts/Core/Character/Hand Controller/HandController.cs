using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Weapon;
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
        public event Action<IHandItem> OnItemPicked;
        public event Action<IHandItem> OnItemDropped;

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

        public bool Initialized { get; private set; }

        /// <summary>
        /// Gets or sets the current weapon. Deactivates the previous weapon and activates the new one.
        /// </summary>
        public IHandItem CurrentItem
        {
            get => _currentItem;
            private set
            {
                if (Equals(value, _currentItem))
                    return;

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

                if (_currentAbility)
                {
                    _currentAbility.OnDrop();
                    Destroy(_currentAbility.gameObject, 2f);
                }

                _currentAbility = value;
                _currentAbility.OnPickup(PlayerController);
                OnItemPicked?.Invoke(value);
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

        private IEnumerator Start()
        {
            if (!hasPreMadeLoadout)
            {
                yield return new WaitUntil(() => WeaponDataSystem.Instance.Initialized);
                yield return LoadWeapon(PlayerWeaponIDs);
            }
            else
            {
                foreach (var weapon in weapons)
                {
                    weapon.OnPickup(PlayerController);
                    OnItemPicked?.Invoke(weapon);
                }
            }

            _currentWeaponIndex = 0;
            CurrentItem = weapons[_currentWeaponIndex];

            Initialized = true;
        }

        /// <summary>
        /// Loads an ability by its type.
        /// </summary>
        /// <param name="abilityType">type of the ability</param>
        private IEnumerator LoadAbility(AbilityType abilityType)
        {
            // Check if the player has no ability
            if (abilityType == AbilityType.None)
                yield break;

            // Get the ability prefab
            Ability abilityPrefab = null;
            yield return AbilityDataSystem.Instance.GetAbilityPrefab(abilityType, prefab => abilityPrefab = prefab);

            // Check if the ability prefab is not null
            if (abilityPrefab)
            {
                // Instantiate the ability prefab
                CurrentAbility = Instantiate(abilityPrefab, weaponParent);
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
        private IEnumerator LoadWeapon(List<string> weaponIDs)
        {
            // Validate input
            if (weaponIDs == null || weaponIDs.Count == 0)
            {
                Debug.LogError("No weapon IDs provided!");
                yield break;
            }

            // Initialize the weapons array
            weapons = new Weapon[weaponIDs.Count];

            // Instantiate all weapons but only activate the first one
            for (var i = 0; i < weaponIDs.Count; i++)
            {
                // Wait for the weapon prefab to be loaded
                Weapon weaponPrefab = null;
                yield return WeaponDataSystem.Instance.GetWeaponAsync(weaponIDs[i], prefab => weaponPrefab = prefab);

                if (!weaponPrefab)
                    continue;

                // Instantiate the weapon prefab
                var weapon = Instantiate(weaponPrefab, weaponParent);
                weapon.gameObject.SetActive(false);

                PickupWeapon(i, weapon);
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
            CurrentItem?.BeginUse();
        }

        /// <summary>
        /// Ends the attack with the current weapon.
        /// </summary>
        public void EndAttack()
        {
            CurrentItem?.EndUse();
        }

        public bool OnItemInteracted(IInteractable interactable)
        {
            switch (interactable)
            {
                case AbilityPickup abilityPickup:
                    return OnAbilitySwitched(abilityPickup.Type);
                case Weapon weapon:
                    return OnWeaponSwitched(weapon);
                default:
                    Debug.LogWarning("Unknown interactable type");
                    return false;
            }
        }

        public bool OnItemCollected(ICollectible collectible)
        {
            switch (collectible)
            {
                case AmmoPickup ammoPickup:
                    return OnAmmoCollected(ammoPickup.Ammo);
                case RangedWeapon rangedWeapon:
                    var currentRangedWeapon = weapons[0] as RangedWeapon;
                    if (currentRangedWeapon)
                    {
                        if (currentRangedWeapon.WeaponID != rangedWeapon.WeaponID)
                            return false;

                        currentRangedWeapon.AddAmmo(rangedWeapon.MaxAmmo);
                        return true;
                    }

                    PickupWeapon(0, rangedWeapon);
                    if (_currentWeaponIndex != 0)
                        rangedWeapon.gameObject.SetActive(false);
                    return true;
                case MeleeWeapon meleeWeapon:
                    if (weapons[1])
                        return false;

                    PickupWeapon(1, meleeWeapon);
                    if (_currentWeaponIndex != 1)
                        meleeWeapon.gameObject.SetActive(false);
                    return true;

                default:
                    Debug.LogWarning("Unknown collectible type");
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
            StartCoroutine(LoadAbility(type));

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
                MeleeWeapon => 1
            };

            var oldWeapon = weapons[weaponIndex];

            if (weaponIndex == 0 && oldWeapon.WeaponID == newWeapon.WeaponID)
            {
                var oldRangedWeapon = oldWeapon as RangedWeapon;
                var newRangedWeapon = newWeapon as RangedWeapon;

                if (oldRangedWeapon && newRangedWeapon)
                {
                    oldRangedWeapon.AddAmmo(newRangedWeapon.MaxAmmo);
                    return true;
                }
            }

            if (oldWeapon)
                DropWeapon(weaponIndex);

            PickupWeapon(weaponIndex, newWeapon);
            CurrentItem = weapons[_currentWeaponIndex];
            oldWeapon.gameObject.SetActive(true);
            if (weaponIndex != _currentWeaponIndex)
                weapons[weaponIndex].gameObject.SetActive(false);

            return true;
        }

        private bool OnAmmoCollected(int amount)
        {
            var currentRangedWeapon = weapons[0] as RangedWeapon;
            if (!currentRangedWeapon)
            {
                Debug.LogWarning("No ranged weapon found to collect ammo for.");
                return false;
            }

            currentRangedWeapon.AddAmmo(amount);
            return true;
        }

        private void PickupWeapon(int weaponIndex, Weapon newWeapon)
        {
            if (!newWeapon)
            {
                Debug.LogError("Weapon is null");
                return;
            }

            weapons[weaponIndex] = newWeapon;
            newWeapon.transform.SetParent(weaponParent);
            newWeapon.OnPickup(PlayerController);

            // Auto-equip if player currently has no item in hand
            if (CurrentItem == null)
            {
                _currentWeaponIndex = weaponIndex;
                CurrentItem = newWeapon;
            }

            OnItemPicked?.Invoke(newWeapon);
        }

        public void DropWeapon()
        {
            // Check if the current item is a weapon
            if (CurrentItem is not Weapon weaponToDrop)
                return;

            // Drop the current item
            DropWeapon(_currentWeaponIndex);

            SwitchWeapon(1);
            weaponToDrop.gameObject.SetActive(true);
        }

        private void DropWeapon(int index)
        {
            var weaponToDrop = weapons[index];

            weapons[index] = null;
            weaponToDrop.transform.SetParent(null);
            var body = PlayerController.MovementController.Body;
            weaponToDrop.transform.position = body.position + body.forward;
            weaponToDrop.transform.rotation = body.rotation;
            weaponToDrop.OnDrop();
            weaponToDrop.gameObject.SetActive(true);

            OnItemDropped?.Invoke(weaponToDrop);
        }
    }
}