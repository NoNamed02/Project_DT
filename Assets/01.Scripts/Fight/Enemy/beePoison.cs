using UnityEngine;

public class beePoison : EnemyState
{
    [SerializeField]
    private int AttackStateIndex = 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 매 턴 고정 대미지
        // Poison();

        base.Action();
    }

    public override void CheckStateChange()
    {
        // 체력이 회복되어 최대 체력의 절반 초과하면 일반 공격으로 복귀
        if (GetCurrentHP() > GetMaxHP() / 2)
        {
            RequestStateChange(AttackStateIndex);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
