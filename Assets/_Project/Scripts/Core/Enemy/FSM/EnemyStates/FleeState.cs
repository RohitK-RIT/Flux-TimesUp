using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class FleeState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
    
        // Constructor for the FleeState, setting the state key and storing a reference to the input controller
        public FleeState(EnemyInputController enemyInputController) : base(EnemyState.Flee) 
        {
            _enemyInputController = enemyInputController;
        }

        // Called when the enemy exits the FleeState
        public override void ExitState()
        {
            Debug.Log("Exiting Flee State");
            // Record the time spent in the FleeState
            _enemyInputController.LastFleeDuration = _enemyInputController.FleeTimer;
        }

        // Called every frame while the enemy is in the FleeState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            if (!_enemyInputController.ClosestPlayer || !_enemyInputController.Enemy)
            {
                return;
            }
        
            // Increment the flee timer
            _enemyInputController.FleeTimer += Time.deltaTime;

            // Calculate the direction away from the player
            Vector3 fleeDirection = (_enemyInputController.Enemy.transform.position - _enemyInputController.ClosestPlayer.position).normalized;

            // Find a point to flee to
            Vector3 fleeTarget = _enemyInputController.Enemy.transform.position + fleeDirection * EnemyInputController.SafeDistance;
        
            // Set the NavMeshAgent's destination to the flee target
            if (UnityEngine.AI.NavMesh.SamplePosition(fleeTarget, out UnityEngine.AI.NavMeshHit hit, EnemyInputController.SafeDistance, UnityEngine.AI.NavMesh.AllAreas))
            {
                _enemyInputController.Enemy.SetDestination(hit.position);
            }
        }
        
        public override EnemyState GetNextState()
        {
            if (_enemyInputController.EnemyHUD.enemy.CurrentHealth<=0)
            {
                return EnemyState.Death;
            }
            // Check if the enemy is now at a safe distance
            return !_enemyInputController.ClosestPlayer ||
                   Vector3.Distance(_enemyInputController.Enemy.transform.position,
                       _enemyInputController.ClosestPlayer.position) > EnemyInputController.SafeDistance
                ? EnemyState.Patrol
                : EnemyState.Flee;
        }
    }
}
