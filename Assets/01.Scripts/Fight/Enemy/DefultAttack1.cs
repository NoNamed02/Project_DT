using UnityEngine;

public class DefultAttack1 : EnemyState
{
    [SerializeField]
    private int currentHP = 1000;

    [SerializeField]
    private int IFNextStateIndex = 1;
    public override void Enter()
    {
        base.Enter();
        currentHP = GetHP();
    }
    public override void Action()
    {
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.GetPlayer(), 5);
        base.Action();
    }
    public override void CheckStateChange()
    {
        if (GetHP() <= currentHP - 5)
            RequestStateChange(IFNextStateIndex);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
