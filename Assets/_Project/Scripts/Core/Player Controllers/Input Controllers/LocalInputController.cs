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
        /// Event that is called when the player starts attacking.
        /// </summary>
        public override event Action OnAttackInputBegan;

        /// <summary>
        /// Event that is called when the player stops attacking.
        /// </summary>
        public override event Action OnAttackInputEnded;

        /// <summary>
        /// Component that handles player input Unity API calls.
        /// </summary>
        private readonly PlayerInput _playerInput = new();

        /// <summary>
        /// Enables the input controller.
        /// </summary>
        public void Enable()
        {
            // Enable the PlayerInput component
            _playerInput.PlayerControl.Enable();

            // Subscribe to input events
            // Move Input Events
            _playerInput.PlayerControl.Move.started += OnMovementInputUpdated;
            _playerInput.PlayerControl.Move.performed += OnMovementInputUpdated;
            _playerInput.PlayerControl.Move.canceled += OnMovementInputUpdated;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started += OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled += OnAttackInputCancelled;
        }

        /// <summary>
        /// Disables the input controller.
        /// </summary>
        public void Disable()
        {
            // Unsubscribe from input events
            // Move Input Events
            _playerInput.PlayerControl.Move.started -= OnMovementInputUpdated;
            _playerInput.PlayerControl.Move.performed -= OnMovementInputUpdated;
            _playerInput.PlayerControl.Move.canceled -= OnMovementInputUpdated;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started -= OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled -= OnAttackInputCancelled;

            // Disable the PlayerInput component
            _playerInput.PlayerControl.Disable();
        }

        /// <summary>
        /// Destructor that disables the input controller.
        /// </summary>
        ~LocalInputController()
        {
            Disable();

            _playerInput.Dispose();
        }

        // Input Event Handlers

        #region Movement Input

        /// <summary>
        /// This method is called when the player is moving the character.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMovementInputUpdated(InputAction.CallbackContext context)
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
    }
}