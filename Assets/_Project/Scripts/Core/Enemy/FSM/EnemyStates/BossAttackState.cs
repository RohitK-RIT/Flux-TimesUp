using _Project.Scripts.Core.Enemy.Types.Boss;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class BossAttackState : BaseState
    {
        // Stores the current attack type
        private BossAttackType _currentAttack;
        
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
        
        // Duration for each attack before switching
        private readonly float _attackDuration = 5f;
        
        // Cooldown duration between attacks
        private readonly float _cooldownDuration = 2f;
        
        // Timer to track attack duration and cooldown time
        private float _stateTimer;
        
        // Flag to check if the boss is in cooldown mode
        private bool _isOnCooldown;
        
        // VFX Prefabs for different attack types
        private GameObject _currentVFX;
    
        // Constructor initializing the attack state with an EnemyInputController
        public BossAttackState(EnemyInputController enemyInputController) : base(EnemyState.BossAttack)
        {
            _enemyInputController = enemyInputController;
        }

        public override void EnterState() 
        {
            Debug.Log("in enter boss attack state");
            _currentAttack = (BossAttackType)Random.Range(0, 3); // Pick a new attack when entering the state
            _stateTimer = 0f;
            _isOnCooldown = false;
            ActivateVFX();
        }

        public override void ExitState()
        {
            Debug.Log("in exit boss attack state");
            DeactivateVFX();
        }

        public override void UpdateState()
        {
            // Update the timer each frame
            _stateTimer += Time.deltaTime;

            if (_isOnCooldown)
            {
                // If the cooldown period is over, start a new attack
                if (_stateTimer >= _cooldownDuration)
                {
                    _isOnCooldown = false;
                    _stateTimer = 0f;
                    
                    // Randomly choose the next attack
                    _currentAttack = (BossAttackType)Random.Range(0, 3);
                    ActivateVFX(); // Activate new attack VFX
                }
            }
            else
            {
                // If the attack duration is over, switch to cooldown mode
                if (_stateTimer >= _attackDuration)
                {
                    _isOnCooldown = true;
                    _stateTimer = 0f;
                    DeactivateVFX(); // Deactivate VFX when attack ends
                    Debug.Log("Boss is on cooldown, waiting...");
                }
                else
                {
                    // Perform the selected attack
                    ExecuteAttack();
                }
            }
            Debug.Log("in update boss attack state");
        }

        public override EnemyState GetNextState()
        {
            // If a player is still in attack range, stay in attack state
            if (_enemyInputController.IsPlayerInAttackRange())
            {
                return EnemyState.BossAttack;
            }
            //Check if the player is now within chase range
            return _enemyInputController.CanChasePlayer() ? 
                // If the player is in chase range, transition to Chase state
                EnemyState.Chase : 
                EnemyState.Detect;
        }

        // Executes the currently selected attack
        void ExecuteAttack() {
            switch (_currentAttack) {
                case BossAttackType.Shoot:
                    ShootAtPlayer();
                    break;
                case BossAttackType.SlowPlayer:
                    _enemyInputController.StopAttack();
                    PlayerSpeedHandler.Instance.ReduceSpeed(5f);
                    break;
                case BossAttackType.ReduceTSM:
                    _enemyInputController.StopAttack();
                    TimeStabilityMeter.Instance.DecreaseTSM(0.1f);
                    break;
            }
        }
    
        // Handles the shooting attack
        void ShootAtPlayer() 
        {
            Debug.Log("boss is shooting");
            if (_enemyInputController.CanAttack())
            {
                _enemyInputController.AttackPlayer();
            }
        }
        
        void ActivateVFX()
        {
            DeactivateVFX(); // Ensure previous VFX is turned off before activating a new one

            switch (_currentAttack)
            {
                case BossAttackType.SlowPlayer:
                    _currentVFX = Object.Instantiate(_enemyInputController.slowPlayerVFX, _enemyInputController.transform.position, Quaternion.identity);
                    break;
                case BossAttackType.ReduceTSM:
                    _currentVFX = Object.Instantiate(_enemyInputController.reduceTSMVFX, _enemyInputController.transform.position, Quaternion.identity);
                    break;
            }

            if (_currentVFX != null)
            {
                _currentVFX.transform.SetParent(_enemyInputController.transform); // Keep VFX attached to the boss
            }
        }

        // Deactivates the current VFX
        void DeactivateVFX()
        {
            if (_currentVFX != null)
            {
                Object.Destroy(_currentVFX);
                _currentVFX = null;
            }
        }
    }
}
