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
            return;
        }

        for (int i = 0; i < _hand.Length; i++)
        {
            _hand[i].IsSorted = false;
        }

        float angleStep = _maxDegree / (_hand.Length - 1);
        for (int i = 0; i < _hand.Length; i++)
        {
            int index = i;   // ← 지역 변수로 복사 (closure-safe)

            RectTransform rt = _hand[index].GetComponent<RectTransform>();
            float angle = -_maxDegree / 2 + angleStep * i;

            // float radius = _hand.Length * 200;
            float radius = _hand.Length * 150f;

            float spacingFactor = 2f; // 카드 간 거리 상승값 (1.0 ~ 2.0 사이 조절)

            float rad = angle * Mathf.Deg2Rad;

            float posX = Mathf.Sin(rad) * radius * spacingFactor;
            float posY = -Mathf.Abs(i - ((_hand.Length - 1) / 2f)) * 20f;

            Sequence seq = DOTween.Sequence();
            seq.Join(rt.DOLocalRotate(new Vector3(0, 0, -angle), 0.2f));
            seq.Join(rt.DOLocalMove(new Vector3(posX, posY, 0), 0.2f));

            seq.OnComplete(() =>
            {
                _hand[index].IsSorted = true;  // 이제 안전
            });
        }
    }
}
