using System;
using System.Collections;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private Enemy _enemy;

    [SerializeField]
    protected int _nextStateIndex = 0;

    protected Action<int> RequestChange;   // 전이 요청
    protected Action ReportDone;           // 액션 종료 보고

    void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
    }
    // 주입 지점
    public void BindCallbacks(Action<int> requestChange, Action reportDone)
    {
        RequestChange = requestChange;
        ReportDone = reportDone;
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
        ReportDone?.Invoke();
        RequestStateChange(_nextStateIndex);
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

    /// <summary>
    /// enemy객체에게 state 변환을 요청함
    /// </summary>
    /// <param name="stateIndex"></param>
    // 상태 내부에서 필요 시 호출
    protected void RequestStateChange(int idx) => RequestChange?.Invoke(idx);
}
