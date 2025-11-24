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

    private bool CheckDie()
    {
        return _stats.CurrentHP <= 0;
    }
    private void Die()
    {
        Debug.Log($"{name} is dead");
        // 사망 처리 로직
    }
}
