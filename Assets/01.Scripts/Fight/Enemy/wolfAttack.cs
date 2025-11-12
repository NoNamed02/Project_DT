using UnityEngine;

public class wolfAttack : EnemyState
{
    [SerializeField]
    private int currentHP = 100;

    [SerializeField]
    private int DefenseStateIndex = 1;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        // °ø°Ý 5
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, 5);
        // ÃâÇ÷, 10% È®·ü
        // bleeding();
        base.Action();
    }

    public override void CheckStateChange()
    {
        if (GetCurrentHP() <= currentHP - 5)
            RequestStateChange(DefenseStateIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
