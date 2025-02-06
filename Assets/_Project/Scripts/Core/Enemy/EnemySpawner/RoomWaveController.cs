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
        internal readonly List<Vector3> OriginalSpawnPoints = new List<Vector3>(); // Stores enemy spawn positions
        internal readonly List<GameObject> EnemiesInRoom = new List<GameObject>(); // Stores references to original enemies
    
        private readonly HashSet<int> _spawnedWaves = new HashSet<int>(); // Stores triggered waves
        private EnemyInputController _enemyInputController;
        private SpawnManager _spawnManager;
        private EnemyController _enemyController;
        private EnemyDeathListener _enemyDeathListener;

        private void Awake()
        {
            _enemyInputController = GetComponentInChildren<EnemyInputController>();
            _spawnManager = GetComponentInChildren<SpawnManager>();
            _enemyController = GetComponentInChildren<EnemyController>();
            
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
    
        private void StoreEnemyPositions()
        {
            // Get all enemies that are children of the room prefab
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Enemy")) // Ensure enemies have the "Enemy" tag
                {
                    OriginalSpawnPoints.Add(child.position);
                    EnemiesInRoom.Add(child.gameObject);
                }
            }
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
