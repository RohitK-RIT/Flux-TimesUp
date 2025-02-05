using UnityEngine;

namespace _Project.Scripts.Core.Enemy.Types
{
    public class ChargerType : MonoBehaviour
    {
        private readonly float _lowHealthThreshold = 40f; // Trigger when health drops below this
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
            if (!_hasSpawnedChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= _lowHealthThreshold)
            {
                _hasSpawnedChargers = true;
                SpawnChargers();
            }
        }
    
        private void SpawnChargers()
        {
            if (_spawnManager != null)
            {
                _spawnManager.ChargerSpawner(_enemyInputController.Enemy.transform.position);
            }
            else
            {
                Debug.LogWarning("SpawnManager not found in the scene!");
            }
        }
    }
}
