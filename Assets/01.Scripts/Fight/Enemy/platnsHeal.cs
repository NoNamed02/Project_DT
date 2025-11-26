using UnityEngine;
using UnityEngine.UIElements;

public class plantsHeal : EnemyState
{
    [SerializeField]
    private int AttackStateIndex = 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            base.Action();
            BattleManager.Instance.HealTarget(BattleManager.Instance.GetEnemys()[Random.Range(0, BattleManager.Instance.GetEnemys().Count)], 5);
            
            delayedAction(2f, () => { RequestStateChange(AttackStateIndex); });
        }
        );
    }

    public override void Exit()
    {
        base.Exit();
    }
}
