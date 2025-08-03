using _Project.Scripts.Core.Enemy.FSM;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.EnemySpawner
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject chargerEnemyPrefab; // Reference to charger enemy prefab
        [SerializeField] private GameObject basicEnemyPrefab; // Reference to basic enemy prefab
        [SerializeField] private AudioClip spawnSFX;
        [SerializeField] private GameObject spawnEffectPrefab;

        private readonly float _spawnDistanceFromEnemy = 4f; // Distance from an enemy
        private RoomWaveController _roomWaveController;

        private void Awake()
        {
            // Get reference to the RoomWaveController in the parent object
            _roomWaveController = GetComponentInParent<RoomWaveController>();
        }

        // Spawns two Charger enemies near the given enemy position.
        // One Charger appears to the right, and the other appears to the left.
        internal void ChargerSpawner(Vector3 enemyPosition, EnemyType type)
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
            Vector3 spawnPos3 = enemyPosition + (Vector3.forward * _spawnDistanceFromEnemy);

            // Instantiate Chargers
            if (type == EnemyType.Basic)
            {
                AudioSource.PlayClipAtPoint(spawnSFX, Camera.main.transform.position);

                Instantiate(spawnEffectPrefab, spawnPos1, Quaternion.identity);
                Instantiate(chargerEnemyPrefab, spawnPos1, Quaternion.identity, transform.parent);
                Instantiate(spawnEffectPrefab, spawnPos2, Quaternion.identity);
                Instantiate(chargerEnemyPrefab, spawnPos2, Quaternion.identity, transform.parent);
                
            }

            if (type == EnemyType.Boss)
            {
                AudioSource.PlayClipAtPoint(spawnSFX, Camera.main.transform.position);

                Instantiate(spawnEffectPrefab, spawnPos1, Quaternion.identity);
                Instantiate(chargerEnemyPrefab, spawnPos1, Quaternion.identity, transform.parent);
                Instantiate(spawnEffectPrefab, spawnPos2, Quaternion.identity);
                Instantiate(chargerEnemyPrefab, spawnPos2, Quaternion.identity, transform.parent);
                Instantiate(spawnEffectPrefab, spawnPos3, Quaternion.identity);
                Instantiate(chargerEnemyPrefab, spawnPos3, Quaternion.identity, transform.parent);
            }
        }

        // Respawns all enemies in the current room at their original positions.
        // Resets their state and reactivates them.
        internal void WaveEnemySpawner()
        {
            // Clear old enemy references
            _roomWaveController.EnemiesInRoom.Clear();

            for (int i = 0; i < _roomWaveController.OriginalSpawnPoints.Count; i++)
            {
                Vector3 spawnPosition = _roomWaveController.OriginalSpawnPoints[i];

                // Instantiate a brand new enemy
                GameObject newEnemy = Instantiate(basicEnemyPrefab, spawnPosition, Quaternion.identity, transform.parent);
                
                newEnemy.SetActive(true);

                // Reset any state or values
                var enemyController = newEnemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.Reset();
                }

                // Add to current room’s list
                _roomWaveController.EnemiesInRoom.Add(newEnemy);
            }

            Debug.Log($"Spawned {_roomWaveController.EnemiesInRoom.Count} fresh enemies in room {gameObject.name}.");
        }
    }
}