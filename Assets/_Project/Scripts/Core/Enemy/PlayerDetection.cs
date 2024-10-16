using System.Collections.Generic;
using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class PlayerDetection : CharacterMovement
    {
        public float detectionRange = 5f;  // The distance at which the enemy detects the player
        public float fieldOfViewAngle = 45f; // The conical angle at which the enemy detects the player
        
        public LayerMask layerMask; // A LayerMask to specify which layers the detection should interact with
        
        private readonly List<Transform> _playersInRange = new List<Transform>();  // List of players currently in range

      
        void Update()
        {
            // Find all players in range
            FindPlayersInRange();

            // Debugging: Print the list of players in range --> Just for testing purpose
            DebugPlayersInRange();

            // If there are players in range, process the closest one
            if (_playersInRange.Count > 0)
            {
                // Find the closest player
                Transform closestPlayer = FindClosestPlayerInRange();

                // Check if the closest player is in the conical view
                if (closestPlayer && IsPlayerInCone(closestPlayer))
                {
                    // Rotate the enemy towards the closest player
                    RotateTowardsPlayer(closestPlayer);
                    Debug.Log("Closest player in range and view."+closestPlayer.name);
                }
            }
        }
       
        // Find all players currently within the detection range.
        void FindPlayersInRange()
        {
            // Get all colliders within the detection range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

            // Create a temporary list to track the players in this frame
            List<Transform> currentPlayers = new List<Transform>();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    currentPlayers.Add(hitCollider.transform);

                    // If player is not already in the list, add them
                    if (!_playersInRange.Contains(hitCollider.transform))
                    {
                        _playersInRange.Add(hitCollider.transform);
                        Debug.Log("Player entered range: " + hitCollider.transform.name);
                    }
                }
            }

            // Remove players who have left the detection range
            for (int i = _playersInRange.Count - 1; i >= 0; i--)
            {
                if (!currentPlayers.Contains(_playersInRange[i]))
                {
                    Debug.Log("Player left range: " + _playersInRange[i].name);
                    _playersInRange.RemoveAt(i);  // Remove the player reference
                }
            }
        }
        
        // Print the names of the players currently in range for debugging.
        void DebugPlayersInRange()
        {
            if (_playersInRange.Count == 0)
            {
                Debug.Log("No players in range.");
            }
            else
            {
                Debug.Log("Players in range:"+_playersInRange.Count);
                foreach (Transform player in _playersInRange)
                {
                    Debug.Log(player.name);
                }
            }
        }
        
        // Find the closest player within the playersInRange list.
        Transform FindClosestPlayerInRange()
        {
            // Variable to store the closest player's Transform
            Transform closest = null;
            
            // Initialize the closest distance as the detection range
            float closestDistance = detectionRange;

            foreach (Transform player in _playersInRange)
            {
                // Calculate the distance between the enemy and the current player
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                
                // Check if this player is closer than the previously found closest player
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer; // Update the closest distance
                    closest = player; // Update the closest player reference
                }
            }
            
            // Return the Transform of the closest player, or null if no player was found
            return closest;
        }
        
        bool IsPlayerInCone(Transform player)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;  // Only rotate on the horizontal plane

            // Angle between enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

            // Check if the player is within the field of view angle
            return angleToPlayer <= fieldOfViewAngle;
        }
        
        void RotateTowardsPlayer(Transform player)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            directionToPlayer.y = 0;  // Keep rotation on the horizontal plane
            
            // Use the inherited HandleLook method to rotate the enemy
            HandleLook(directionToPlayer);
        }
      
    }
}
