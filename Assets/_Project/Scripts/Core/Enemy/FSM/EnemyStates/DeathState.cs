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
            Debug.Log("Death State");
        }

        public override void ExitState()
        {
            Debug.Log("Death State");
        }

        public override void UpdateState()
        {
            Debug.Log("Death State");
            _enemyInputController.gameObject.SetActive(false);
        }

        public override EnemyState GetNextState()
        {
            Debug.Log("Death State");
            return EnemyState.Death;
        }
    }
}