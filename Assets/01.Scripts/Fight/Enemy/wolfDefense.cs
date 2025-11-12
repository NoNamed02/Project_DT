using UnityEngine;

public class wolfDefense : EnemyState
{
    [SerializeField]
    private int AttackStateIndex = 0;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Action()
    {
        // 자신에게 방어막 5 추가
        BattleManager.Instance.ApplyShield(transform.parent.GetComponent<Character>(), 5);
        base.Action();
        // 방어 행동 후 바로 공격 상태로 전환
        RequestStateChange(AttackStateIndex);
    }

    public override void Exit()
    {
        base.Exit();
    }
}


