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


    [Header("select card")]
    [SerializeField]
    private Card _selectCard;

    [SerializeField]
    private Canvas _canvas;          // 최상위 Canvas

    void Start()
    {
        _player.OnDrawCard += InstanceCard;
    }

    /// <summary>
    /// 카드 드로우시 생성
    /// </summary>
    /// <param name="cardID"></param>
    public void InstanceCard(int cardID)
    {
        GameObject card = Instantiate(_card, _handArea);
        card.GetComponent<Card>().Init(_canvas, cardID);
    }

    /// <summary>
    /// 카드 효과 호출 함수
    /// </summary>
    /// <param name="target"></param>
    /// <param name="cardID"></param>
    public void UsedCard(Character target, int cardID)
    {
        Debug.Log($"target:{target} / card : {cardID}");
    }
}
