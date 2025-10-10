using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardDatabase : MonoSingleton<CardDatabase>
{
    [SerializeField]
    private CardSpec[] cardSpecs;

    [SerializeField]
    private CardEffect[] cardDamageEffects;
    private Dictionary<int, CardSpec> _map;

    protected override void Awake()
    {
        base.Awake();

        _map = new Dictionary<int, CardSpec>();
        // foreach (var spec in cardSpecs)
        // {
        //     _map[spec.id] = spec;
        // }
        foreach (var spec in cardSpecs)
        {
            if (spec == null)
            {
                Debug.LogWarning("CardSpec에 null이 있음");
                continue;
            }
            _map[spec.id] = spec;
        }
    }
    // void Start()
    // {
    //     _map = new Dictionary<int, CardSpec>();
    //     foreach (var spec in cardSpecs)
    //     {
    //         _map[spec.id] = spec;
    //     }
    // }

    public CardSpec Get(int id) => _map.TryGetValue(id, out var spec) ? spec : null;

    /// <summary>
    /// 카드 생성시 effect list를 카드에 넣기 위한 함수
    /// </summary>
    /// <param name="effectName"></param>
    /// <param name="amount"></param>
    /// <param name="holdingTime"></param>
    /// <returns></returns>
    public CardEffect GetCardEffect(string effectName, int amount, int holdingTime)
    {
        if (effectName == "Damage")
        {
            return cardDamageEffects[amount];
        }
        return null;
    }
}
