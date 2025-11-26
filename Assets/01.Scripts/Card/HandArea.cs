using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandArea : MonoSingleton<HandArea>
{
    private float _maxDegree = 20;
    public void SortCards()
    {
        Card[] _hand = GetComponentsInChildren<Card>(false);
        if (_hand.Length == 1)
        {
            RectTransform rt = _hand[0].GetComponent<RectTransform>();
            rt.DOKill();
            rt.DOLocalRotate(Vector3.zero, 0.2f);
            rt.DOLocalMove(Vector3.zero, 0.2f);
            _hand[0].OriginalPos = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < _hand.Length; i++)
        {
            _hand[i].IsSorted = false;
        }

        float angleStep = _maxDegree / (_hand.Length - 1);
        for (int i = 0; i < _hand.Length; i++)
        {
            int index = i;

            RectTransform rt = _hand[index].GetComponent<RectTransform>();
            float angle = -_maxDegree / 2 + angleStep * i;

            float radius = _hand.Length * 150f;

            float spacingFactor = 2f; // 카드 간 거리 상승값 (1.0 ~ 2.0 사이 조절)

            float rad = angle * Mathf.Deg2Rad;

            float posX = Mathf.Sin(rad) * radius * spacingFactor;
            float posY = _hand.Length % 2 == 1 && index == _hand.Length / 2 ? -Mathf.Abs(i - ((_hand.Length - 1) / 2f)) * 20f - 10f : -Mathf.Abs(i - ((_hand.Length - 1) / 2f)) * 20f;

            Sequence seq = DOTween.Sequence();
            seq.Join(rt.DOLocalRotate(new Vector3(0, 0, -angle), 0.2f));
            seq.Join(rt.DOLocalMove(new Vector3(posX, posY, 0), 0.2f));


            seq.OnComplete(() =>
            {
                _hand[index].OriginalPos = rt.localPosition;
                _hand[index].IsSorted = true;
            });
        }
    }

    public void ThrowAwayHand()
    {
        Card[] _hand = GetComponentsInChildren<Card>(false);
        for (int i = 0; i < _hand.Length; i++)
        {
            _hand[i].DiscardCard();
        }
    }
}
