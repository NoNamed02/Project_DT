using UnityEngine;

public class plantsHeal : EnemyState
{
    [SerializeField]
    private int AttackStateIndex = 0;

    // 회복 쿨타임
    [SerializeField]
    private int healCooldown = 2;

    // 현재 대기한 턴 수
    [SerializeField]
    private int currentCooldown = 0;

    public override void Enter()
    {
        base.Enter();
        currentCooldown = 0;
    }

    public override void Action()
    {
        // 쿨타임이 다 차면 회복 후 공격 상태로 전환
        // 일단 자가회복
        if (currentCooldown >= healCooldown)
        {
            BattleManager.Instance.HealTarget(transform.parent.GetComponent<Character>(), 2);
            currentCooldown = 0;
            RequestStateChange(AttackStateIndex);
        }
        currentCooldown++;
        base.Action();
    }

    public override void Exit()
    {
        currentCooldown = 0;
        base.Exit();
    }
}
