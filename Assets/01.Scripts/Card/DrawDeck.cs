using UnityEngine;

public class DrawDeck : Deck
{
    void Start()
    {
        Cards = RunTimeData.Instance.DeckList;
    }
}
