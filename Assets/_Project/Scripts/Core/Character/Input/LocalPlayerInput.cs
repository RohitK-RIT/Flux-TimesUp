using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.Character.Input
{
    /// <summary>
    /// This class is responsible for handling the player's input and moving the character based on the input.
    /// </summary>
    [RequireComponent(typeof(CharacterMovement))]
    public class LocalPlayerInput : MonoBehaviour
    {
        /// <summary>
        /// The CharacterMovement component attached to the player.
        /// </summary>
        private CharacterMovement _movement;

        /// <summary>
        /// The PlayerInput component that gives the input calls.
        /// </summary>
        private PlayerInput _playerInput;

        private void Awake()
        {
            // Initialize PlayerInput from Unity's New Input System
            _playerInput = new PlayerInput();
            // Get the CharacterMovement component attached to the player
            _movement = GetComponent<CharacterMovement>();
        }

        private void OnEnable()
        {
            _playerInput.PlayerControl.Enable();

            // Subscribe to input events
            _playerInput.PlayerControl.Move.started += OnMovementInputStarted;
            _playerInput.PlayerControl.Move.performed += OnMovementInputPerformed;
            _playerInput.PlayerControl.Move.canceled += OnMovementInputCancelled;
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _playerInput.PlayerControl.Move.started -= OnMovementInputStarted;
            _playerInput.PlayerControl.Move.performed -= OnMovementInputPerformed;
            _playerInput.PlayerControl.Move.canceled -= OnMovementInputCancelled;

            _playerInput.PlayerControl.Disable();
        }

        // Input Event Handlers
        /// <summary>
        /// This method is called when the player starts moving the character. It reads the input value and sets the MoveInput property to the input value. It also sets the IsMoving property to true.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMovementInputStarted(InputAction.CallbackContext context)
        {
            // Read the input value and set the moveDirection variable in the CharacterMovement script.
            _movement.moveDirection = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// This method is called when the player is moving the character. It reads the input value and sets the MoveInput property to the input value.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMovementInputPerformed(InputAction.CallbackContext context)
        {
            // Read the input value and set the moveDirection variable in the CharacterMovement script.
            _movement.moveDirection = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// This method is called when the player stops moving the character. It sets the MoveInput property to zero and sets the IsMoving property to false.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMovementInputCancelled(InputAction.CallbackContext context)
        {
            // Set the moveDirection variable in the CharacterMovement script to zero.
            _movement.moveDirection = Vector2.zero;
        }
    }
}