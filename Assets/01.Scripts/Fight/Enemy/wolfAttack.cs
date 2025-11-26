using UnityEngine;

public class wolfAttack : EnemyState
{
    [SerializeField]
    private int currentHP = 100;

    [SerializeField]
    private int DefenseStateIndex = 1;

    [SerializeField]
    private float probability = 0.1f;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        base.Action();
        delayedAction(3f, () => {
            // 6ÀÇ ÇÇÇØ·Î °ø°Ý
            BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, 6);
            // 10% È®·ü·Î ÃôÇ÷
            if (Random.value < probability) {
                BattleManager.Instance.ApplyBleeding(BattleManager.Instance.Player, 5, 2);
            }

            delayedAction(2f, () =>
            {
                CheckStateChange();
            });
        });
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
