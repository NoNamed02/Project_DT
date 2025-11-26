using DG.Tweening;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Player : Character
{
    [Header("Decks")]
    [SerializeField]
    private Deck _drawDeck;
    [SerializeField]
    private Deck _handDeck;
    [SerializeField]
    private Deck _graveyardDeck;
    public event System.Action<int> OnDrawCard;

    [SerializeField]
    private int _drawCount = 5;

    [SerializeField]
    private int _costRecoveryValue = 3;

    [SerializeField]
    private int _cost = 3;
    public int Cost { get => _cost; set => _cost = value; }

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        _drawDeck.SuffleDeck();
        HPForSet = RunTimeData.Instance.HP;
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;
        DrawCard(_drawCount);
    }

    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Player)
        {
            DrawCard(_drawCount);
            _cost = _costRecoveryValue;
        }
        else if (owner == TurnManager.TurnOwner.Enemy)
        {
            DOVirtual.DelayedCall(0.1f, () =>  // 0.1초 지연
            {
                HandArea.Instance.ThrowAwayHand();
            });
        }
    }

    private void DrawCard(int n)
    {
        for (int i = 0; i < n; i++)
        {
            if (!_drawDeck.IsDeckEmpty())
            {
                int drawCardId = _drawDeck.DrawCard();
                _handDeck.SetCardToTop(drawCardId);
                OnDrawCard.Invoke(drawCardId);
            }
            else
            {
                Debug.Log("draw deck is empty");
                if (!_graveyardDeck.IsDeckEmpty())
                {
                    _graveyardDeck.MoveCardsToDeck(_drawDeck);
                    if (!_drawDeck.IsDeckEmpty())
                        DrawCard(1);
                }
            }
        }
        // HandArea.Instance.SortCards();
    }

    public bool CanUseCard(int cardCost)
    {
        return _cost >= cardCost;
    }
}
