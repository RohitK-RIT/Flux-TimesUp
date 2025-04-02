using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Enemy.FSM.EnemyStates;
using _Project.Scripts.Gameplay.Time_Stability_Meter;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackState : BaseState
{
    private BossController boss;
    public enum BossAttackType { Shoot, SlowPlayer, ReduceTSM }
    public BossAttackType currentAttack;
    //public EnemyController enemy;
    private readonly EnemyInputController _enemyInputController;
    private float attackDuration = 5f; // Duration for each attack
    private float cooldownDuration = 2f; // Cooldown between attacks
    private float stateTimer = 0f;
    private bool isOnCooldown = false;
    
    public BossAttackState(EnemyInputController enemyInputController) : base(EnemyState.BossAttack)
    {
        _enemyInputController = enemyInputController;
    }

    public override void EnterState() {
        // boss = _enemyInputController.Enemy.GetComponent<BossController>();
        //
        // if (boss == null) {
        //     Debug.LogError("BossAttackState: Enemy is not a BossController!");
        //     return;
        // }
        //
        
        Debug.Log("in enter boss attack state");
    }

    public override void ExitState()
    {
        Debug.Log("in exit boss attack state");
    }

    public override void UpdateState()
    {
        stateTimer += Time.deltaTime;

        if (isOnCooldown)
        {
            if (stateTimer >= cooldownDuration)
            {
                isOnCooldown = false;
                stateTimer = 0f;
                currentAttack = (BossAttackType)Random.Range(0, 3); // Pick a new attack
                Debug.Log("Boss attack resumes: " + currentAttack);
            }
        }
        else
        {
            if (stateTimer >= attackDuration)
            {
                isOnCooldown = true;
                stateTimer = 0f;
                Debug.Log("Boss is on cooldown, waiting...");
            }
            else
            {
                ExecuteAttack();
            }
        }
        Debug.Log("in update boss attack state");
    }

    public override EnemyState GetNextState()
    {
        Debug.Log("in getnext boss attack state");
        return EnemyState.BossAttack;
    }

    void ExecuteAttack() {
        switch (currentAttack) {
            case BossAttackType.Shoot:
                ShootAtPlayer();
                break;
            case BossAttackType.SlowPlayer:
                _enemyInputController.StopAttack();
                PlayerDebuffHandler.Instance.ApplySlow(5f);
                break;
            case BossAttackType.ReduceTSM:
                _enemyInputController.StopAttack();
                TimeStabilityMeter.Instance.DecreaseTSM(0.1f);
                break;
        }
    }
    
    void ShootAtPlayer() {
        // Implement shooting logic
        Debug.Log("boss is shooting");
        if (_enemyInputController.CanAttack())
        {
            _enemyInputController.AttackPlayer();
        }
    }
}
