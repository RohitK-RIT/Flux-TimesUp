using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// This class is responsible for moving the character based on the movement input.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterMovement : MonoBehaviour
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
            PlayerLookAtMouse();
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
        public void HandleLook(Vector3 direction)
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
        
        public void PlayerLookAtMouse()
        {
            // Ground plane at y = 0
            var groundPlane = new Plane(Vector3.up, Vector3.zero); 

            if (Camera.main)
            {
                // Ray from the camera to the mouse cursor position
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

                // Variable to store the distance from the camera to the intersection point on the plane
                if (groundPlane.Raycast(ray, out var rayDistance))
                {
                    // Get the point where the ray intersects the ground plane (mouse world position)
                    Vector3 hitPoint = ray.GetPoint(rayDistance);

                    // Calculate the direction to look at from the player's position
                    var directionToLook = hitPoint - transform.position;

                    // Instead of immediately zeroing the y component, pass the full direction to HandleLook
                    HandleLook(new Vector3(directionToLook.x, directionToLook.y, directionToLook.z));
                }
            }
        }
    }
}