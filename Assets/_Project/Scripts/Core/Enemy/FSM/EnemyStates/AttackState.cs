using _Project.Scripts.Core.Enemy.GroupEnemyBehavior;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class AttackState : BaseState
    {
        // Reference to the enemy's input controller
        internal readonly EnemyInputController _enemyInputController;
        
        // Constructor for the AttackState, setting the state key and storing a reference to the input controller
        public AttackState(EnemyInputController enemyInputController) : base(EnemyState.Attack)
        {
            _enemyInputController = enemyInputController;
        }

        // Called when the enemy enters the AttackState
        public override void EnterState()
        {
            // Stop chasing the player when entering attack state
            _enemyInputController.StopChasing();
            
            // If the broadcaster re-enters the attack state it should not be the broadcaster again
            if (_enemyInputController.MemberType == MemberType.Broadcaster)
            {
                EnemyManager.Instance.BroadcasterEnemy = null;
            }
        }

        // Called when the enemy exits the AttackState
        public override void ExitState()
        {
            // Stop any ongoing attack actions
            _enemyInputController.StopAttack();
            _enemyInputController.StopChasing();
            
            // If the broadcaster leaves the attack state it should not be the broadcaster again
            if (_enemyInputController.MemberType == MemberType.Broadcaster)
            {
                EnemyManager.Instance.BroadcasterEnemy = null;
            }

        }

        // Called every frame while the enemy is in the AttackState
        public override void UpdateState()
        {
            // Broadcast message when health is low and helpers are less than 3
            if (EnemyManager.Instance.HelperEnemies.Count <=3 && _enemyInputController.EnemyHUD.enemy.CurrentHealth < 60)
            {
                
                //memberType = MemberType.Broadcaster;
                EnemyManager.Instance.BroadcastMessage(_enemyInputController, _enemyInputController.ClosestPlayer.transform.position);
                //BroadcastSystem.BroadcastMessage(_enemyInputController, BroadcastType.Detect);
            }
            
            // Check if the player health is low
            if (_enemyInputController.EnemyHUD.enemy.CurrentHealth < 50)
            {
                // Check if the enemy has been in FleeState recently and exceeded timeout or Enemy type is boss
                if ((_enemyInputController.LastFleeDuration >= _enemyInputController.FleeTimeout && _enemyInputController.CanAttack())|| _enemyInputController.enemyType == EnemyType.Boss)
                {
                    // Continue attacking as timeout condition overrides health
                    AttackPlayer();
                }
                else
                {
                    //Health is low. Transitioning to Flee state.
                    _enemyInputController.StateManager.TransitionToState(EnemyState.Flee);
                }
            }
            // If health is not low check if enemy can attack and not a charger type
            else if (_enemyInputController.CanAttack() && (_enemyInputController.enemyType != EnemyType.Charger))
            {
                AttackPlayer();
            }
            
            // If enemy is charger type start attacking directly
            else if (_enemyInputController.enemyType == EnemyType.Charger)
            {
                _enemyInputController.RotateTowardsPlayer();
                _enemyInputController.StartChasing();
                _enemyInputController.StartAttack();
            }
        }

        private void AttackPlayer()
        {
            // Face towards the player
            _enemyInputController.RotateTowardsPlayer();

            // Player is in attack range, so keep attacking
            _enemyInputController.TryAttack();
            
            // Attack and move towards the player till the DistanceFromPlayer is reached
            if ( Vector3.Distance(_enemyInputController.Enemy.transform.position,
                    _enemyInputController.ClosestPlayer.transform.position) <= _enemyInputController.EnemyDistanceFromPlayer)
            {
               _enemyInputController.StopChasing();
            }
            else
            {
                _enemyInputController.StartChasing();
            }
        }
        
        public override EnemyState GetNextState()
        {
            if (_enemyInputController.enemyType == EnemyType.Basic)
            {
                if (_enemyInputController.RangedWeapon.IsReloading)
                {
                    Debug.Log("is reloading going to patrol");
                    return EnemyState.Patrol;
                }
            }
            
            // If a player is still in attack range, stay in attack state

            if (_enemyInputController.IsPlayerInAttackRange())
            {
                return EnemyState.Attack;
            }
            //Check if the player is now within chase range
            return _enemyInputController.CanChasePlayer() ? 
                // If the player is in chase range, transition to Chase state
                EnemyState.Chase : 
                EnemyState.Detect;
        }
    }
}