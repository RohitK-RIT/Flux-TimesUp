using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject chargerPrefab;
        private readonly float _spawnDistanceFromEnemy = 4f;  // Distance from enemy
        private readonly float _spawnOffsetFromPlayer = 3f;   // Distance in front of the player
    
        internal void ChargerSpawner(Vector3 enemyPosition)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Debug.Log("Player not found while Spawning!");
                return;
            }
        
            Vector3 playerForward = player.transform.forward.normalized; // Direction the player is facing
            Vector3 spawnCenter = player.transform.position + (playerForward * _spawnOffsetFromPlayer); // In front of player
        
            // Calculate spawn positions around the enemy
            Vector3 spawnPos1 = enemyPosition + (Vector3.right * _spawnDistanceFromEnemy);
            Vector3 spawnPos2 = enemyPosition + (Vector3.left * _spawnDistanceFromEnemy);
        
            // Instantiate Chargers
            Instantiate(chargerPrefab, spawnPos1, Quaternion.identity);
            Instantiate(chargerPrefab, spawnPos2, Quaternion.identity);
        }
    }
}
