using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected int _hp = 10;

    public int GetHP()
    {
        return _hp;
    }

    public void SetHP(int HP)
    {
        _hp = HP;
    }

    public void TakeDamage(int amount)
    {
        _hp -= amount;
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
