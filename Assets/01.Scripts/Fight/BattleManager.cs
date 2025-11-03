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
        Debug.Log($"       {target}에게 {damage}만큼의 데미지를 줌");
    }

    public void ApplyShield(Character target, int shieldPoint)
    {
        target.ApplyShield(shieldPoint);
    }
    
    public void ResetShield(Character target)
    {
        target.Stats.Shield = 0;
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
