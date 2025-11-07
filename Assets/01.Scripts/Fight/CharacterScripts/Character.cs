using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum StatusAbnormalityNumber
    {
        bleeding
    }

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private TurnManager.TurnOwner _identity = TurnManager.TurnOwner.Player;

    public Stats Stats
    {
        get => _stats;
    }

    [Header("상태이상 리스트")]
    [SerializeField]
    private List<StatusAbnormality> _statusAbnormalitys = new List<StatusAbnormality>();

    [Header("상태 표시줄")]
    [SerializeField]
    private StatusBar _statusBar;

    protected virtual void Awake()
    {
        TurnManager.Instance.OnTurnChanged += ResetShield;

        if (_statusAbnormalitys == null)
            _statusAbnormalitys = new List<StatusAbnormality>();

        InitStatusAbnormality();
    }
    private void InitStatusAbnormality()
    {
        _statusAbnormalitys.Add(new StatusAbnormality("출혈", 0, 0));
    }
    public void TakeDamage(int damage)
    {
        if (_stats.Shield >= damage)
        {
            _stats.Shield -= damage;
        }
        else
        {
            damage -= _stats.Shield;
            _stats.CurrentHP -= damage;
        }

        if (_stats.CurrentHP <= 0)
        {
            _stats.CurrentHP = 0;
            Die();
        }
    }

    public void ApplyShield(int shieldPoint)
    {
        _stats.Shield += shieldPoint;
    }
    public void ResetShield(TurnManager.TurnOwner turnOwner)
    {
        if (_identity == turnOwner)
            Stats.Shield = 0;
    }

    public void Heal(int healPoint)
    {
        _stats.CurrentHP += healPoint;
        if (_stats.CurrentHP > _stats.MaxHP)
        {
            _stats.CurrentHP = _stats.MaxHP;
        }
    }
    public void ApplyBleeding(int amount, int HoldingTime)
    {
        if (_statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding].Amount > 0)
        {
            _statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding].Amount = (_statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding].Amount + amount) / 2;
        }
        else
        {
            _statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding].Amount += amount;
        }
        _statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding].HoldingTime += HoldingTime;
    }

    public void EffectBleeding()
    {
        var bleed = _statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding];

        // 출혈이 없으면 바로 종료
        if (bleed.Amount <= 0 || bleed.HoldingTime <= 0)
            return;

        // 피해 적용
        _stats.CurrentHP -= bleed.Amount;
        bleed.HoldingTime--;

        // 지속이 끝났으면 상태 초기화
        if (bleed.HoldingTime <= 0)
        {
            bleed.Amount = 0;
            bleed.HoldingTime = 0;
            // TODO: 효과 GUI 끄기
        }

        _statusAbnormalitys[(int)StatusAbnormalityNumber.bleeding] = bleed;
    }

    private void Die()
    {
        Debug.Log($"{name} is dead");
        // 사망 처리 로직
    }
}
