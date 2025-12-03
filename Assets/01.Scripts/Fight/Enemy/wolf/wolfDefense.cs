using UnityEngine;

public class wolfDefense : EnemyState
{
    [SerializeField]
    private int nextAttackIndex = 0;

    [SerializeField]
    private int _shieldAmount = 7;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        
        delayedAction(3f, () => {
            // 자신에게 방어막 7 추가
            BattleManager.Instance.ApplyShield(
                Enemy, _shieldAmount
            );

            base.Action();
            RequestStateChange(nextAttackIndex);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        return new IntentData(IntentType.Defend, _shieldAmount);
    }
}


