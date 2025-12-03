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

    [Header("페이즈 설정")]
    [SerializeField] private bool _isPhase2 = false;

    public bool IsPhase2 => _isPhase2;

    [Header("애니메이션 설정")]
    [SerializeField] private Animator _animator;

    public Animator Animator => _animator;

    // 콜백 묶음
    private Action<int> _requestChange;
    private Action _reportDone;

    public event Action OnIntentChanged;


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

        if (_animator == null)
            _animator = GetComponent<Animator>();

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

        OnIntentChanged?.Invoke();

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

        OnIntentChanged?.Invoke();
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

    /// <summary>
    /// 현재 상태의 단일 Intent 정보 반환 (하위 호환)
    /// </summary>
    public IntentData GetCurrentIntent()
    {
        if (_currentState == null)
            return new IntentData(IntentType.Unknown);

        return _currentState.GetIntent();
    }

    /// <summary>
    /// 현재 상태의 다중 Intent 정보 반환
    /// </summary>
    public IntentDataSet GetCurrentIntentSet()
    {
        if (_currentState == null)
            return new IntentDataSet(new IntentData(IntentType.Unknown));

        return _currentState.GetIntentSet();
    }

    public EnemyState GetCurrentState()
    {
        return _currentState;
    }

    public override void Die()
    {
        if(_animator != null)
        {
            _animator.SetTrigger("isDead");
        }

        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged -= TurnChanged;

        if (_currentState != null) {
            _currentState.Exit();
            _currentState = null;
        };
        
        base.Die();
    }

    public void TriggerPhase2()
    {
        if (_isPhase2) return;

        _isPhase2 = true;

        if (_animator != null)
            _animator.SetTrigger("phase2");

        Debug.Log($"{name} entered Phase2!");
    }

    public float GetHPRatio()
    {
        return (float)Stats.CurrentHP / Stats.MaxHP;
    }
}