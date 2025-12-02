using System.Net.Cache;
using UnityEngine;

public class bossAttack2 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 2;
    [SerializeField] private int bossHealIndex = 5;

    [SerializeField] private int currentHP = 0;

    [Header("약화 설정")]
    [SerializeField] private int _baseWeakenAmount = 2;
    [SerializeField] private int _weakenDuration = 2;

    [Header("페이즈 설정")]
    [SerializeField] private float _phase2Multiplier = 1.5f;

    private bool IsPhase2 => GetCurrentHP() <= GetMaxHP() * 0.5f;

    private int GetWeakenAmount()
    {
        if (IsPhase2)
            return Mathf.CeilToInt(_baseWeakenAmount * _phase2Multiplier);
        return _baseWeakenAmount;
    }

    public override void Enter()
    {
        base.Enter();
        currentHP = GetCurrentHP();
    }

    public override void Action()
    {
        int weakenAmount = GetWeakenAmount();
        delayedAction(3f, () =>
        {
            BattleManager.Instance.ApplyEffect(
                BattleManager.Instance.Player,
                StatusAbnormalityNumber.weaken,
                weakenAmount, _weakenDuration);

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
        int weakenAmount = GetWeakenAmount();
        string desc = $"공격력을 {weakenAmount}만큼 {_weakenDuration}턴 동안 감소시킵니다.";
        return new IntentData(IntentType.Weaken, weakenAmount, desc);
    }
}


