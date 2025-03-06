using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class DetectState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
    
        // Constructor for the DetectState, setting the state key and storing a reference to the input controller
        public DetectState(EnemyInputController enemyInputController) : base(EnemyState.Detect) 
        {
            _enemyInputController = enemyInputController;
        }
    
        // Called when the enemy enters the DetectState
        public override void EnterState()
        {
            // Resets player movement when entering the state
            _enemyInputController.StopChasing(); 
            if (_enemyInputController.memberType == MemberType.Broadcaster)
            {
                EnemyManager.Instance.broadcasterEnemy = null;
            }
        }

        // Called when the enemy exits the DetectState
        public override void ExitState()
        {
            Debug.Log("Exiting Detect State");
            if (_enemyInputController.memberType == MemberType.Broadcaster)
            {
                EnemyManager.Instance.broadcasterEnemy = null;
            }
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            // If the player is detected, rotate towards them
            _enemyInputController.RotateTowardsPlayer();
            if (EnemyManager.Instance.helperEnemies.Count <=3)
            {
                
                //memberType = MemberType.Broadcaster;
                EnemyManager.Instance.EnemyDetected(_enemyInputController, _enemyInputController.ClosestPlayer.transform.position);
                //BroadcastSystem.BroadcastMessage(_enemyInputController, BroadcastType.Detect);
            }
        }
        
        public override EnemyState GetNextState()
        {
            // check if the closest player in range
            if (_enemyInputController.CanChasePlayer())
            {
                return EnemyState.Chase;
            }
            
            // check if any player is in the player detection range
            return _enemyInputController.FindPlayer()? 
                // If player is in Detect range, stay in detected state
                EnemyState.Detect :
                EnemyState.Patrol;
        }
    }
}