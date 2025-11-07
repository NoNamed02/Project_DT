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
        currentHP = GetCurrentHP();
    }
    public override void Action()
    {
        BattleManager.Instance.ApplyBleeding(BattleManager.Instance.GetPlayer(), 2, 2);
        base.Action();
    }
    public override void CheckStateChange()
    {
        if (GetCurrentHP() <= currentHP - 5)
            RequestStateChange(IFNextStateIndex);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
