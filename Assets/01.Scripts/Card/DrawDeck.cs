using UnityEngine;

public class DrawDeck : Deck
{
    void Awake()
    {
        Cards = RunTimeData.Instance.DeckList;
    }
}
