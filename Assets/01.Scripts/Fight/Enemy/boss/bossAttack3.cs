using UnityEngine;

public class bossAttack3 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 3;
    [SerializeField] private int bossHealIndex = 5;

    [SerializeField] private int currentHP = 0;

    [Header("방어 설정")]
    [SerializeField] private int _baseShieldAmount = 8;

    [Header("페이즈 설정")]
    [SerializeField] private float _phase2Multiplier = 1.5f;

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetShieldAmount()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseShieldAmount * _phase2Multiplier);
        return _baseShieldAmount;
    }

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        int shieldAmount = GetShieldAmount();

        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyShield(Enemy, shieldAmount);

            base.Action();
            RequestStateChange(nextAttackIndex);
        });
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        if (currentHP - GetCurrentHP() >= 6)
            RequestStateChange(bossHealIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        int shieldAmount = GetShieldAmount();
        return new IntentData(IntentType.Defend, shieldAmount);
    }
}