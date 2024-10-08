using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class EnemyAttack : CharacterMovement
    {
        public Transform player;  // Reference to the player's Transform
        public float detectionRange = 5f;  // The distance at which the enemy detects the player
      
        void Update()
        {
            // Call the method to rotate towards the player
            RotateTowardsPlayer();

            // Check if the player is within the detection range
            CheckProximity();
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

        void CheckProximity()
        {
            // Calculate distance between enemy and player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Log a message if the player is within detection range
            if (distanceToPlayer <= detectionRange)
            {
                Debug.Log("Player is close to the enemy!");
            }
        }
    }
}
