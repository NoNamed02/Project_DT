using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/DealDamage")]
public class DealDamageEffect : CardEffect
{
    [SerializeField] private int damageAmount;
    public int DamageAmount
    {
        get => damageAmount;
        set => damageAmount = value;
    }

    public override void Execute(Player source, Character target, Card card)
    {
        BattleManager.Instance.ApplyDamage(target, damageAmount);
    }
}
