using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR;

public class CardManager : MonoBehaviour
{
    [Header("card prefab")]
    [SerializeField]
    private GameObject _card;

    [Header("draw Deck")]
    [SerializeField]
    private Deck _drawDeck;

    [Header("graveyard Deck")]
    [SerializeField]
    private Deck _graveyardDeck;
    
    [Header("player's hand deck")]
    [SerializeField]
    private Deck _playerHand;

    [Header("player object")]
    [SerializeField]
    private Player _player;


    [Header("select card")]
    [SerializeField]
    private Card _selectCard;


    [SerializeField]
    private CardAnimator _cardAnimator;

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
        GameObject card = Instantiate(_card, HandArea.Instance.GetComponent<RectTransform>());
        card.GetComponent<Card>().Init(cardID, _cardAnimator);
        card.GetComponent<Card>().OnUsingCard += UsingCard;
        card.GetComponent<Card>().OnDiscardCard += DiscardCard;
        _cardAnimator.DrawCard(card.GetComponent<Card>());
    }

    /// <summary>
    /// 카드 효과 호출 함수
    /// </summary>
    /// <param name="target"></param>
    /// <param name="cardID"></param>
    public void UsingCard(Card card, Character target)
    {
        _playerHand.DeleteCard(card.CardID);
        _graveyardDeck.SetCardToTop(card.CardID);
        for (int i = 0; i < card.cardEffects.Count; i++)
        {
            int index = i;
            float delay = 0.3f * i;

            DOVirtual.DelayedCall(delay, () =>
            {
                card.cardEffects[index].Execute(
                    target,
                    card.CardSpec.effectAmount[index],
                    card.CardSpec.effectHoldingTime[index]
                );
            });
        }
    }

    public void DiscardCard(Card card)
    {
        _playerHand.DeleteCard(card.CardID);
        _graveyardDeck.SetCardToTop(card.CardID);
    }
}
