using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class ChargerType : MonoBehaviour
{
    [SerializeField] private float lowHealthThreshold = 20f; // Trigger when health drops below this
    private EnemyInputController _enemyInputController;
    private SpawnManager _spawnManager;
    private bool hasSpawnedChargers = false; // Ensure Chargers spawn only once

    void Awake()
    {
        _enemyInputController = GetComponent<EnemyInputController>();
        _spawnManager = GetComponent<SpawnManager>();
    }

    private void Update()
    {
        if (!hasSpawnedChargers && _enemyInputController.EnemyHUD.enemy.CurrentHealth <= lowHealthThreshold)
        {
            hasSpawnedChargers = true;
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
