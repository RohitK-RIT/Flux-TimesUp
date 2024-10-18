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
        internal void HandleLook(Vector3 direction)
        {
            //Normalizing the direction value
            direction = direction.normalized;

            // Ignore vertical rotation by setting y to 0 during rotation calculation
            var positionToLookAt = new Vector3(direction.x, 0.0f, direction.z);
            var currentRotation = transform.rotation;
            
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

            // Smoothly interpolate the character's rotation towards the target rotation
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, RotationFactorPerFrame * Time.deltaTime);
            
        }

        public void PlayerLookAtMouse(Vector2 mousePosition)
        {
            if (Camera.main)
            {
                // Convert mouse position to a ray from the camera
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        
                // Use a fixed distance for the target point
                float distanceToGround = 10f; // You can adjust this value as needed
                Vector3 targetPoint = ray.GetPoint(distanceToGround);

                // Calculate the direction to look at
                Vector3 directionToLook = targetPoint - transform.position;
                directionToLook.y = 0; // Ignore the vertical component

                // Pass the direction to HandleLook
                HandleLook(directionToLook);
            }
        }
    }
}