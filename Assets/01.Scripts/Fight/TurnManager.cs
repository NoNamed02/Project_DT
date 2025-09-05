using UnityEngine;

public sealed class TurnManager : MonoSingleton<TurnManager>
{
    public event System.Action<TurnOwner> OnTurnChanged;
    public enum TurnOwner
    {
        Player,
        Enemy
    }
    [Header("현재 턴 카운트")]
    [SerializeField]
    private int _currentTurn = 0;
    [Header("현재 턴 주체")]
    [SerializeField]
    private TurnOwner _turnOwner = TurnOwner.Player;
    public TurnOwner CurrentOwner => _turnOwner;
    private bool _battleStarted = false;
    private bool _battleOver = false;

    void Start()
    {
        StartTurn();
    }
    /// <summary>전투 시작: 현재 _turnOwner를 첫 주체로 알림</summary>
    public void StartTurn()
    {
        if (_battleStarted || _battleOver) return;

        _battleStarted = true;
        _currentTurn = 1;
        OnTurnChanged?.Invoke(_turnOwner);
    }

    /// <summary>
    /// 현재 주체의 턴을 끝내고 타 주체에게 턴으로 넘겨줌
    /// </summary>
    public void NextTurn()
    {
        if (!_battleStarted || _battleOver) return;

        _turnOwner = (_turnOwner == TurnOwner.Player) ? TurnOwner.Enemy : TurnOwner.Player;
        _currentTurn++;

        OnTurnChanged?.Invoke(_turnOwner);
    }
}
