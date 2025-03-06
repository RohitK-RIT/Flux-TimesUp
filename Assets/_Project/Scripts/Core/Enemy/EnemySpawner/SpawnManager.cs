using UnityEngine;

namespace _Project.Scripts.Core.Enemy.EnemySpawner
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject chargerEnemyPrefab; // Reference to charger enemy prefab
        [SerializeField] private GameObject basicEnemyPrefab; // Reference to basic enemy prefab
        private readonly float _spawnDistanceFromEnemy = 4f;  // Distance from an enemy
        private RoomWaveController _roomWaveController;

        private void Awake()
        {
            // Get reference to the RoomWaveController in the parent object
            _roomWaveController = GetComponentInParent<RoomWaveController>();
        }
    
        // Spawns two Charger enemies near the given enemy position.
        // One Charger appears to the right, and the other appears to the left.
        internal void ChargerSpawner(Vector3 enemyPosition, Transform enemyParent)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                Debug.Log("Player not found while Spawning!");
                return;
            }
            
            // Calculate spawn positions around the enemy
            Vector3 spawnPos1 = enemyPosition + (Vector3.right * _spawnDistanceFromEnemy);
            Vector3 spawnPos2 = enemyPosition + (Vector3.left * _spawnDistanceFromEnemy);
        
            // Instantiate Chargers
            Instantiate(chargerEnemyPrefab, spawnPos1, Quaternion.identity, enemyParent);
            Instantiate(chargerEnemyPrefab, spawnPos2, Quaternion.identity, enemyParent);
        }
        
        // Respawns all enemies in the current room at their original positions.
        // Resets their state and reactivates them.
        internal void WaveEnemySpawner()
        {
            for (int i = 0; i < _roomWaveController.EnemiesInRoom.Count; i++)
            {
                if (_roomWaveController.EnemiesInRoom[i] != null)
                {
                    // Get enemy reference
                    var enemy = _roomWaveController.EnemiesInRoom[i];
            
                    // Reset enemy position to its original spawn point
                    enemy.transform.position = _roomWaveController.OriginalSpawnPoints[i]; 
                    
                    // Reactivate the enemy GameObject
                    enemy.gameObject.SetActive(true);
                    
                    // Call Reset() on the enemy controller
                    enemy.gameObject.GetComponent<EnemyController>().Reset();
                }
            }
            // Reset the enemy list in RoomWaveController to reflect the changes
            _roomWaveController.ResetEnemiesInRoom();
            
            Debug.Log($"Reactivated {_roomWaveController.EnemiesInRoom.Count} enemies in room {gameObject.name}.");
        }
    }
    
    
}
