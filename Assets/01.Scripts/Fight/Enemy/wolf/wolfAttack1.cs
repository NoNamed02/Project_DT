using UnityEngine;

public class wolfAttack1 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 1;
    [SerializeField] private int wolfDefenseIndex = 3;

    [SerializeField]
    private int currentHP = 0;

    [SerializeField]
    private int _damagePerHit = 4;
    [SerializeField]
    private int _attackCount = 2;
    

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        delayedAction(3f, () => {
            // 6의 피해로 공격
            BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, _damagePerHit);
            delayedAction(0.5f, () =>
            {
                BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, _damagePerHit);
                base.Action();
                RequestStateChange(nextAttackIndex);
            });
        });
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        if (GetCurrentHP() <= currentHP - 6)
            RequestStateChange(wolfDefenseIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        string desc = $"{_damagePerHit}의 피해를 {_attackCount}회 입힙니다.";
        return new IntentData(IntentType.Attack, _damagePerHit, desc);
    }
}
