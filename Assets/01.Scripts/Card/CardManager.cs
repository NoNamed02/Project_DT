using System;
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

    [Header("graveyard Deck")]
    [SerializeField]
    private Deck _graveyardDeck;

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

        card.GetComponent<Card>().OnUsingCard += UsingCard;
    }

    /// <summary>
    /// 카드 효과 호출 함수
    /// </summary>
    /// <param name="target"></param>
    /// <param name="cardID"></param>
    public void UsingCard(int cardID, Character target)
    {
        Debug.Log($"target:{target} / card : {cardID}");
        // forTest
        BattleManager.Instance.ApplyDamage(target, 5);
        Debug.Log("적에게 5 데미지를 주었습니다");

        _graveyardDeck.SetCardToTop(cardID); // todo : 뭔가 뭔가 이상한 구조일 것 같음. 수정해야할 필요가 있지 않을까????
    }
}
