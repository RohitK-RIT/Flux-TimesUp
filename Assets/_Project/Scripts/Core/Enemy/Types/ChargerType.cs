using _Project.Scripts.Core.Enemy.EnemySpawner;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.Types
{
    public class ChargerType : MonoBehaviour
    {
        private readonly float _lowHealthThreshold = 35f; // Trigger when health drops below this
        private EnemyInputController _enemyInputController;
        private SpawnManager _spawnManager;
        private bool _hasSpawnedChargers; // Ensure Chargers spawn only once

        void Awake()
        {
            _enemyInputController = GetComponent<EnemyInputController>();
            _spawnManager = GetComponent<SpawnManager>();
        }

        private void Update()
        {
            // Check if Chargers haven't been spawned yet and enemy health is below the threshold
            if (!_hasSpawnedChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= _lowHealthThreshold)
            {
                // Mark that Chargers have been spawned to prevent multiple spawns
                _hasSpawnedChargers = true;
                SpawnChargers();
            }
        }
    
        private void SpawnChargers()
        {
            if (_spawnManager)
            {
                // Request the SpawnManager to spawn Chargers near the enemy's position
                _spawnManager.ChargerSpawner(_enemyInputController.Enemy.transform.position);
            }
            else
            {
                Debug.LogWarning("SpawnManager not found in the scene!");
            }
        }
    }
}
