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

    private void Awake()
    {
        _enemyInputController = GetComponentInChildren<EnemyInputController>();
        _spawnManager = GetComponentInChildren<SpawnManager>();
    }
    private void Update()
    {
        int stabilityThreshold = Mathf.FloorToInt(TimeStabilityMeter.Instance.TimeStability / 10) * 10; // Round to nearest 10
        if(_enemyInputController == null) return;
        if (stabilityThreshold <= 50 && !_spawnedWaves.Contains(stabilityThreshold) && _enemyInputController.IsPlayerOnNavMesh())
        {
            Debug.Log($"Spawning wave for stability {stabilityThreshold}");
            _spawnedWaves.Add(stabilityThreshold);
            SpawnEnemies();
        }
    }
    
    internal void SpawnEnemies()
    {
       Debug.Log("Spawning enemies");
        List<Vector3> spawnPoints = GetSpawnPoints();
        foreach (var point in spawnPoints)
        {
            _spawnManager.WaveEnemySpawner(point);
        }
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

    private void Start()
    {
        
        //Debug.Log("Enemies: " + string.Join(", ", GetEnemies().ConvertAll(e => e.name)));
    }

    public List<Vector3> GetSpawnPoints()
    {
        StoreEnemyPositions();
        Debug.Log($"Room '{gameObject.name}' initialized with {originalSpawnPoints.Count} enemies.");
        return originalSpawnPoints;
    }

    public List<GameObject> GetEnemies()
    {
        return enemiesInRoom;
    }
}
