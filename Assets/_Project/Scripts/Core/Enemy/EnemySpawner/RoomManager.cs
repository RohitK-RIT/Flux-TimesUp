using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<Vector3> originalSpawnPoints = new List<Vector3>(); // Stores enemy spawn positions
    private List<GameObject> enemiesInRoom = new List<GameObject>(); // Stores references to original enemies
    
    private HashSet<int> _spawnedWaves = new HashSet<int>(); // Stores triggered waves
    private EnemyInputController _enemyInputController;
    private SpawnManager _spawnManager;
    private EnemyController _enemyController;

    private void Awake()
    {
        _enemyInputController = GetComponentInChildren<EnemyInputController>();
        _spawnManager = GetComponentInChildren<SpawnManager>();
        _enemyController = GetComponentInChildren<EnemyController>();
        StoreEnemyPositions(); // Store spawn points at the start

    }
    private void Update()
    {
        if(!_enemyInputController) return;
        if(!_enemyInputController.IsPlayerOnNavMesh()) return;
        int stabilityThreshold = Mathf.FloorToInt(TimeStabilityMeter.Instance.TimeStability / 10) * 10; // Round to nearest 10
        
        if (stabilityThreshold <= 50 && stabilityThreshold > 25 && !_spawnedWaves.Contains(50) && _enemyInputController.IsPlayerOnNavMesh())
        {
            Debug.Log($"Spawning wave for stability 50");
            _spawnedWaves.Add(50);
            SpawnEnemyWave();
        }
        if (stabilityThreshold <= 25 && !_spawnedWaves.Contains(25) && _enemyInputController.IsPlayerOnNavMesh())
        {
            Debug.Log($"Spawning wave for stability 25");
            _spawnedWaves.Add(25);
            SpawnEnemyWave();
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
    
    private void SpawnEnemyWave()
    {
        for (int i = 0; i < enemiesInRoom.Count; i++)
        {
            if (enemiesInRoom[i] != null)
            {
                enemiesInRoom[i].transform.position = originalSpawnPoints[i]; // Reset position
                enemiesInRoom[i].gameObject.SetActive(true); // Reactivate enemy
                _enemyController.Reset();
            }
        }
            
        Debug.Log($"Reactivated {enemiesInRoom.Count} enemies in room {gameObject.name}.");
    }

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
}
