using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField]
    private Character _player;

    public Character Player => _player;
    [SerializeField]
    private List<Enemy> _enemys = new List<Enemy>();
    void Start()
    {
        _player = FindAnyObjectByType<Player>();
        _enemys = FindObjectsByType<Enemy>(FindObjectsSortMode.None).ToList();
    }


    /// <summary>
    /// target에게 damage값 만큼 피해
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public void ApplyDamage(Character target, int damage)
    {
        // 필요하5면 여기서 방어력, 크리티컬 계산
        int finalDamage = damage;
        target.TakeDamage(finalDamage);
        Debug.Log($"------{target}에게 {damage}만큼의 데미지를 줌");
    }

    /// <summary>
    /// target 방어도 += shieldPoint연산
    /// </summary>
    /// <param name="target"></param>
    /// <param name="shieldPoint"></param>
    public void ApplyShield(Character target, int shieldPoint)
    {
        target.ApplyShield(shieldPoint);
    }

    /// <summary>
    /// target 방어도 0으로 처리
    /// </summary>
    /// <param name="target"></param>
    public void ResetShield(Character target)
    {
        target.Stats.Shield = 0;
    }

    /// <summary>
    /// target 체력 회복
    /// </summary>
    /// <param name="target"></param>
    /// <param name="amount"></param>
    public void HealTarget(Character target, int amount)
    {
        target.Heal(amount);
    }

    /// <summary>
    /// target에게 출혈 효과 부여
    /// </summary>
    /// <returns></returns>
    public void ApplyBleeding(Character target, int amount, int holdingTime)
    {
        target.ApplyBleeding(amount, holdingTime);
    }

    /// <summary>
    /// target에게 출혈 처리
    /// </summary>
    /// <returns></returns>
    public void EffectBleeding(Character target)
    {
        target.EffectBleeding();
    }

    public Character GetPlayer()
    {
        return _player;
    }
    public List<Enemy> GetEnemys()
    {
        return _enemys;
    }
}
