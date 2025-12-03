using UnityEngine;

public class plantAttack1 : EnemyState
{
    [SerializeField] private int nextAttackIndex = 1;
    [SerializeField] private int plantsHealIndex = 3;
    
    [Header("공격 설정")]
    [SerializeField] private int _damage = 8;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        
        delayedAction(3f, () => { 
            BattleManager.Instance.ApplyDamage(Enemy, BattleManager.Instance.Player, _damage);

            base.Action();
            RequestStateChange(nextAttackIndex);
        });
        
    }

    public override void CheckStateChange()
    {
        base.CheckStateChange();
        // 체력이 반 이하일 때 전환
        if (GetCurrentHP() < GetMaxHP() / 5)
            RequestStateChange(plantsHealIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override IntentData GetIntent()
    {
        return new IntentData(IntentType.Attack, _damage);
    }
}

