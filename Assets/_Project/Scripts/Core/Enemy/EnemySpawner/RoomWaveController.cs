using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Gameplay.PCG;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.EnemySpawner
{
    public class RoomWaveController : MonoBehaviour
    {
        internal List<Vector3> originalSpawnPoints = new List<Vector3>(); // Stores enemy spawn positions
        internal List<GameObject> enemiesInRoom = new List<GameObject>(); // Stores references to original enemies
    
        private HashSet<int> _spawnedWaves = new HashSet<int>(); // Stores triggered waves
        private EnemyInputController _enemyInputController;
        private SpawnManager _spawnManager;
        private EnemyController _enemyController;
        private EnemyDeathListener _enemyDeathListener;

        private void Awake()
        {
            _enemyInputController = GetComponentInChildren<EnemyInputController>();
            _spawnManager = GetComponentInChildren<SpawnManager>();
            _enemyController = GetComponentInChildren<EnemyController>();
            StoreEnemyPositions(); // Store spawn points at the start
            
            if(_enemyInputController==null) return;
            if (_enemyInputController.enemyType == EnemyType.Basic)
            {
                _enemyDeathListener = new EnemyDeathListener(gameObject);
                Debug.Log("Initialized enemy death listener");
            }
        }
        
        private void OnEnable()
        {
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead += OnAllEnemiesDead;
        }
        
        private void OnDisable()
        {
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead -= OnAllEnemiesDead;
        }
        
        private void OnAllEnemiesDead()
        {
            Debug.Log("All enemies dead");
            CanSpawnEnemies();
        }

        private void Update()
        {
            int stabilityThreshold = Mathf.FloorToInt(TimeStabilityMeter.Instance.TimeStability / 10) * 10; // Round to nearest 10

            if (stabilityThreshold <= 50 && stabilityThreshold > 25)
            {
                Debug.Log($"Spawning wave for stability 50");
            }
            if (stabilityThreshold <= 25)
            {
                Debug.Log($"Spawning wave for stability 25");
            }
        }
        internal void CanSpawnEnemies()
        {
            if(!_enemyInputController) return;
            //if(!_enemyInputController.IsPlayerOnNavMesh()) return;
            int stabilityThreshold = Mathf.FloorToInt(TimeStabilityMeter.Instance.TimeStability / 10) * 10; // Round to nearest 10
        
            if (stabilityThreshold <= 50 && stabilityThreshold > 25 && !_spawnedWaves.Contains(50))
            {
                Debug.Log($"Spawning wave for stability 50");
                _spawnedWaves.Add(50);
                _spawnManager.WaveEnemySpawner();
            }
            if (stabilityThreshold <= 25 && !_spawnedWaves.Contains(25))
            {
                Debug.Log($"Spawning wave for stability 25");
                _spawnedWaves.Add(25);
                _spawnManager.WaveEnemySpawner();
            }
        }
    
        // internal void SpawnEnemies()
        // {
        //    Debug.Log("Spawning enemies");
        //     List<Vector3> spawnPoints = GetSpawnPoints();
        //     foreach (var point in spawnPoints)
        //     {
        //         _spawnManager.WaveEnemySpawner(point);
        //     }
        // }
    
        
        private void StoreEnemyPositions()
        {
            // Get all enemies that are children of the room prefab
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Enemy")) // Ensure enemies have the "Enemy" tag
                {
                    originalSpawnPoints.Add(child.position);
                    enemiesInRoom.Add(child.gameObject);
                }
            }

            //Debug.Log($"Room '{gameObject.name}' initialized with {originalSpawnPoints.Count} enemies.");
        }
    
        public List<Vector3> GetSpawnPoints()
        {
            //StoreEnemyPositions();
            Debug.Log($"Room '{gameObject.name}' initialized with {originalSpawnPoints.Count} enemies.");
            return originalSpawnPoints;
        }

        public List<GameObject> GetEnemies()
        {
            return enemiesInRoom;
        }
        
        public void SpawnEnemyWavechk()
        {
            // Your logic to spawn enemies
            Debug.Log("Spawning wave in RoomManager...");
        }

        internal void Reset()
        {
            _enemyController.Reset();
        }

        internal void ResetEnemiesInRoom()
        {
            _enemyDeathListener.OnAllEnemiesDead -= OnAllEnemiesDead;
            _enemyDeathListener = new EnemyDeathListener(gameObject);
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead += OnAllEnemiesDead;
            
        }
    }
}
