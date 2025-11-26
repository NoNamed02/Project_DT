using UnityEngine;

public class bossAttack2 : EnemyState
{
    // 강타 / 맹렬한 일격
    // HP가 반 이하일 때 대미지 1.5배 상승

    [SerializeField]
    private int BossBleeding = 2;

    public int SlugDamage = 15;
    public bool HPHalfLess = false;
    public float damageUP = 1.5f;

    public override void Enter()
    {
        base.Enter();
        // if 보스의 체력이 반 이하, HPHalfLess = true
        if (GetCurrentHP() <= GetMaxHP() / 2)
            HPHalfLess = true;
        else HPHalfLess = false;
    }

    public override void Action()
    {
        base.Action();
        delayedAction(3f, () =>
        {
            // if 보스의 체력이 반 이하, 이상
            if (HPHalfLess)
                BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, (int)(SlugDamage * damageUP));
            else BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, SlugDamage);

            delayedAction(2f, () =>
            {
                RequestStateChange(BossBleeding);
            });
        });
    }
}


