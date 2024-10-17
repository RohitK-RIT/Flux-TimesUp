using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// This class is responsible for moving the character based on the movement input.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        [HideInInspector] public Vector2 moveDirection;

        /// <summary>
        /// The speed at which the player moves.
        /// </summary>
        [SerializeField] private float moveSpeed = 5f;

        /// <summary>
        /// The CharacterController component attached to the character.
        /// </summary>
        private CharacterController _characterController;

        /// <summary>
        /// The constant rotation factor per frame.
        /// </summary>
        private const float RotationFactorPerFrame = 10.0f;

        private void Awake()
        {
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
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
            if (moveDirection == Vector2.zero) return;

            // Assign horizontal and vertical inputs to the movement vector
            var currentMovement = new Vector3(moveDirection.x, 0.0f, moveDirection.y);

            // Move the character via the character controller.
            _characterController.Move(currentMovement * (moveSpeed * Time.deltaTime));
        }

        /// <summary>
        /// This method is called to rotate the character based on the movement input.
        /// </summary>
        private void HandleLook()
        {
            var positionToLookAt = new Vector3(moveDirection.x, 0.0f, moveDirection.y);
            var currentRotation = transform.rotation;

            // If movement input is pressed, rotate the character to face the movement direction
            if (positionToLookAt == Vector3.zero) return;

            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            // Smoothly interpolate the character's rotation towards the target rotation
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, RotationFactorPerFrame * Time.deltaTime);
        }
    }
}