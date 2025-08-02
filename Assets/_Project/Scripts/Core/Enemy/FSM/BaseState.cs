namespace _Project.Scripts.Core.Enemy.FSM
{
    // Abstract base class representing a general enemy state in the finite state machine
    public abstract class BaseState
    {
        // Key identifier for this state, associated with the EnemyState enum
        public EnemyState StateKey { get; private set; }

        // Constructor to initialize the state with a specific EnemyState key
        public BaseState(EnemyState key)
        {
            StateKey = key;
        }

        // Called once when the state is entered
        public virtual void EnterState() {}

        // Called once when the state is exited
        public virtual void ExitState() {}

        // Called on each frame while the state is active
        public virtual void UpdateState() {}

        // Determines the next state to transition to, based on conditions
        public virtual EnemyState GetNextState() => StateKey;
    }
}