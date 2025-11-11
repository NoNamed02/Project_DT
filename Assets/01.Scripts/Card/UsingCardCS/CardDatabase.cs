using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardDatabase : MonoSingleton<CardDatabase>
{
    public enum EffectNum
    {
        Damage,
        Defence,
        Bleeding
    }
    [SerializeField]
    private CardSpec[] cardSpecs;
    public CardSpec[] CardSpecs { set => value = cardSpecs; }

    [SerializeField]
    private List<CardEffect> Effects = new List<CardEffect>();

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

        Effects.Add(new DamageEffect());
        Effects.Add(new DefenceEffect());
        Effects.Add(new BleedingEffect());
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
    public CardEffect GetCardEffect(string effectName)
    {
        if (effectName == "Damage")
        {
            return Effects[(int)EffectNum.Damage];
        }
        else if (effectName == "Defence")
        {
            return Effects[(int)EffectNum.Defence];
        }
        else if (effectName == "Bleeding")
        {
            return Effects[(int)EffectNum.Bleeding];
        }
        Debug.Log("없는 개념에 대한 것을 가져오려함");
        return null;
    }
}
