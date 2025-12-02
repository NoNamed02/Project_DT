using UnityEngine;

public class wolfAttack2 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 2;
    [SerializeField] private int wolfDefenseIndex = 3;

    [SerializeField] private int currentHP = 0;

    [Header("출혈 설정")]
    [SerializeField] private int _bleedingDamage = 3;
    [SerializeField] private int _bleedingDuration = 2;

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.bleeding,
                _bleedingDamage, _bleedingDuration
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
        string desc = $"{_bleedingDamage}의 출혈을 {_bleedingDuration}턴 동안 부여합니다.";
        return new IntentData(IntentType.Bleeding, _bleedingDamage, desc);
    }
}
