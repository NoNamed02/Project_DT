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
        BattleManager.Instance.HealTarget(BattleManager.Instance.GetEnemys()[Random.Range(0, BattleManager.Instance.GetEnemys().Count)], 5);
        RequestStateChange(AttackStateIndex);
        base.Action();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
