using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [Header("덱 리스트")]
    [SerializeField]
    private List<int> _cards = new List<int>();

    // 덱에서 덱으로 이동
    // ex - 드로우 덱 전부 사용시 무덤덱 -> 드로우 덱으로 리스트 이동
    private void MoveCardsToDeck(Deck deck)
    {
        List<int> targetDeck = deck.GetCards();
        targetDeck.AddRange(_cards);

        ResetDeck();
    }

    // 카드 한장을 드로우 후 반환
    public int DrawCard()
    {
        if (_cards.Count == 0)
        {
            Debug.Log("deck is empty");
            return -1; // 핸들링으로 if (return == -1) 로 빈 것 확인
        }
        else
        {
            int cardId = _cards[_cards.Count - 1];
            _cards.RemoveAt(_cards.Count - 1);

            return cardId;
        }
    }

    /// <summary>
    /// 덱에 카드가 남아 있는가 확인
    /// </summary>
    /// <returns></returns>
    public bool IsDeckEmpty()
    {
        return _cards.Count == 0;
    }

    // 덱 섞기 = 피셔 예이츠 제자리 셔플
    private void SuffleDeck()
    {
        for (int i = _cards.Count - 1; i > 0; i++)
        {
            int j = Random.Range(0, i + 1);
            (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
        }
    }


    // 난수 드로우 기능 구현 필요
    public int RandomDrawCard()
    {
        return 0;
    }

    // 자신의 덱을 반환
    public List<int> GetCards()
    {
        return _cards;
    }

    // 덱 세팅
    public void SetDeck(List<int> cardIdData)
    {
        _cards = cardIdData;
    }

    // 자신의 덱을 비움
    private void ResetDeck()
    {
        _cards.Clear();
    }

    // 덱에 카드를 상단에 세팅
    public void SetCardToTop(int cardId)
    {
        _cards.Add(cardId);
    }
}
