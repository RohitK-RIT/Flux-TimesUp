using _Project.Scripts.Core.Enemy.EnemySpawner;
using _Project.Scripts.Core.Enemy.FSM;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.Types
{
    public class ChargerType : MonoBehaviour
    {
        private readonly float _lowHealthThreshold = 35f; // Trigger when health drops below this
        private EnemyInputController _enemyInputController;
        private SpawnManager _spawnManager;
        private bool _hasSpawnedChargers; // Ensure Chargers spawn only once
        private bool _hasSpawnedSecondChargers; // Ensure Chargers spawn only once
        private bool _hasSpawnedThirdChargers; // Ensure Chargers spawn only once

        void Awake()
        {
            _enemyInputController = GetComponent<EnemyInputController>();
            _spawnManager = GetComponent<SpawnManager>();
        }

        private void Update()
        {
            if (_enemyInputController.enemyType == EnemyType.Basic && !_hasSpawnedChargers &&
                _enemyInputController.EnemyHUD.enemy.CurrentHealth <= _lowHealthThreshold)
            {
                _hasSpawnedChargers = true;
                SpawnChargers(EnemyType.Basic);
            }

            // Check if the enemy is a Boss type and should spawn Chargers at 75 and 50 health
            if (_enemyInputController.enemyType == EnemyType.Boss)
            {
                if (!_hasSpawnedChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= 75)
                {
                    _hasSpawnedChargers = true;
                    SpawnChargers(EnemyType.Boss);
                }
                if (!_hasSpawnedSecondChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= 50)
                {
                    _hasSpawnedSecondChargers = true;
                    SpawnChargers(EnemyType.Boss);
                }
                if (!_hasSpawnedThirdChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= 25)
                {
                    _hasSpawnedThirdChargers = true;
                    SpawnChargers(EnemyType.Boss);
                }
            }
        }
    
        private void SpawnChargers(EnemyType type)
        {
            if (_spawnManager)
            {
                // Request the SpawnManager to spawn Chargers near the enemy's position
                _spawnManager.ChargerSpawner(_enemyInputController.Enemy.transform.position, type);
            }
            else
            {
                Debug.LogWarning("SpawnManager not found in the scene!");
            }
        }
    }
}
