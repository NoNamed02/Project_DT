using UnityEngine;

public class bossAttack1 : EnemyState
{
    // 상태이상, 2텀동안 플레이어 공격력 소량 감소. 디버프.
    [SerializeField]
    private int BossSlug = 1;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 공격 감소 디버프 상태이상 걸기 Player()
        // 고민 중...
        base.Action();
        RequestStateChange(BossSlug);
    }
}