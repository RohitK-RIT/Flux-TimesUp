using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.FSM.EnemyStates;
using Unity.VisualScripting;
using UnityEngine;

public class BossAttackState : AttackState
{
    private BossController boss;
    public enum BossAttackType { Shoot, SlowPlayer, ReduceTSM }
    public BossAttackType currentAttack;
    //public EnemyController enemy;

    public BossAttackState(EnemyInputController enemyInputController, BossController boss, BossAttackType currentAttack) : base(enemyInputController)
    {
        this.boss = boss;
        this.currentAttack = currentAttack;
    }

    public override void EnterState() {
        base.EnterState();
        boss = _enemyInputController.Enemy.GetComponent<BossController>();

        if (boss == null) {
            Debug.LogError("BossAttackState: Enemy is not a BossController!");
            return;
        }

        currentAttack = (BossAttackType)Random.Range(0, 3);
        ExecuteAttack();
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
    }
}
