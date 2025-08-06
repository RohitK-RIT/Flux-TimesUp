using _Project.Scripts.Core.Enemy.GroupEnemyBehavior;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class DeathState : BaseState
    {
        private readonly EnemyInputController _enemyInputController;
        public DeathState(EnemyInputController enemyInputController) : base(EnemyState.Death) 
        {
            _enemyInputController = enemyInputController;
        }
        
        public override void EnterState()
        {
            _enemyInputController.DeathEffects();
            _enemyInputController.RoamingPosition = Vector3.zero;
            _enemyInputController.StopChasing();
            _enemyInputController.StopAttack();
            _enemyInputController.ClosestPlayer = null;
            float lifespan = Time.time - _enemyInputController.spawnTime;
            DataCollectionEvents.EnemyDied(_enemyInputController.enemyID, lifespan);
            _enemyInputController.gameObject.SetActive(false);
        }

        public override EnemyState GetNextState()
        {
            return EnemyState.Death;
        }
    }
}