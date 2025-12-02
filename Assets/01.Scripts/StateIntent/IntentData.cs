using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IntentData
{
    public IntentType Type;
    public int Value;
    public string Description;

    public IntentData(IntentType type, int value=0, string description ="")
    {
        Type = type;
        Value = value;
        Description = string.IsNullOrEmpty(description)
            ? GetDefaultDescription(type, value)
            : description;
    }

    //기본 설명
    private string GetDefaultDescription(IntentType type, int value)
    {
        return type switch
        {
            IntentType.Attack => $"{value}의 피해를 입힙니다.",
            IntentType.Defend => $"{value}의 방어를 얻습니다.",
            IntentType.Heal => $"{value}의 체력을 회복합니다.",
            IntentType.Bleeding => $"{value}의 출혈을 부여합니다.",
            IntentType.Poison => $"중독 {value}스택을 부여합니다.",
            IntentType.Dodge => $"다음 공격을 회피합니다.",
            IntentType.Weaken => $"공격력을 {value}만큼 감소시킵니다.",
            IntentType.Unknown => $"행동을 예측할 수 없습니다.",
            _ => ""
        };
    }

}

[System.Serializable]
public class IntentDataSet
{
    public List<IntentData> Intents = new List<IntentData>();

    public IntentDataSet()
    {
        Intents = new List<IntentData>();
    }

    public IntentDataSet(IntentData singleIntent)
    {
        Intents = new List<IntentData> { singleIntent };
    }

    public IntentDataSet(params IntentData[] intents)
    {
        Intents = new List<IntentData>(intents);
    }

    /// <summary>
    /// Intent 추가
    /// </summary>
    /// <param name="intent"></param>
    public void Add(IntentData intent)
    {
        Intents.Add(intent);
    }

    /// <summary>
    /// Intent 개수
    /// </summary>
    public int Count => Intents.Count;

    /// <summary>
    /// 첫 번재 Intent (단일 Intent 호환용)
    /// </summary>
    public IntentData Primary => 
        Intents.Count > 0 ? Intents[0] : new IntentData(IntentType.Unknown);
}