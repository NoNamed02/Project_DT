using UnityEngine;

public class bossAttack4 : EnemyState
{
    // 출혈, 공격.

    [SerializeField]
    private int BossDefensePoison = 3;

    public int attackDamage = 8;
    public int bleedingDamage = 5;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 5만큼 2턴 출혈
        BattleManager.Instance.ApplyBleeding(BattleManager.Instance.Player, bleedingDamage, 2);
        // 8 공격
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, attackDamage);
        base.Action();
        RequestStateChange(BossDefensePoison);
    }
}