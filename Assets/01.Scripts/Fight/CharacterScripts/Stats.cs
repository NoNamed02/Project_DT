using UnityEngine;

[System.Serializable]
public class Stats
{
    [Header("최대 체력")]
    [SerializeField]
    protected int _maxHP = 10;
    
    [Header("현재 체력")]
    [SerializeField]
    protected int _hp = 10;

    [Header("방어도")]
    [SerializeField]
    protected int _shield = 0;

    public int Shield
    {
        get => _shield;
        set => _shield = value;
    }

    public int CurrentHP
    {
        get => _hp;
        set => _hp = value;
    }
    public int MaxHP
    {
        get => _maxHP;
    }
}
