using UnityEngine;

public class bossAttack5 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 0;
    [SerializeField] private int bossHealIndex = 5;

    [SerializeField] private int currentHP = 0;

    [Header("중독 설정")]
    [SerializeField] private int _basePoisonStack = 3;

    [Header("페이즈 설정")]
    [SerializeField] private float _phase2Multiplier = 1.5f;

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetPoisonStack()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_basePoisonStack * _phase2Multiplier);
        return _basePoisonStack;
    }

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        int poisonStack = GetPoisonStack();

        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.poison,
                poisonStack,
                0
            );

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
        int poisonStack = GetPoisonStack();
        string desc = $"중독 {poisonStack}스택을 부여합니다.";
        return new IntentData(IntentType.Poison, poisonStack, desc);
    }
}
