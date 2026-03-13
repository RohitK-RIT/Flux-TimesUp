using UnityEngine;

namespace _Project.Scripts.Core.Enemy.GroupEnemyBehavior
{
    public class GroupManager : MonoBehaviour
    {
        public static GroupManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        // Method to get helper flanking position
        public Vector3 GetHelperPosition(Vector3 enemyPosition, Vector3 playerPosition)
        {
            // Flanking logic
            Vector3 directionToPlayer = (playerPosition - enemyPosition).normalized;
            Vector3 flankingDirection = Vector3.Cross(directionToPlayer, Vector3.up); // Perpendicular direction for flanking
            float flankDistance = 5f;

            return playerPosition + (flankingDirection * flankDistance);
        }
    }
}
