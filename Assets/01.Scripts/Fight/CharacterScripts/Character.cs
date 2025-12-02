using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private TurnManager.TurnOwner _identity = TurnManager.TurnOwner.Player;

    public Stats Stats
    {
        get => _stats;
    }
    protected int HPForSet
    {
        set => _stats.CurrentHP = value;
    }

    [Header("상태이상 리스트")]
    [SerializeField]
    private List<StatusAbnormality> _statusAbnormalitys = new List<StatusAbnormality>();
    public List<StatusAbnormality> StatusAbnormalitys => _statusAbnormalitys;

    [Header("상태 표시줄")]
    [SerializeField]
    private StatusBar _statusBar;

    [Header("(회피 상태 / 확인용)")]
    [SerializeField]
    private bool _dodge = false;

    protected virtual void Awake()
    {
        TurnManager.Instance.OnTurnChanged += ResetShield;

        if (_statusAbnormalitys == null)
            _statusAbnormalitys = new List<StatusAbnormality>();
    }
    public void TakeDamage(int damage)
    {
        if (_dodge)
        {
            _dodge = false;
            Debug.Log("회피함");
            return;
        }
        if (_stats.Shield >= damage)
        {
            _stats.Shield -= damage;
        }
        else
        {
            damage -= _stats.Shield;
            _stats.Shield = 0;
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
    public void ApplyEffect(StatusAbnormalityNumber effectID, int amount, int holdingTime)
    {
        if (effectID == StatusAbnormalityNumber.poison)
        {
            int currentPoisonStack = GetTotalPoisonStack();

            if(currentPoisonStack + amount > 8)
                amount = 8 - currentPoisonStack;
        }

        var effect = new StatusAbnormality(effectID, amount, holdingTime);
        _statusAbnormalitys.Add(effect);
        if (effectID == StatusAbnormalityNumber.dodge)
        {
            if (CheckDodge())
            {
                EffectDodge();
            }
        }
    }

    /// <summary>
    /// 현재 중독 스택 합계 반환
    /// </summary>
    /// <returns></returns>
    private int GetTotalPoisonStack()
    {
        int totalStack = 0;
        foreach (var effect in _statusAbnormalitys)
        {
            if (effect.EffectID == StatusAbnormalityNumber.poison)
            {
                totalStack += effect.Amount;
            }
        }
        return totalStack;
    }

    private bool CheckDodge()
    {
        int amount = 0;
        foreach (var effect in _statusAbnormalitys)
        {
            if (effect.EffectID == StatusAbnormalityNumber.dodge)
                amount += effect.Amount;
        }
        return amount >= 10;
    }
    private void EffectDodge()
    {
        Debug.Log("회피 상태");
        for (int i = _statusAbnormalitys.Count - 1; i >= 0; i--)
        {
            if (_statusAbnormalitys[i].EffectID == StatusAbnormalityNumber.dodge)
            {
                _statusAbnormalitys.RemoveAt(i);
            }
        }

        _dodge = true;
    }

    public void EffectBleeding()
    {
        int bleedingAmount = 0;
        for (int i = _statusAbnormalitys.Count - 1; i >= 0; i--)
        {
            if (_statusAbnormalitys[i].EffectID == StatusAbnormalityNumber.bleeding)
            {
                var effect = _statusAbnormalitys[i];

                bleedingAmount += effect.Amount;
                effect.HoldingTime--;

                if (effect.HoldingTime <= 0)
                {
                    _statusAbnormalitys.RemoveAt(i);
                    continue;
                }

                _statusAbnormalitys[i] = effect;
            }
        }
        _stats.CurrentHP -= bleedingAmount;

        if (CheckDie())
        {
            Die();
        }
    }

    public void EffectPoison()
    {
        int totalPoisonStack = 0;

        for (int i = _statusAbnormalitys.Count - 1; i >= 0; i--)
        {
            if (_statusAbnormalitys[i].EffectID == StatusAbnormalityNumber.poison)
            {
                totalPoisonStack += _statusAbnormalitys[i].Amount;
            }
        }

        if (totalPoisonStack <= 0) return;

        int poisonDamage = totalPoisonStack * 2;
        _stats.CurrentHP -= poisonDamage;
        Debug.Log($"{name}이(가) 중독으로 {poisonDamage} 대미지를 받음 (스택: {totalPoisonStack})");

        ReducePoisonStack(1);

        if (CheckDie()) Die();
    }

    public void ReducePoisonStack(int reduceAmount)
    {
        int remaining = reduceAmount;

        for (int i = _statusAbnormalitys.Count - 1; i >= 0; i--)
        {
            if (_statusAbnormalitys[i].EffectID == StatusAbnormalityNumber.poison)
            {
                var effect = _statusAbnormalitys[i];

                if(effect.Amount <= remaining)
                {
                    remaining -= effect.Amount;
                    _statusAbnormalitys.RemoveAt(i);
                }
                else
                {
                    effect.Amount -= remaining;
                    _statusAbnormalitys[i] = effect;
                    remaining = 0;
                    break;
                }

                if(remaining <= 0) break;
            }
        }
    }

    /// <summary>
    /// 현재 공격력 감소 효과의 총 감소량 반환
    /// Amount가 감소량, HoldingTime 지속 턴
    /// 여러 개가 있을 시 최대값만 적용 (중첨X)
    /// </summary>
    /// <returns></returns>
    public int GetWeakenAmount()
    {
        int maxWeaken = 0;
        foreach (var effect in _statusAbnormalitys)
        {
            if(effect.Amount > maxWeaken)
                maxWeaken = effect.Amount;
        }
        return maxWeaken;
    }

    /// <summary>
    /// 공격력 감소가 적용된 실제 데미지 계산
    /// </summary>
    /// <param name="baseDamage"></param>
    /// <returns></returns>
    public int CalculateWeakenedDamage(int baseDamage)
    {
        int weakenAmount = GetWeakenAmount();

        if (weakenAmount <= 0) return baseDamage;

        int finalDamage = Mathf.Max(0, baseDamage - weakenAmount);

        return finalDamage;
    }

    /// <summary>
    /// 공격력 감소 효과 업데이트, 턴 종료 시 지속 시간 감소
    /// </summary>
    public void UpdateWeaken()
    {
        for (int i = _statusAbnormalitys.Count - 1; i >= 0; i--)
        {
            if (_statusAbnormalitys[i].EffectID == StatusAbnormalityNumber.weaken)
            {
                var effect = _statusAbnormalitys[i];
                effect.HoldingTime--;

                if (effect.HoldingTime <= 0)
                    _statusAbnormalitys.RemoveAt(i);
                else _statusAbnormalitys[i] = effect;
            }
        }
    }

    private bool CheckDie()
    {
        return _stats.CurrentHP <= 0;
    }
    private void Die()
    {
        Debug.Log($"{name} is dead");
        if (this is Enemy)
        {
            BattleManager.Instance.GetEnemys().Remove(this as Enemy);
        }
        gameObject.SetActive(false);
        _statusBar.gameObject.SetActive(false);
        // Destroy(_statusBar);
        // Destroy(gameObject);
        // 사망 처리 로직
    }
}
