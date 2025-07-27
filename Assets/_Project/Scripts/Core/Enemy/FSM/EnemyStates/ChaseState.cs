using UnityEngine;
namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class ChaseState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;

        // Constructor for the ChaseState, setting the state key and storing a reference to the input controller
        public ChaseState(EnemyInputController enemyInputController) : base(EnemyState.Chase)
        {
            _enemyInputController = enemyInputController;
        }

        // Called when the enemy enters the ChaseState
        public override void EnterState()
        {
            //Debug.Log("Entering Chase State");
        }

        // Called when the enemy exits the ChaseState
        public override void ExitState()
        {
            // Stop following the player when exiting this state
            _enemyInputController.StopChasing(); 
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            _enemyInputController.RotateTowardsPlayer();
            // Uses NavMeshAgent to move towards the player
            _enemyInputController.StartChasing();
        }
        
        public override EnemyState GetNextState()
        {
            if (_enemyInputController.EnemyHUD.enemy.CurrentHealth<=0)
            {
                return EnemyState.Death;
            }
            // If the player is out of chase range, transition to Detect state
            if (!_enemyInputController.CanChasePlayer())
            {
                return EnemyState.Detect;
            }

            if (_enemyInputController.enemyType == EnemyType.Boss)
            {
                return _enemyInputController.IsPlayerInAttackRange() ?
                    // If the player is in attack range, transition to Attack state
                    EnemyState.BossAttack :
                    EnemyState.Chase;
            }

            //Check if the player is now within attack range
            return _enemyInputController.IsPlayerInAttackRange() ?
                // If the player is in attack range, transition to Attack state
                EnemyState.Attack :
                EnemyState.Chase;
        }
    }
}