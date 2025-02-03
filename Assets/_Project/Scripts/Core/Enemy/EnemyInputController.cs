using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Enemy.FSM.EnemyStates;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(PlayerDetection))]
    public class EnemyInputController : InputController
    {
        public override event Action<Vector2> OnMoveInputUpdated;
        public override event Action OnAttackInputBegan;
        public override event Action OnAttackInputEnded;

        private PlayerDetection _playerDetection;

        private Transform _currentTarget; // current target to assign

        private readonly float _attackRange = 10f; // Attack range

        [SerializeField] private float attackCooldown = 3f; // Cooldown time between attacks

        private bool _isAttacking; // Tracks if an attack is in progress

        private bool _isCooldownActive; // Tracks if cooldown is active

        private Coroutine _attackCoroutine; // Holds the attack coroutine instance

        internal Transform ClosestPlayer; // closest player to the enemy which is the actual target

        internal NavMeshAgent Enemy; // navmesh agent

        internal StateManager StateManager; // reference to state manager

        private const float ChaseRange = 15f; // chase range

        internal Vector3 RoamingPosition; // random roaming position for an enemy

        private bool _isRoaming; // tracks if the enemy is roaming

        internal EnemyHUD EnemyHUD; // reference for enemy HUD

        internal const float SafeDistance = 15f; // The Distance the enemy should maintain from the player after fleeing

        internal float FleeTimer { get; set; } // Tracks time spent in FleeState

        internal float LastFleeDuration { get; set; } // Stores the duration of the last FleeState

        internal float FleeTimeout { get; private set; } = 5f; // Timeout threshold for FleeState
        
        public EnemyType enemyType; // The enemy type
        
        internal readonly float DistanceFromPlayer = 3.0f; // Distance between the player and enemy
        
        private void Awake()
        {
            Enemy = GetComponent<NavMeshAgent>();
            StateManager = GetComponent<StateManager>();
            InitializeState();
            EnemyHUD = GetComponentInChildren<EnemyHUD>();
        }

        private void InitializeState()
        {
            var states = new Dictionary<EnemyState, BaseState>();
            states.Clear();
            switch (enemyType)
            {
                case EnemyType.Basic:
                    states[EnemyState.Patrol] = new PatrolState(this);
                    states[EnemyState.Detect] = new DetectState(this);
                    states[EnemyState.Chase] = new ChaseState(this);
                    states[EnemyState.Attack] = new AttackState(this);
                    states[EnemyState.Flee] = new FleeState(this);
                    StateManager.InitializeStates(states, EnemyState.Patrol);
                    break;

                case EnemyType.Boss:
                    states[EnemyState.Detect] = new DetectState(this);
                    states[EnemyState.Chase] = new ChaseState(this);
                    states[EnemyState.Attack] = new AttackState(this);
                    StateManager.InitializeStates(states, EnemyState.Detect);
                    break;

                // Add additional cases for other enemy types
                default:
                    Debug.LogError($"Unhandled enemy type: {enemyType}");
                    break;
            }
        }

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);

            _playerDetection = GetComponent<PlayerDetection>();

            _playerDetection.Initialize(playerController);
            
            Enemy.speed = playerController.Stats.movementSpeed;
        }

        public void Disable()
        {
            // Disable AI logic
            _currentTarget = null;
        }
        

        //Method to check if the player in present on the navmesh rooms
        private bool IsPlayerOnNavMesh()
        {
            NavMeshHit hit;
            // Ensure the ClosestPlayer object exists before proceeding.
            if (!ClosestPlayer) return false;
            
            // Create a mask for the "Room" area on the NavMesh. 
            // NavMesh.GetAreaFromName("Room") fetches the index of the "Room" area,
            // and the bitwise shift (1 << index) creates a mask for this area.
            int roomAreaMask = 1 << NavMesh.GetAreaFromName("Room");
            bool isOnNavMesh = NavMesh.SamplePosition(ClosestPlayer.transform.position, out hit, 1.0f, roomAreaMask);

            // Return true if the player's position is on the NavMesh within the specified area.
            return isOnNavMesh;
        }
        
        // Method to find the closest player and check if its in detection range and in conical field of view
        internal bool FindPlayer()
        {
            ClosestPlayer = _playerDetection.FindClosestPlayerInRange();
            return ClosestPlayer && IsPlayerInCone();
        }
        
        // Method to check if player is in chase range and conical field of view
        internal bool CanChasePlayer()
        {
            if (!IsPlayerInCone()) return false;
            var distance = Vector3.Distance(Enemy.transform.position, ClosestPlayer.position);
            return distance <= ChaseRange; // Return true if within chase range
        }

        // Method to check if a player is in conical field of view
        private bool IsPlayerInCone()
        {
            // You need to implement this method in your PlayerDetection script to check if the player is in the cone
            return _playerDetection.IsPlayerInCone(ClosestPlayer);
        }

        // Method to start chasing the player if player is in conical field of view
        // ReSharper disable Unity.PerformanceAnalysis
        internal void StartChasing()
        {
            if (IsPlayerInCone() && IsPlayerOnNavMesh())
            {
                StartCoroutine(FollowPlayer()); // Start following the player
            }
        }

        // Method to stop chasing the player, i.e., resetting the navmesh agent path
        internal void StopChasing()
        {
            Enemy.ResetPath(); // Stop following the player
            Enemy.velocity = Vector3.zero;
        }

        // Method to check if the player is in attack range and conical field of view
        // ReSharper disable Unity.PerformanceAnalysis
        internal bool CanAttack()
        {
            return IsPlayerInCone() && IsPlayerInAttackRange() && IsPlayerOnNavMesh(); // Check if the player is within attack range
        }

        //method to check if the player is within attack range and cool down is not active
        // ReSharper disable Unity.PerformanceAnalysis
        internal void TryAttack()
        {
            if (CanAttack() && !_isCooldownActive)
            {
                // start attacking when the player is in range and not in cooldown
                StartAttack();
                _currentTarget = ClosestPlayer;
            }
            else if (_currentTarget == ClosestPlayer)
            {
                // Stopping the enemy attack if the player is not in range and cooldown is active
                StopAttack();
                _currentTarget = null;
            }
        }

        // method to check if player is in attack range
        internal bool IsPlayerInAttackRange()
        {
            var distanceToPlayer = Vector3.Distance(Enemy.transform.position, ClosestPlayer.position);
            return distanceToPlayer <= _attackRange;
        }


        // Start the attack process if not already attacking and not in cooldown
        private void StartAttack()
        {
            if (_isAttacking || _isCooldownActive) return; // Prevent multiple attacks or attacks during cooldown

            _isAttacking = true;
            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        // Stop the attack when the player is out of range
        internal void StopAttack()
        {
            if (!_isAttacking) return;

            // Invoke an event to notify end attack
            OnAttackInputEnded?.Invoke();
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine); // Stop the attack coroutine if it's running
            }

            _isAttacking = false;
        }

        // Coroutine to handle the attack and initiate cooldown after finishing
        private IEnumerator AttackCoroutine()
        {
            // Invoke an event to notify start attack
            OnAttackInputBegan?.Invoke();
            yield return new WaitForSeconds(attackCooldown); // Wait for the attack duration or cooldown time
            StartCoroutine(StartCooldown());
        }

        // Coroutine to handle cooldown
        private IEnumerator StartCooldown()
        {
            _isCooldownActive = true;
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown period
            _isCooldownActive = false;
            _isAttacking = false; // Allow a new attack after cooldown
        }

        //Method to rotate the enemy towards the player
        internal void RotateTowardsPlayer()
        {
            var player = ClosestPlayer;
            if (!player)
                PlayerController.MovementController.AimTransform.position = PlayerController.MovementController.Body.forward * 1000f;
            else
                PlayerController.MovementController.AimTransform.position = player.position;
        }

        // Coroutine to follow player
        private IEnumerator FollowPlayer()
        {
            while (CanChasePlayer())
            {
                // Move towards the player
                Enemy.SetDestination(ClosestPlayer.position);
                OnMoveInputUpdated?.Invoke(Enemy.velocity.normalized);

                // If a player is in DistanceFromPlayer range, stop chasing
                if (Vector3.Distance(Enemy.transform.position,
                        ClosestPlayer.transform.position) <= DistanceFromPlayer)
                {
                    StopChasing(); // Stop chasing once the DistanceFromPlayer range is reached
                    break;
                }

                yield return null; // Keep following every frame
            }
            OnMoveInputUpdated?.Invoke(Vector2.zero);
        }

        // Method to make the enemy move towards roam position
        private IEnumerator MoveToRoamPosition()
        {
            // Set flag to prevent multiple coroutines
            _isRoaming = true;

            // move enemy towards roam position
            Enemy.SetDestination(RoamingPosition);

            while (Enemy.remainingDistance > 0.5f)
            {
                OnMoveInputUpdated?.Invoke(Enemy.velocity.normalized);
                yield return null; // Wait for the next frame
            }

            // Once close enough, set roaming position to a new location
            RoamingPosition = GetRoamingPosition(Enemy.transform.position);
            OnMoveInputUpdated?.Invoke(Vector2.zero);
            _isRoaming = false;
        }

        // Method to get the roaming position
        internal Vector3 GetRoamingPosition(Vector3 startPosition)
        {
            return startPosition + GetRandomDirection() * UnityEngine.Random.Range(5, 15);
        }

        // Method to get the roaming direction
        private static Vector3 GetRandomDirection()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        // Method to start the coroutine to move the enemy to roam position
        internal void StartRoaming()
        {
            if (!_isRoaming)
            {
                StartCoroutine(MoveToRoamPosition());
            }
        }

        // Method to visualize the detect, chase, attack and conical FOV for testing purpose
        private void OnDrawGizmos()
        {
            if (Enemy == null) return;

            // Visualization of the chase range (sphere)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, ChaseRange);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, PlayerDetection._detectionRange);

            // Visualization of the attack range (sphere)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, _attackRange);

            // Visualization of the field of view (cone)
            Gizmos.color = Color.yellow;

            // Use the current forward direction of the enemy
            Vector3 forwardDirection = PlayerController.MovementController.Body.forward * ChaseRange; // Adjust cone length with chase range
            float fovHalfAngle = _playerDetection.fieldOfViewAngle * 0.5f;

            // Calculate the boundaries of the cone
            Vector3 leftBoundary = Quaternion.Euler(0, -fovHalfAngle, 0) * forwardDirection;
            Vector3 rightBoundary = Quaternion.Euler(0, fovHalfAngle, 0) * forwardDirection;

            // Draw the cone in the updated direction
            Gizmos.DrawLine(PlayerController.MovementController.Body.position,
                PlayerController.MovementController.Body.position + leftBoundary); // Left boundary
            Gizmos.DrawLine(PlayerController.MovementController.Body.position,
                PlayerController.MovementController.Body.position + rightBoundary); // Right boundary
            Gizmos.DrawLine(PlayerController.MovementController.Body.position,
                PlayerController.MovementController.Body.position + forwardDirection); // Forward direction line
        }

        private void Update()
        {
            OnMoveInputUpdated?.Invoke(Enemy.velocity.normalized);
        }
    }
}