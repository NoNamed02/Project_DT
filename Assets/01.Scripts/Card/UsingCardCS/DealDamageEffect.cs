using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/DamageEffect")]
public class DealDamageEffect : CardEffect
{
    [SerializeField] private int damageAmount;
    public int DamageAmount
    {
        get => damageAmount;
        set => damageAmount = value;
    }

    public override void Execute(Character target, Card card)
    {
        BattleManager.Instance.ApplyDamage(target, damageAmount);
    }
}
