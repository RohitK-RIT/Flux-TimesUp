using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Enemy.FSM.EnemyStates;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackState : BaseState
{
    private BossController boss;
    public enum BossAttackType { Shoot, SlowPlayer, ReduceTSM }
    public BossAttackType currentAttack;
    //public EnemyController enemy;
    private readonly EnemyInputController _enemyInputController;

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
        currentAttack = (BossAttackType)Random.Range(0, 3);
        ExecuteAttack();
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
                PlayerDebuffHandler.Instance.ApplySlow(2f);
                break;
            case BossAttackType.ReduceTSM:
                StabilityMeterHandler.Instance.DecreaseTSM(5);
                break;
        }
    }
    
    void ShootAtPlayer() {
        // Implement shooting logic
        Debug.Log("boss is shooting");
    }
}
