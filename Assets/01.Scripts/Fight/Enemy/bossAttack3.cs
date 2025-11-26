using UnityEngine;

public class bossAttack3 : EnemyState
{
    [SerializeField]
    private int BossAttackDown = 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        base.Action();
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyShield(transform.parent.GetComponent<Character>(), 10);

            delayedAction(2f, () =>
            {
                RequestStateChange(BossAttackDown);
            });
        });

    }
}