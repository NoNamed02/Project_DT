using UnityEngine;

[System.Serializable]
public class StatusAbnormality
{
    [Header("상태이상 이름")]
    public string Name;
    [Header("가중치")]
    public int Amount;
    [Header("지속 턴")]
    public int HoldingTime;
    
    public StatusAbnormality(string Name, int Amount, int HoldingTime)
    {
        this.Name = Name;
        this.Amount = Amount;
        this.HoldingTime = HoldingTime;
    }
}
