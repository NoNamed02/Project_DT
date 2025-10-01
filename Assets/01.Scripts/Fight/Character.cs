using UnityEngine;

public class Character : MonoBehaviour
{
    protected int _hp = 10;

    public int HP
    {
        get => _hp;
        set => _hp = value;
    }


    [SerializeField]
    protected int _shield = 0;

    public void TakeDamage(int amount)
    {
        if (_shield >= amount)
        {
            _shield -= amount;
        }
        else
        {
            amount -= _shield;
            _hp -= amount;
        }

        if (_hp <= 0)
        {
            _hp = 0;
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} is dead");
        // 사망 처리 로직
    }
}
