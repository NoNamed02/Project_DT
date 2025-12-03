using UnityEngine;

public class plantsHeal : EnemyState
{
    [SerializeField]
    private int nextAttackIndex = 0;

    [Header("Èí¼ö ¼³Á¤")]
    [SerializeField] private int _healAmount = 6;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.HealTarget(Enemy, _healAmount);

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
        return new IntentData(IntentType.Heal, _healAmount);
    }
}
