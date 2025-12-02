using UnityEngine;

public class bossAttack4 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 4;
    [SerializeField] private int bossHealIndex = 5;

    [SerializeField] private int currentHP = 0;

    [Header("공격 설정")]
    [SerializeField] private int _baseDamage = 3;

    [Header("출혈 설정")]
    [SerializeField] private int _baseBleedingDamage = 4;
    [SerializeField] private int _bleedingDuration = 2;

    [Header("페이즈 설정")]
    [SerializeField] private float _phase2Multiplier = 1.5f;

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetDamage()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseDamage * _phase2Multiplier);
        return _baseDamage;
    }

    private int GetBleedingDamage()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseBleedingDamage * _phase2Multiplier);
        return _baseBleedingDamage;
    }

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        int damage = GetDamage();
        int bleedingDamage = GetBleedingDamage();

        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.bleeding,
                bleedingDamage,
                _bleedingDuration
            );

            delayedAction(0.5f, () =>
            {
                BattleManager.Instance.ApplyDamage(
                Enemy, BattleManager.Instance.Player,
                damage
                );

                base.Action();
                RequestStateChange(nextAttackIndex);
            });
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
        int damage = GetDamage();
        int bleedingDamage = GetBleedingDamage();
        string desc = $"{damage}의 피해를 입히고 {bleedingDamage}의 출혈을 {_bleedingDuration}턴 동안 부여합니다.";
        return new IntentData(IntentType.Attack, damage, desc);
    }
}