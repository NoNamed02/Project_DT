using UnityEngine;

public class beeAttack3 : EnemyState
{
    [SerializeField]
    private int nextAttackIndex = 0;
    [SerializeField]
    private int beeDodgeIndex = 3;

    [SerializeField]
    public int currentHP = 0;

    [SerializeField]
    private int _attackCount = 3;
    [SerializeField]
    private int _damagePerHit = 3;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, 3);
            delayedAction(0.5f, () =>
            {
                BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, 3);
                delayedAction(0.5f, () =>
                {
                    BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, 3);
                    base.Action();
                    RequestStateChange(nextAttackIndex);
                    CheckStateChange();
                });
            });
        });
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        if (GetCurrentHP() >= currentHP && currentHP != GetMaxHP())
            RequestStateChange(beeDodgeIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        int totalDamage = _damagePerHit * _attackCount;
        string desc = $"{_damagePerHit}의 피해를 {_attackCount}회 입힙니다.";
        return new IntentData(IntentType.Attack, _damagePerHit, desc);
    }
}
