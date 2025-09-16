using System.Collections;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private Enemy _enemy;
    public event System.Action<int> OnRequestChange;
    public event System.Action OnActionDone;
    protected void ActionDone() => OnActionDone?.Invoke();

    [SerializeField]
    protected int _nextStateIndex = 0;

    void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }
    protected void RequestChange(int stateIndex)
    {
        OnRequestChange?.Invoke(stateIndex);
    }

    /// <summary>
    /// 상태 진입시 수행되는 함수
    /// </summary>
    public virtual void Enter()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 적이 자신의 턴에 할 행동
    /// </summary>
    public virtual void Action()
    {
        RequestChange(_nextStateIndex);
        ActionDone();
    }

    /// <summary>
    /// 상태 종료시 수행되는 함수
    /// </summary>
    public virtual void Exit()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 상태 변환 체크용 if조건을 넣어서 조건에 맞을 시 _wantChangeState로 이동시킴
    /// </summary>
    public virtual void CheckStateChange()
    {

    }
}
