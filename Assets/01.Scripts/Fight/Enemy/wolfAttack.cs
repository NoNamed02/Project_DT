using UnityEngine;

public class wolfAttack : EnemyState
{
    [SerializeField]
    private int currentHP = 100;

    [SerializeField]
    private int DefenseIndex = 1;

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
        if(GetHP() <= currentHP-5)
            RequestStateChange(DefenseIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
