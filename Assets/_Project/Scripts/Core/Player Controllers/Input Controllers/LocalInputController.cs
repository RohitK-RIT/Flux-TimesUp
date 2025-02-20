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

        public override event Action OnReloadInput;

        public override event Action OnLootPickupInput;

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
            // Move Input Events
            _playerInput.PlayerControl.Move.started += OnMoveInputReceived;
            _playerInput.PlayerControl.Move.performed += OnMoveInputReceived;
            _playerInput.PlayerControl.Move.canceled += OnMoveInputReceived;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started += OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled += OnAttackInputCancelled;

            // Look Input Events
            _playerInput.PlayerControl.Look.performed += OnLookInputReceived;
            _playerInput.PlayerControl.Look.canceled += OnLookInputReceived;

            // Equip Ability Input Events
            _playerInput.PlayerControl.EquipAbility.performed += OnEquipAbilityInput;

            _playerInput.PlayerControl.SwitchWeapon.performed += OnSwitchWeaponInputReceived;
            
            _playerInput.PlayerControl.Reload.performed += OnReloadInputReceived;
            
            _playerInput.PlayerControl.LootPickUp.performed += OnLootPickupInputReceived;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Enable the PlayerInput component
            _playerInput.PlayerControl.Enable();
        }

        public void OnDisable()
        {
            // Unsubscribe from input events
            // Move Input Events
            _playerInput.PlayerControl.Move.started -= OnMoveInputReceived;
            _playerInput.PlayerControl.Move.performed -= OnMoveInputReceived;
            _playerInput.PlayerControl.Move.canceled -= OnMoveInputReceived;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started -= OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled -= OnAttackInputCancelled;

            // Look Input Events
            _playerInput.PlayerControl.Look.performed -= OnLookInputReceived;
            _playerInput.PlayerControl.Look.canceled -= OnLookInputReceived;

            // Equip Ability Input Events
            _playerInput.PlayerControl.EquipAbility.performed -= OnEquipAbilityInput;

            _playerInput.PlayerControl.SwitchWeapon.performed -= OnSwitchWeaponInputReceived;
            
            _playerInput.PlayerControl.Reload.performed -= OnReloadInputReceived;
            
            _playerInput.PlayerControl.LootPickUp.performed -= OnLootPickupInputReceived;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable the PlayerInput component
            _playerInput.PlayerControl.Disable();
        }

        private void OnDestroy()
        {
            _playerInput.Dispose();
        }

        // Input Event Handlers
        
        #region Loot Pickup Input
        private void OnLootPickupInputReceived(InputAction.CallbackContext context)
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
        private void OnAttackInputStarted(InputAction.CallbackContext context)
        {
            // Invoke the OnAttackInputBegan event.
            OnAttackInputBegan?.Invoke();
        }

        /// <summary>
        /// This method is called when the player stops attacking.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnAttackInputCancelled(InputAction.CallbackContext context)
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
            // Invoke the OnSwitchWeaponInput event.
            OnSwitchWeaponInput?.Invoke((int)context.ReadValue<float>());
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
    }
}