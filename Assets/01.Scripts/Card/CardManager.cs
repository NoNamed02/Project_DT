using System;
using System.Collections.Generic;
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
    public void UsingCard(Card card, Character target)
    {
        Debug.Log($"target:{target} / card : {card.CardID}");

        // foreach (var cardeffect in card.cardEffects)
        // {
        //     cardeffect.Execute(target, card);
        //     // StartCoroutine 로 지연처리
        // }

        for (int i = 0; i < card.cardEffects.Count; i++)
        {
            card.cardEffects[i].Execute(target, card, card.CardSpec.effectAmount[i], card.CardSpec.effectHoldingTime[i]);
        }

        card.ActiveCard(); // 카드 사용 처리
        _graveyardDeck.SetCardToTop(card.CardID);
    }

    //IEnumerator 로 처리
}
