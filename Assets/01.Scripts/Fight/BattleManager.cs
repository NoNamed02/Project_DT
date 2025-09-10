using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField]
    private Character _player;
    [SerializeField]
    private List<Character> _enemys = new List<Character>();
    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _enemys = FindObjectsByType<Character>(FindObjectsSortMode.None).ToList();
    }

    public void ApplyDamage(Character target, int damage)
    {
        // 필요하면 여기서 방어력, 크리티컬 계산
        int finalDamage = damage;
        target.TakeDamage(finalDamage);
    }

    public Character GetPlayer()
    {
        return _player;
    }
    public List<Character> GetEnemys()
    {
        return _enemys;
    }
}
