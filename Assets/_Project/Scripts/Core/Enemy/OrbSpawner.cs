using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _orbPrefab; // Assign the orb prefab in inspector
    [SerializeField] private Transform _spawnPoint; // Point where orb spawns
    [SerializeField] private int _healAmount = 1; // Amount to heal per second

    private GameObject _spawnedOrb;
    private bool _orbSpawned;
    private Coroutine _healCoroutine;
    internal void SpawnOrb(float currentHealth)
    {
        _spawnedOrb = Instantiate(_orbPrefab, _spawnPoint.position, Quaternion.identity);
        _orbSpawned = true;

        // Start healing coroutine
        _healCoroutine = StartCoroutine(HealOverTime(currentHealth));
    }

    private IEnumerator HealOverTime(float currentHealth)
    {
        while (_spawnedOrb != null)
        {
            currentHealth += _healAmount;
            yield return new WaitForSeconds(1f);
        }

        // Stop healing when orb is destroyed
        StopCoroutine(_healCoroutine);
    }

    public void DestroyOrb()
    {
        if (_spawnedOrb != null)
        {
            Destroy(_spawnedOrb);
            _spawnedOrb = null;
        }
    }
}
