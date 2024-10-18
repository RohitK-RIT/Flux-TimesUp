using System.Collections;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
     [SerializeField] private Weapon weapon; // Reference to the weapon
     [SerializeField] private float attackRange = 5f; // Attack range
     [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

     
     private bool _isAttacking;      // Tracks if an attack is in progress
     private bool _isCooldownActive; // Tracks if cooldown is active
     private Coroutine _attackCoroutine; // Holds the attack coroutine instance
     private float _lastAttackTime;
     
     private void Awake()
     {
         if (weapon == null)
         {
             weapon = GetComponentInChildren<Weapon>();
         }
     }
     
     // Method to check if player is within the attack range.
     public bool IsPlayerInAttackRange(Transform player)
     {
         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
         return distanceToPlayer <= attackRange;
     }


     // Start the attack process if not already attacking and not in cooldown
     public void StartAttack()
     {
         if (_isAttacking || _isCooldownActive) return;  // Prevent multiple attacks or attacks during cooldown

         Debug.Log("Starting attack.");
         _isAttacking = true;
         _lastAttackTime = Time.time;                   // Record the attack start time
         _attackCoroutine = StartCoroutine(AttackCoroutine());
     }
     
     // Stop the attack when the player is out of range
     public void StopAttack()
     {
         if (_isAttacking)
         {
             Debug.Log("Stopping attack.");
             weapon.EndAttack();    // Stop the weapon from attacking
             if (_attackCoroutine != null)
             {
                 StopCoroutine(_attackCoroutine);  // Stop the attack coroutine if it's running
             }
             _isAttacking = false;
         }
     }

     // Check if the cooldown period is active
     public bool IsOnCooldown()
     {
         return _isCooldownActive;
     }

    // Coroutine to handle the attack and initiate cooldown after finishing
     private IEnumerator AttackCoroutine()
     {
         weapon.BeginAttack();  // Start the attack
         yield return new WaitForSeconds(attackCooldown);  // Wait for the attack duration or cooldown time
         StartCoroutine(StartCooldown());
     }

     // Coroutine to handle cooldown
     private IEnumerator StartCooldown()
     {
         _isCooldownActive = true;
         Debug.Log("Cooldown active.");
         yield return new WaitForSeconds(attackCooldown);  // Wait for the cooldown period
         _isCooldownActive = false;
         Debug.Log("Cooldown finished.");
         _isAttacking = false;  // Allow a new attack after cooldown
     }
}

