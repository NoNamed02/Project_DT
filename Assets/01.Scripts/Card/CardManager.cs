using UnityEngine;

public class CardManager : MonoBehaviour
{
    [Header("card area")]
    [SerializeField]
    private Transform _handArea;

    [Header("card prefab")]
    [SerializeField]
    private GameObject _card;

    [Header("draw Deck")]
    [SerializeField]
    private Deck _drawDeck;

    [Header("player object")]
    [SerializeField]
    private Player _player;

    void Start()
    {
        _player.OnDrawCard += InstanceCard;
    }

    public void InstanceCard(int cardID)
    {
        GameObject card = Instantiate(_card, _handArea);
        card.GetComponent<Card>().SetCardID(cardID);
    }
}
