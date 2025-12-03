using UnityEngine;

public class beeAttack2 : EnemyState
{
    [SerializeField]
    private int nextAttackIndex = 2;
    [SerializeField]
    private int beeDodgeIndex = 3;

    [SerializeField]
    public int currentHP = 0;

    [SerializeField]
    private int _poisonStack = 2;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {

        delayedAction(3f, () =>
        {
            // 중독 스택 부여
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.poison,
                2,
                0 //HoldingTime은 poison에서 사용하지 않음
            );

            base.Action();
            RequestStateChange(nextAttackIndex);
            CheckStateChange();
        });
        
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
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
        return new IntentData(IntentType.Poison, _poisonStack);
    }
}
