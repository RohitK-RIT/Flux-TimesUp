namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class DeathState : BaseState
    {
        private readonly EnemyInputController _enemyInputController;
        public DeathState(EnemyInputController enemyInputController) : base(EnemyState.Detect) 
        {
            _enemyInputController = enemyInputController;
        }

        public override void EnterState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override EnemyState GetNextState()
        {
            throw new System.NotImplementedException();
        }
    }
}