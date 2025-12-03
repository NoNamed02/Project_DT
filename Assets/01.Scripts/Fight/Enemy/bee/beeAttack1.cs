
using UnityEngine;

public class beeAttack1 : EnemyState
{
    [SerializeField]
    private int nextAttackIndex = 1;
    [SerializeField]
    private int beeDodgeIndex = 3;

    [SerializeField]
    public int currentHP = 0;


    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        // 자신의 현재 체력의 20%만큼(올림) 데미지 계산
        int damage = Mathf.CeilToInt(GetCurrentHP() * 0.2f);

        delayedAction(3f, () => {
            BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, damage);
            base.Action();
            RequestStateChange(nextAttackIndex);
            CheckStateChange();
        });
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        if (GetCurrentHP() <= currentHP - 5)
            RequestStateChange(beeDodgeIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        int damage = Mathf.CeilToInt(GetCurrentHP() * 0.2f);
        return new IntentData(IntentType.Attack, damage);
    }
}

