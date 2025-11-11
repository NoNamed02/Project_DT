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

    protected override void Awake()
    {
        base.Awake();
        _drawDeck.SuffleDeck();
    }
    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;
        DrawCard(_drawCount);
    }

    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Player)
        {
            DrawCard(_drawCount);
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
    }
}
