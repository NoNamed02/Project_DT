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
    public StatusAbnormalityNumber EffectID = new StatusAbnormalityNumber();
    public StatusAbnormality(StatusAbnormalityNumber EffectID, int Amount, int HoldingTime)
    {
        if (EffectID == StatusAbnormalityNumber.bleeding)
            this.Name = "출혈";
        else if (EffectID == StatusAbnormalityNumber.dodge)
            this.Name = "회피";
        this.EffectID = EffectID;
        this.Amount = Amount;
        this.HoldingTime = HoldingTime;
    }
}
