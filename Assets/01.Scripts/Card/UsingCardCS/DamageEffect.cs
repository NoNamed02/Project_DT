using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/DamageEffect")]
public class DamageEffect : CardEffect
{
    public override void Execute(Character target, Card card, int effectAmount, int effectHoldingTime)
    {
        BattleManager.Instance.ApplyDamage(target, effectAmount);
    }
}
