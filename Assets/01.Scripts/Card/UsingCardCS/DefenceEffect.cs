using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/DefenceEffect")]
public class DefenceEffect : CardEffect
{
    public override void Execute(Character target, Card card, int effectAmount, int effectHoldingTime)
    {
        BattleManager.Instance.ApplyShield(target, effectAmount);
    }
}
