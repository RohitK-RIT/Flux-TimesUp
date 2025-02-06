using System.Collections.Generic;
using _Project.Scripts.Core.Enemy.EnemySpawner;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject chargerEnemyPrefab;
        [SerializeField] private GameObject basicEnemyPrefab;
        private readonly float _spawnDistanceFromEnemy = 4f;  // Distance from enemy
        private readonly float _spawnOffsetFromPlayer = 3f;   // Distance in front of the player
        [SerializeField] private Transform checking;
        private RoomWaveController _roomWaveController;

        private void Awake()
        {
            _roomWaveController = GetComponentInParent<RoomWaveController>();
        }
    
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
            // Instantiate(chargerEnemyPrefab, spawnPos1, Quaternion.identity);
            // Instantiate(chargerEnemyPrefab, spawnPos2, Quaternion.identity);
        }
        
        internal void WaveEnemySpawner()
        {
            for (int i = 0; i < _roomWaveController.enemiesInRoom.Count; i++)
            {
                if (_roomWaveController.enemiesInRoom[i] != null)
                {
                    // _roomWaveController.enemiesInRoom[i].transform.position = _roomWaveController.originalSpawnPoints[i]; // Reset position
                    // _roomWaveController.enemiesInRoom[i].gameObject.SetActive(true); // Reactivate enemy
                    // _roomWaveController.Reset();
                    
                    var enemy = _roomWaveController.enemiesInRoom[i]; // Store reference
            
                    enemy.transform.position = _roomWaveController.originalSpawnPoints[i]; // Reset position
                    enemy.gameObject.SetActive(true); // Reactivate enemy
            
                    enemy.gameObject.GetComponent<EnemyController>().Reset(); // Ensure Reset() is being called for each enemy
                }
            }
            
            Debug.Log($"Reactivated {_roomWaveController.enemiesInRoom.Count} enemies in room {gameObject.name}.");
        }
    }
    
    
}
