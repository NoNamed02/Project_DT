using UnityEngine;

public class DefultAttack : EnemyState
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void Action()
    {
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.GetPlayer(), 5);
        base.Action();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
