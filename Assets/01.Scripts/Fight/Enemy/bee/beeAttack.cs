
using UnityEngine;

public class beeAttack : EnemyState
{
    [SerializeField]
    private int PoisonStateIndex = 2;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 자신의 현재 체력의 20%만큼 데미지 계산
        int damage = Mathf.CeilToInt(GetCurrentHP() * 0.2f);

        delayedAction(2f, () => {

            BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, damage);
            base.Action();
            RequestStateChange(PoisonStateIndex);
        });
    }


    public override void Exit()
    {
        base.Exit();
    }
}

