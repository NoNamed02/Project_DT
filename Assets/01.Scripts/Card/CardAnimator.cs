using UnityEngine;
using DG.Tweening;

public class CardAnimator : MonoBehaviour
{
    [SerializeField]
    private float moveDuration = 0.5f; // 이동 시간
    [SerializeField]
    public Vector3 startOffset = new Vector3(-500f, -300f, 0f); // 왼쪽 하단 오프셋
    // [SerializeField]
    // private HandArea _handArea;

    private RectTransform rt;

    private float hoverHeight = 40f;
    private float hoverScale = 1.15f;
    private Vector3 _originalPos;

    public void DrawCard(Card card)
    {
        RectTransform rt = card.GetComponent<RectTransform>();

        rt.anchoredPosition = startOffset;
        rt.localScale = Vector3.one * 0.3f;

        RectTransform handRT = HandArea.Instance.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPos(handRT.anchoredPosition, moveDuration));
        seq.Join(rt.DOScale(1f, moveDuration * 1.5f));

        seq.OnComplete(() =>
        {
            HandArea.Instance.SortCards();
        });
    }

    public void EnterHighlight(Card card)
    {
        RectTransform rt = card.GetComponent<RectTransform>();

        if (!card.IsDragging)
        {
            _originalPos = rt.localPosition;
        }

        rt.DOKill();
        rt.DOScale(new Vector3(1f, 1f, 1f) * hoverScale, 0.15f);
        rt.DOLocalMoveY(_originalPos.y + hoverHeight, 0.15f)
           .SetUpdate(true);
    }
    public void ExitHighlight(Card card)
    {
        if (card.IsDragging)
            return;

        RectTransform rt = card.GetComponent<RectTransform>();

        rt.DOKill();
        rt.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
        rt.DOLocalMove(_originalPos, 0.15f)
           .SetUpdate(true);
    }
    public void Kill(Card card)
    {
        rt = null;
        rt = card.GetComponent<RectTransform>();
        if (rt != null)
            rt.DOKill();
    }
}
