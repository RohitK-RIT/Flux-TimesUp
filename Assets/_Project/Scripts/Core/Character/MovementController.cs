using System;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// This class is responsible for moving the character based on the movement input.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : CharacterComponent
    {
        /// <summary>
        /// Aim transform of a character.
        /// </summary>
        public Transform AimTransform => aimTransform;

        /// <summary>
        /// Body of the player.
        /// </summary>
        public Transform Body => body;

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        [NonSerialized] public Vector2 MoveInput;

        /// <summary>
        /// Aim transform of a character.
        /// </summary>
        [SerializeField] private Transform aimTransform;

        /// <summary>
        /// Body of the player.
        /// </summary>
        [SerializeField] private Transform body;

        /// <summary>
        /// Weapon parent of the character.
        /// </summary>
        [SerializeField] private Transform weaponParent;

       
        /// <summary>
        /// The CharacterController component attached to the character.
        /// </summary>
        private CharacterController _characterController;

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        private Vector3 _moveDirection;
        
        /// <summary>
        /// Current Movement of the player
        /// </summary>
        private Vector3 _currentMovement;
        
        /// <summary>
        /// Vertical velocity for player's falling speed.
        /// </summary>
        private float _velocity;
        
        /// <summary>
        /// Setting gravity value
        /// </summary>
        private readonly float _gravity = -9.81f;
        
        /// <summary>
        /// Multiplier to adjust the strength of gravity
        /// </summary>
        private readonly float _gravityMultiplier = 3f;
        
        private Camera _camera;

        private void Awake()
        {
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            // Handle movement and look.
            HandleMovement();
            HandleLook();
        }

        /// <summary>
        /// This method is called to move the character based on the movement input.
        /// </summary>
        private void HandleMovement()
        {
            // If movement input is zero then return.
            if (MoveInput == Vector2.zero) return;

            // Assign horizontal and vertical inputs to the movement vector
            _moveDirection = (_camera.transform.right * MoveInput.x + _camera.transform.forward * MoveInput.y) * PlayerController.Stats.movementSpeed;

            // Applying gravity for the y value
            HandleGravity();
            
            // Can add jump here if needed by modifying the y component of the movement vector. 

            // Move the character via the character controller.
            _characterController.Move(_currentMovement * (PlayerController.Stats.movementSpeed * Time.deltaTime));
        }

        /// <summary>
        /// This method is called to rotate the character based on aim transform's position.
        /// </summary>
        private void HandleLook()
        {
            // Horizontal rotation
            var horizontalTargetLocation = new Vector3(aimTransform.position.x, body.position.y, aimTransform.position.z);
            var horizontalDirection = (horizontalTargetLocation - transform.position).normalized;
            body.forward = Vector3.Lerp(body.forward, horizontalDirection, Time.deltaTime * 20f);

            // Vertical rotation
            var direction = aimTransform.position - weaponParent.position;
            var horizontalDistance = new Vector3(direction.x, 0f, direction.z).magnitude;
            var verticalDistance = direction.y;
            var pitchAngle = Mathf.Atan2(verticalDistance, horizontalDistance) * Mathf.Rad2Deg;

            var weaponTargetRotation = Quaternion.Euler(-pitchAngle, 0f, 0f);
            weaponParent.localRotation = Quaternion.Slerp(weaponParent.localRotation, weaponTargetRotation, Time.deltaTime * 20f);
        }

        /// <summary>
        /// This method is called to apply the gravity to the player's y direction.
        /// </summary>
        private void HandleGravity()
        {
            // Check if the player is on ground
            if (_characterController.isGrounded && _velocity < 0.0f)
            {
                    _velocity = -1.0f;  // Small negative value to keep the character grounded
            }
            else
            {
                // Apply gravity when not grounded
                _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
            }
            
            // Combine horizontal and vertical movement
            _currentMovement = new Vector3(_moveDirection.x, _velocity, _moveDirection.z);

        }
    }
}