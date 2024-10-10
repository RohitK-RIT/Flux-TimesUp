using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class EnemyAttack : CharacterMovement
    {
        public Transform player;  // Reference to the player's Transform
        public float detectionRange = 5f;  // The distance at which the enemy detects the player
        public float fieldOfViewAngle = 45f; // The conical angle at which the enemy detects the player
      
        void Update()
        {
            // Rotate the player only if its in the Conical Field of View
            if(isPlayerInCone())
            {
                 // Call the method to rotate towards the player
                 RotateTowardsPlayer();

                // Check if the player is within the detection range
                Debug.Log("Player in range");
            }
           
        }

        void RotateTowardsPlayer()
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;  // Keep rotation on the horizontal plane

            // Convert the 3D direction to a 2D vector for HandleLook()
            Vector2 playerDirection = new Vector2(directionToPlayer.x, directionToPlayer.z);

            // Using the inherited HandleLook method to rotate towards the player
            HandleLook(playerDirection);
        }

        private bool isPlayerInCone()
        {
            // Calculating the direction from the enemy to player
            Vector3 directionToPlayer = player.position - transform.position;

            // Calculating the distance between the enemy to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Checking if the player is within the detection range
            if (distanceToPlayer > detectionRange)
            {
                return false;
            }

            // Normalizing the direction vector and getting the forward direction of the enemy
            directionToPlayer.Normalize();
            Vector3 forward = transform.forward;

            // The angle between the enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(forward, directionToPlayer);

            // Checking if the player is in the field of view angle
            if (angleToPlayer <= fieldOfViewAngle)
            {
                return true;
            }
            return false;

        }
    }
}
