using UnityEngine;

public class wolfAttack3 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 0;
    [SerializeField] private int wolfDefenseIndex = 3;

    [SerializeField] private int currentHP = 0;

    [SerializeField] private int _damage = 10;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyDamage(
                Enemy,
                BattleManager.Instance.Player,
                _damage
            );

            base.Action();
            RequestStateChange(nextAttackIndex);
        });
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        if (GetCurrentHP() <= currentHP - 6)
            RequestStateChange(wolfDefenseIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        return new IntentData(IntentType.Attack, _damage);
    }
}
