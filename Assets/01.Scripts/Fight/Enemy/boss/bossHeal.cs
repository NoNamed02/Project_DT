using UnityEngine;

public class bossHeal : EnemyState
{
    [SerializeField] private int nextAttackIndex = 0;

    [Header("회복 설정")]
    [SerializeField] private int _baseHealAmount = 8;

    [Header("페이즈 설정")]
    [SerializeField] private float _phase2Multiplier = 1.5f;

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetHealAmount()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseHealAmount * _phase2Multiplier);
        return _baseHealAmount;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        int healAmount = GetHealAmount();

        delayedAction(3f, () =>
        {
            BattleManager.Instance.HealTarget(
                Enemy,
                healAmount
            );

            base.Action();
            RequestStateChange(nextAttackIndex);
        });
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        int healAmount = GetHealAmount();
        return new IntentData(IntentType.Heal, healAmount);
    }
}
