using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    public class LocalInputController : InputController
    {
        /// <summary>
        /// Event that is called when the player moves the character.
        /// </summary>
        public override event Action<Vector2> OnMoveInputUpdated;

        /// <summary>
        /// Event that is called when the player looks around.
        /// </summary>
        public event Action<Vector2> OnLookInputUpdated;

        /// <summary>
        /// Event that is called when the player starts attacking.
        /// </summary>
        public override event Action OnAttackInputBegan;

        /// <summary>
        /// Event that is called when the player stops attacking.
        /// </summary>
        public override event Action OnAttackInputEnded;

        /// <summary>
        /// Event that is called when the player equips ability.
        /// </summary>
        public override event Action OnAbilityEquipped;

        /// <summary>
        /// Event that is called when the player switches weapons.
        /// </summary>
        public override event Action<int> OnSwitchWeaponInput;

        [SerializeField] private float scrollCooldown = 0.25f; // Cooldown for weapon switching
        private float _lastScrollTime;
        public virtual event Action<int> OnSwitchWeaponHotkey;

        public override event Action OnReloadInput;

        public virtual event Action OnLootPickupInput;

        public event Action OnDropInput;

        /// <summary>
        /// Component that handles player input Unity API calls.
        /// </summary>
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        public void OnEnable()
        {
            // Subscribe to input events
            // Look Input Events
            _playerInput.Character.Look.performed += OnLookInputReceived;
            _playerInput.Character.Look.canceled += OnLookInputReceived;

            // Move Input Events
            _playerInput.Character.Move.started += OnMoveInputReceived;
            _playerInput.Character.Move.performed += OnMoveInputReceived;
            _playerInput.Character.Move.canceled += OnMoveInputReceived;

            // Attack Input Events
            _playerInput.Character.Use.started += OnUseStarted;
            _playerInput.Character.Use.canceled += OnUseCancelled;

            // Item Input Events
            _playerInput.Character.EquipAbility.performed += OnEquipAbilityInput;
            _playerInput.Character.SwitchWeapon.performed += OnSwitchWeaponInputReceived;
            _playerInput.Character.Drop.performed += OnDropPerformed;
            _playerInput.Character.SwitchWeaponHotkey.performed += OnSwitchWeaponHotkeyInput;
            _playerInput.Character.Reload.performed += OnReloadInputReceived;
            _playerInput.Character.Pick.performed += OnPickPerformed;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Enable the PlayerInput component
            _playerInput.Character.Enable();
        }

        public void OnDisable()
        {
            // Unsubscribe from input events
            // Look Input Events
            _playerInput.Character.Look.performed -= OnLookInputReceived;
            _playerInput.Character.Look.canceled -= OnLookInputReceived;

            // Move Input Events
            _playerInput.Character.Move.started -= OnMoveInputReceived;
            _playerInput.Character.Move.performed -= OnMoveInputReceived;
            _playerInput.Character.Move.canceled -= OnMoveInputReceived;

            // Use Input Events
            _playerInput.Character.Use.started -= OnUseStarted;
            _playerInput.Character.Use.canceled -= OnUseCancelled;

            // Item Input Events
            _playerInput.Character.EquipAbility.performed -= OnEquipAbilityInput;
            _playerInput.Character.SwitchWeapon.performed -= OnSwitchWeaponInputReceived;
            _playerInput.Character.Drop.performed -= OnDropPerformed;
            _playerInput.Character.SwitchWeaponHotkey.performed -= OnSwitchWeaponHotkeyInput;
            _playerInput.Character.Reload.performed -= OnReloadInputReceived;
            _playerInput.Character.Pick.performed -= OnPickPerformed;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable the PlayerInput component
            _playerInput.Character.Disable();
        }

        private void OnDestroy()
        {
            _playerInput.Dispose();
        }

        // Input Event Handlers

        #region Loot Pickup Input

        private void OnPickPerformed(InputAction.CallbackContext context)
        {
            OnLootPickupInput?.Invoke();
        }

        #endregion

        #region Equip Ability Input

        /// <summary>
        /// This method is called when the player equips an ability.
        /// </summary>
        /// <param name="context">struct that holds the action context</param>
        private void OnEquipAbilityInput(InputAction.CallbackContext context)
        {
            // Invoke the OnEquipAbilityInput event.
            OnAbilityEquipped?.Invoke();
        }

        #endregion

        #region Movement Input

        /// <summary>
        /// This method is called when the player is moving the character.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMoveInputReceived(InputAction.CallbackContext context)
        {
            // Invoke the OnMoveInputUpdated event with the input value.
            OnMoveInputUpdated?.Invoke(context.ReadValue<Vector2>());
        }

        #endregion

        #region Attack Input

        /// <summary>
        /// This method is called when the player starts attacking.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnUseStarted(InputAction.CallbackContext context)
        {
            // Invoke the OnAttackInputBegan event.
            OnAttackInputBegan?.Invoke();
        }

        /// <summary>
        /// This method is called when the player stops attacking.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnUseCancelled(InputAction.CallbackContext context)
        {
            // Invoke the OnAttackInputEnded event.
            OnAttackInputEnded?.Invoke();
        }

        #endregion

        #region Look Input

        /// <summary>
        /// This method is called when the player is looking around.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnLookInputReceived(InputAction.CallbackContext context)
        {
            // Invoke the OnLookInputUpdated event with the input value.
            OnLookInputUpdated?.Invoke(context.ReadValue<Vector2>());
        }

        #endregion

        #region Switch Weapon Input

        /// <summary>
        /// This method is called when the player switches weapons.
        /// </summary>
        /// <param name="context">input callback context</param>
        private void OnSwitchWeaponInputReceived(InputAction.CallbackContext context)
        {
            // Prevents firing too often
            if (Time.time - _lastScrollTime < scrollCooldown) return;

            float scrollY = context.ReadValue<float>();
            if (Mathf.Abs(scrollY) > 0.01f)
            {
                int direction = scrollY > 0 ? -1 : 1;
                OnSwitchWeaponInput?.Invoke(direction);
                _lastScrollTime = Time.time;
            }
        }

        private void OnSwitchWeaponHotkeyInput(InputAction.CallbackContext context)
        {
            string key = context.control.displayName;

            int slotIndex = key switch
            {
                "1" => 0,
                "2" => 1,
                "3" => 2,
                _ => -1
            };

            if (slotIndex >= 0)
                OnSwitchWeaponHotkey?.Invoke(slotIndex);
        }

        #endregion

        #region Reload Input

        /// <summary>
        /// This method is called when the player reloads the weapon.
        /// </summary>
        /// <param name="context">input callback context</param>
        private void OnReloadInputReceived(InputAction.CallbackContext context)
        {
            // Invoke the OnReloadInput event.
            OnReloadInput?.Invoke();
        }

        #endregion

        #region Drop Input

        private void OnDropPerformed(InputAction.CallbackContext context)
        {
            // Invoke the OnDropInput event.
            OnDropInput?.Invoke();
        }

        #endregion
    }
}