using UnityEngine;

public class beeDodge : EnemyState
{
    [SerializeField]
    public int nextAttackIndex = 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                Enemy,
                StatusAbnormalityNumber.dodge,
                10, 0
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
        return new IntentData(IntentType.Dodge);
    }
}
