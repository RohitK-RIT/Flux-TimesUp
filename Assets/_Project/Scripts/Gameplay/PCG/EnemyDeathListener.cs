using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Listens for enemy deaths and invokes an event when all enemies are dead.
    /// </summary>
    public class EnemyDeathListener
    {
        /// <summary>
        /// Event invoked when all enemies are dead.
        /// </summary>
        public event Action OnAllEnemiesDead;

        /// <summary>
        /// List of enemies
        /// </summary>
        private readonly List<EnemyController> _enemies;

        public EnemyDeathListener(GameObject enemiesParent)
        {
            // Get all the enemies in the parent object
            _enemies = enemiesParent.GetComponentsInChildren<EnemyController>().ToList();
            // If no enemies are found, log an error and return
            if(_enemies.Count == 0)
            {
                Debug.LogError("No enemies found in the parent object");
                return;
            }

            // Subscribe to the OnDeath event of each enemy
            foreach (var enemy in _enemies)
                enemy.OnDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath(PlayerController killingPlayer, PlayerController playerKilled, Weapon weaponKilledBy)
        {
            // Remove the enemy from the list and unsubscribe from the event
            _enemies.Remove((EnemyController)playerKilled);
            playerKilled.OnDeath -= OnEnemyDeath;

            // If there are no enemies left, invoke the event
            if (_enemies.Count == 0)
            {
                OnAllEnemiesDead?.Invoke();
            }
        }
    }
}