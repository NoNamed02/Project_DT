using UnityEngine;

public class bossAttack1 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 1;
    [SerializeField] private int bossHealIndex = 5;

    [SerializeField] private int currentHP = 0;

    [Header("공격 설정")]
    [SerializeField] private int _baseDamage = 15;

    [Header("페이즈 설정")]
    [SerializeField] private float _Phase2Multiplier = 1.5f;
    

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetDamage()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseDamage * _Phase2Multiplier);
        return _baseDamage;
    }

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        int damage = GetDamage();

        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyDamage(
                Enemy,
                BattleManager.Instance.Player,
                damage
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
        int damage = GetDamage();
        return new IntentData(IntentType.Attack, damage);
    }
}