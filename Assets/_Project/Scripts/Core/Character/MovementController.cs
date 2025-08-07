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
            if (MoveInput == Vector2.zero && _characterController.isGrounded) return;

            // Project camera forward onto the horizontal plane
            var cameraForward = _camera.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            var cameraRight = _camera.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            _moveDirection = cameraRight * MoveInput.x + cameraForward * MoveInput.y;

            HandleGravity();

            _characterController.Move(_moveDirection * (PlayerController.Stats.movementSpeed * Time.deltaTime));
        }

        /// <summary>
        /// This method is called to rotate the character based on aim transform's position.
        /// </summary>
        private void HandleLook()
        {
            // Horizontal rotation
            var aimPosition = aimTransform.position;
            var horizontalTargetLocation = aimPosition;
            horizontalTargetLocation.y = body.position.y;
            var horizontalDirection = (horizontalTargetLocation - transform.position).normalized;
            body.forward = Vector3.Lerp(body.forward, horizontalDirection, Time.deltaTime * 20f);
            // Resetting the aim position
            aimTransform.position = aimPosition;
        }

        /// <summary>
        /// This method is called to apply the gravity to the player's y direction.
        /// </summary>
        private void HandleGravity()
        {
            // Check if the player is on ground
            if (_characterController.isGrounded && _velocity < 0.0f)
                _velocity = -1.0f; // Small negative value to keep the character grounded
            else
                // Apply gravity when not grounded
                _velocity += _gravity * _gravityMultiplier * Time.deltaTime;

            // Combine horizontal and vertical movement
            _moveDirection.y = _velocity;
        }

        /// <summary>
        /// Sets the world position of the character.
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position)
        {
            // Disable the character controller to avoid issues while setting the position
            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }
    }
}