using UnityEngine;

public class plantAttack : EnemyState
{
    [SerializeField]
    private int currentHP = 100;

    [SerializeField]
    private int HealStateIndex = 1;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.GetPlayer(), 3);
        base.Action();
    }

    public override void CheckStateChange()
    {
        // 체력이 깎였으면 회복 상태로 전환
        if (GetCurrentHP() < currentHP)
        {
            RequestStateChange(HealStateIndex);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

