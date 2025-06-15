using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class PatrolState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
    
        // Constructor for the FleeState, setting the state key and storing a reference to the input controller
        public PatrolState(EnemyInputController enemyInputController) : base(EnemyState.Patrol) 
        {
            _enemyInputController = enemyInputController;
        }
        private Vector3 _startingPosition;
        
        // Called when the enemy enters the FleeState
        public override void EnterState()
        {
            // Set the starting position everytime on entering the PatrolState
            _startingPosition = _enemyInputController.Enemy.transform.position;
            
            // Set the roaming position
            _enemyInputController.RoamingPosition = _enemyInputController.GetRoamingPosition(_startingPosition);
        }

        // Called when the enemy exits the FleeState
        public override void ExitState()
        {
            //Debug.Log("Exiting Patrol State");
            //_enemyInputController.StopChasing();
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            _enemyInputController.StartRoaming(_enemyInputController.RoamingPosition);
            
            // Get a new roaming position if the enemy reaches the previous roaming position
            if (Vector3.Distance(_enemyInputController.Enemy.transform.position, _enemyInputController.RoamingPosition) <= 1f)
            {
                _enemyInputController.RoamingPosition = _enemyInputController.GetRoamingPosition(_startingPosition);
            }
        }

        public override EnemyState GetNextState()
        {
            // If enemy is reloading stay in patrol state
            if (_enemyInputController.enemyType == EnemyType.Basic)
            {
                if (_enemyInputController.RangedWeapon.IsReloading)
                {
                    return EnemyState.Patrol;
                }
            }
            
            // Check if players are in the detection range
            return _enemyInputController.FindPlayer()? EnemyState.Detect :
                EnemyState.Patrol;
        }
    }
}
