using UnityEngine;

public class DodgeEffect : CardEffect
{
    public void Execute(Character target, int amount, int holdingTime)
    {
        BattleManager.Instance.ApplyEffect(target, StatusAbnormalityNumber.dodge, amount, holdingTime);
    }
}
