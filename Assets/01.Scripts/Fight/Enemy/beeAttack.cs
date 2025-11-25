
using UnityEngine;

public class beeAttack : EnemyState
{
    [SerializeField]
    private int PoisonStateIndex = 1;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 자신의 현재 체력의 10%만큼 데미지 계산
        int damage = Mathf.CeilToInt(GetCurrentHP() * 0.1f);
        

        base.Action();
        delayedAction(3f, () => {

            BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, damage);

            delayedAction(2f, () =>
            {
                CheckStateChange();
            });
        });
    }

    public override void CheckStateChange()
    {
        // 현재 체력이 최대 체력의 절반 이하이면 독 공격 상태로 전환
        if (GetCurrentHP() <= GetMaxHP() / 2)
        {
            RequestStateChange(PoisonStateIndex);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

