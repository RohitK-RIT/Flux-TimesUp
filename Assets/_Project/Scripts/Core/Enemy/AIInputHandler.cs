using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class AIInputHandler : MonoBehaviour
{
    private PlayerDetection _playerDetection; // Reference to the PlayerDetection component

    private MovementController _characterMovement; // Reference to CharacterMovement for HandleLook and in future HandleMovement

    private EnemyAttack _enemyAttack; // Reference to EnemyAttack for handling attacks

    private Transform _currentTarget; // Keeps track of the current target (player)

    private void Awake()
    {
        // Get references to necessary components
        _playerDetection = GetComponent<PlayerDetection>();
        _characterMovement = GetComponent<MovementController>();
        _enemyAttack = GetComponent<EnemyAttack>();
    }

    void Update()
    {
        // Get the closest player detected by the PlayerDetection component
        Transform closestPlayer = _playerDetection.FindClosestPlayerInRange();

        // Check if closest player is found and within the conical field of view
        if (closestPlayer && _playerDetection.IsPlayerInCone(closestPlayer))
        {
            // Rotating the enemy in the direction of the closest player
            RotateTowardsPlayer(closestPlayer);

            // Try to attack the player
            TryAttack(closestPlayer);
        }
        else if (_currentTarget != null)
        {
            // Player moved out of range, stop attacking
            _enemyAttack.StopAttack();
            _currentTarget = null;
        }
    }

    // Method to rotate the player towards the player
    private void RotateTowardsPlayer(Transform player)
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Keep rotation on the horizontal plane

        // Use the HandleLook method from CharacterMovement to rotate the enemy
        _characterMovement.HandleLook(directionToPlayer);
    }

    // Tries to attack the player if within range and not on cooldown.
    private void TryAttack(Transform player)
    {
        // Check if the player is within attack range
        if (_enemyAttack.IsPlayerInAttackRange(player))
        {
            if (!_enemyAttack.IsOnCooldown())
            {
                Debug.Log("Not in cooldown");
                _enemyAttack.StartAttack();
                _currentTarget = player; // Set current target
            }
            else
            {
                Debug.Log("Currently in cooldown");
                _enemyAttack.StopAttack();
            }
        }
        else if (_currentTarget == player)
        {
            // Player is out of range; stop attacking
            _enemyAttack.StopAttack();
            _currentTarget = null;
        }
    }
}