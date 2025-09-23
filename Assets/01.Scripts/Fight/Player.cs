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

    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;
    }

    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Player)
        {
            Debug.Log("Player's Turn Start");
            DrawCard(1);
        }
        else
        {
            Debug.Log("Player's Turn End");
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
            }
        }
    }
}
