using UnityEngine;

public class plantsAttack2 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 2;
    [SerializeField] private int plantsHealIndex = 3;

    [Header("약화 설정")]
    [SerializeField] private int _weakenAmount = 2;
    [SerializeField] private int _weakenDuration = 2;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.weaken,
                _weakenAmount, _weakenDuration
            );

            base.Action();
            RequestStateChange(nextAttackIndex);
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
        string desc = $"공격력을 {_weakenAmount}만큼 {_weakenDuration}턴 동안 감소시킵니다.";
        return new IntentData(IntentType.Weaken, _weakenAmount, desc);
    }
}
