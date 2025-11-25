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
        base.Action();
        delayedAction(3f, () => {
            // 자신에게 방어막 7 추가
            BattleManager.Instance.ApplyShield(transform.parent.GetComponent<Character>(), 7);

            delayedAction(2f, () =>
            {
                CheckStateChange();
                RequestStateChange(AttackStateIndex);
            });
        });
    }

    public override void Exit()
    {
        base.Exit();
    }
}


