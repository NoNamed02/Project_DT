using System.Collections;
using UnityEngine;

public class DefultAttack : EnemyState
{
    public override void Enter()
    {
        base.Enter();
    }
    public override void Action()
    {
        Enemy.RunStateCoroutine(Attack());
    }
    public override void Exit()
    {
        base.Exit();
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(3f);
        BattleManager.Instance.ApplyDamage(BattleManager.Instance.Player, 5);
        Enemy.EffectBleeding();
        yield return new WaitForSeconds(1f);
        base.Action();
    }
}
