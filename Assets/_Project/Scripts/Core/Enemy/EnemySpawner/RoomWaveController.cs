using System.Collections.Generic;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Gameplay.PCG;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.EnemySpawner
{
    public class RoomWaveController : MonoBehaviour
    {
        // Stores the original spawn positions of enemies in the room
        internal readonly List<Vector3> OriginalSpawnPoints = new List<Vector3>();
        
        // Stores references to enemy GameObjects in the room
        internal readonly List<GameObject> EnemiesInRoom = new List<GameObject>();
    
        // Keeps track of which waves have been triggered to prevent duplicate spawns
        private readonly HashSet<int> _spawnedWaves = new HashSet<int>();
        private EnemyInputController _enemyInputController;
        private SpawnManager _spawnManager;
        private EnemyController _enemyController;
        private EnemyDeathListener _enemyDeathListener;

        private void Awake()
        {
            _enemyInputController = GetComponentInChildren<EnemyInputController>();
            _spawnManager = GetComponentInChildren<SpawnManager>();
            _enemyController = GetComponentInChildren<EnemyController>();
            
            // Store enemy spawn points at the beginning
            StoreEnemyPositions();
            
            // Initialize death listener only for Basic enemies
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
            _spawnedWaves.Clear();
        }
        
        private void OnDisable()
        {
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead -= OnAllEnemiesDead;
            _spawnedWaves.Clear();
        }
        
        // Called when all enemies in the room are dead. Triggers the next wave if conditions are met.
        private void OnAllEnemiesDead()
        {
            Debug.Log("All enemies dead");
            CanSpawnEnemies();
        }
        
        // Checks if a new wave can be spawned based on the time stability meter.
        private void CanSpawnEnemies()
        {
            if(!_enemyInputController) return;
            
            // Get the current stability threshold rounded down to the nearest 10
            int stabilityThreshold = Mathf.FloorToInt(TimeStabilityMeter.Instance.TimeStability / 10) * 10;
        
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
    
        // Stores the initial positions of all enemies in the room for respawning purposes.
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
        
        // Resets the health & ammo of all enemies in the room.
        internal void Reset()
        {
            _enemyController.Reset();
        }

        // Resets the enemy death listener and re-subscribes to the event.
        internal void ResetEnemiesInRoom()
        {
            _enemyDeathListener.OnAllEnemiesDead -= OnAllEnemiesDead;
            _enemyDeathListener = new EnemyDeathListener(gameObject);
            if(_enemyDeathListener != null)
                _enemyDeathListener.OnAllEnemiesDead += OnAllEnemiesDead;
            
        }
    }
}
