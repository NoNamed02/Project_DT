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
        base.Action();
        delayedAction(3f, () => { 
            BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, 3);

            delayedAction(2f, () =>
            {
                CheckStateChange();
            });
        });
        
    }

    public override void CheckStateChange()
    {
        // 체력이 5 이상 깎였으면 회복 상태로 전환
        if (GetCurrentHP() <= currentHP - 5)
        {
            RequestStateChange(HealStateIndex);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

