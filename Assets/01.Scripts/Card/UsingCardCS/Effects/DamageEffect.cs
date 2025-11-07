public class DamageEffect : CardEffect
{
    public void Execute(Character target, int effectAmount, int effectHoldingTime)
    {
        BattleManager.Instance.ApplyDamage(target, effectAmount);
    }
}
