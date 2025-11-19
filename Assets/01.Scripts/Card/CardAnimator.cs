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

    public void UsingCard(Card card)
    {
        RectTransform rt = card.GetComponent<RectTransform>();
        CanvasGroup cg = card.GetComponent<CanvasGroup>();

        Vector2 startPos = rt.anchoredPosition;
        Vector2 forwardPos = startPos + new Vector2(0f, 60f);

        Sequence seq = DOTween.Sequence();

        seq.Append(rt.DOScale(1.2f, 0.1f));

        seq.Join(rt.DOAnchorPos(forwardPos, 0.12f));

        seq.Append(cg.DOFade(0f, 0.15f));

        seq.OnComplete(() =>
        {
            card.ActiveCard();
        });
    }

    public void EnterHighlight(Card card)
    {
        RectTransform rt = card.GetComponent<RectTransform>();

        rt.DOKill();
        rt.DOScale(new Vector3(1f, 1f, 1f) * hoverScale, 0.15f);
        rt.DOLocalMoveY(card.OriginalPos.y + hoverHeight, 0.15f)
           .SetUpdate(true);
    }
    public void ExitHighlight(Card card)
    {
        if (card.IsDragging)
            return;

        RectTransform rt = card.GetComponent<RectTransform>();

        rt.DOKill();
        rt.DOScale(new Vector3(1f, 1f, 1f), 0.15f);
        rt.DOLocalMove(card.OriginalPos, 0.15f)
           .SetUpdate(true);
    }
    public void Kill(Card card)
    {
        rt = null;
        rt = card.GetComponent<RectTransform>();
        if (rt != null)
            rt.DOKill();
    }
    public void DiscardCard(Card card)
    {
        RectTransform rt = card.GetComponent<RectTransform>();
        CanvasGroup cg = card.GetComponent<CanvasGroup>();

        Vector3 endPos = rt.anchoredPosition + new Vector2(800f, 0f);

        Sequence seq = DOTween.Sequence();

        seq.Append(rt.DOAnchorPos(endPos, 0.6f).SetEase(Ease.InBack));
        // seq.Join(rt.DORotate(new Vector3(0, 0, 120f), 0.6f));
        seq.Join(cg.DOFade(0f, 0.4f));

        seq.OnComplete(() =>
        {
            card.InactiveCard();
        });
    }
}
