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

    void Awake()
    {
        _stats = new Stats();
        TurnManager.Instance.OnTurnChanged += ResetShield;
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

    private void Die()
    {
        Debug.Log($"{name} is dead");
        // 사망 처리 로직
    }
}
