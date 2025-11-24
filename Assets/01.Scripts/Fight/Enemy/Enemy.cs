using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{
    private bool _acting = false;
    private int? _pendingNext = null;
    [SerializeField]
    private List<EnemyState> _states = new List<EnemyState>();

    [Header("현재 상태")]
    [SerializeField]
    private EnemyState _currentState = null;

    // 콜백 묶음
    private Action<int> _requestChange;
    private Action _reportDone;
    protected override void Awake()
    {
        base.Awake();
        _states = GetComponentsInChildren<EnemyState>(true).ToList();
        foreach (var state in _states)
        {
            state.gameObject.SetActive(false);
        }
        _requestChange = ChangeState;
        _reportDone = OnActionDone;
        // 모든 상태에 주입
        foreach (var state in _states)
            state.BindCallbacks(_requestChange, _reportDone);
    }
    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;

        _currentState = _states[0];

        _currentState.Enter();
    }

    void Update()
    {
        // 플레이어 턴일때만 체크함
        if (TurnManager.Instance != null &&
            TurnManager.Instance.CurrentOwner == TurnManager.TurnOwner.Player &&
            _currentState != null)
        {
            _currentState.CheckStateChange();
        }
    }
    public void RunStateCoroutine(IEnumerator routine)
    {
        StartCoroutine(routine);
    }

    void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= TurnChanged;
    }
    /// <summary>
    /// 현재 턴이 적의 턴일시 state의 Action()함수를 통해 행동 시행
    /// </summary>
    /// <param name="owner">현재 턴의 주체</param>
    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Enemy)
        {
            if (_acting) return;
            else _acting = true;

            _currentState.Action();
        }
    }

    /// <summary>
    /// 현재 state를 다른 state로 변경, 무조건 state의 action()이 끝난 후 호출하도록 설계할 것
    /// </summary>
    /// <param name="state">바꾸고 싶은 상태</param>
    public void ChangeState(int stateIndex)
    {
        if (stateIndex < 0 || stateIndex >= _states.Count) { Debug.LogError($"idx {stateIndex}"); return; }
        if (_currentState == _states[stateIndex]) return;

        // 행동 중에는 보류만
        if (_acting) { _pendingNext = stateIndex; return; }

        ApplyState(stateIndex);
    }
    private void ApplyState(int stateIndex)
    {
        var next = _states[stateIndex];
        if (_currentState == next) return;

        _currentState.Exit();
        _currentState = next;
        _currentState.Enter();
    }

    // state 행동 끝났을시 수행
    public void OnActionDone()
    {
        _acting = false;

        // 여기서만 전환 확정
        if (_pendingNext.HasValue)
        {
            ApplyState(_pendingNext.Value);
            _pendingNext = null;
        }

        StartCoroutine(DelayTurnEnd());
    }

    public IEnumerator DelayTurnEnd()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("enemy turn end");
        TurnManager.Instance.NextTurn();
    }
}