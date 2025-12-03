using UnityEngine;

public class plantsAttack3 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 0;
    [SerializeField] private int plantsHealIndex = 3;

    [Header("다회 공격 설정")]
    [SerializeField] private int _damagePerHit = 5;
    [SerializeField] private int _attackCount = 2;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
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
        if (GetCurrentHP() < GetMaxHP() / 2)
            RequestStateChange(plantsHealIndex);
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
