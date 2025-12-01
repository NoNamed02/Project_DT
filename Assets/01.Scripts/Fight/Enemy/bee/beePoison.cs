using UnityEngine;

public class beePoison : EnemyState
{
    [SerializeField]
    private int AttackStateIndex = 1;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {

        delayedAction(2f, () =>
        {
            BattleManager.Instance.ApplyDamage(
                BattleManager.Instance.Player, 2);
            // 중독 3스택 부여
            BattleManager.Instance.ApplyEffect(
            BattleManager.Instance.Player,
            StatusAbnormalityNumber.poison,
            3,
            0 //HoldingTime은 poison에서 사용하지 않음
        );

            base.Action();
            RequestStateChange(AttackStateIndex);
            
        });
        
    }


    public override void Exit()
    {
        base.Exit();
    }
}
