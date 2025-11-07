using UnityEngine;

public class BleedingEffect : CardEffect
{
    public void Execute(Character target, int amount, int holdingTime)
    {
        BattleManager.Instance.ApplyBleeding(target, amount, holdingTime);
    }
}
