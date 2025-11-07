using System;

public class DefenceEffect : CardEffect
{
    public void Execute(Character target, int effectAmount, int effectHoldingTime)
    {
        BattleManager.Instance.ApplyShield(target, effectAmount);
    }
}
