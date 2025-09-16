using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character
{
    private bool _acting = false;
    private int? _pendingNext = null;
    [SerializeField]
    private List<EnemyState> _states = new List<EnemyState>();

    [Header("현재 상태")]
    [SerializeField]
    private EnemyState _currentState = null;
    void Awake()
    {
        _states = GetComponentsInChildren<EnemyState>(true).ToList();
        foreach (var state in _states)
        {
            state.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;

        _currentState = _states[0];
        _currentState.OnRequestChange += ChangeState; // 초기 상태 구독
        _currentState.OnActionDone += OnActionDone;

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

    void OnDestroy()
    {
        TurnManager.Instance.OnTurnChanged -= TurnChanged;
        if (_currentState != null)
        {
            _currentState.OnRequestChange -= ChangeState;
            _currentState.OnActionDone -= OnActionDone;
        }
    }
    /// <summary>
    /// 현재 턴이 적의 턴일시 state의 Action()함수를 통해 행동 시행
    /// </summary>
    /// <param name="owner">현재 턴의 주체</param>
    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Enemy)
        {
            Debug.Log("Enemy's Turn Start");
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

        _currentState.OnRequestChange -= ChangeState;
        _currentState.OnActionDone    -= OnActionDone;
        _currentState.Exit();

        _currentState = next;
        _currentState.OnRequestChange += ChangeState;
        _currentState.OnActionDone    += OnActionDone;
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