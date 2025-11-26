using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class CardSelector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private CardUI _cardUI;
    private CardSpec _cardSpec;
    [SerializeField]
    private int _cardID = -1;

    private CanvasGroup _canvasGroup;
    private RectTransform _rt;
    private Tween _hoverTween;

    public bool IsActive = false;

    [SerializeField]
    private int min = 1;
    [SerializeField]
    private int max = 10;

    void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rt = GetComponent<RectTransform>();

        Init(_cardID);
        PlayAppearAnimation();
    }

    public void Init(int id)
    {
        // _cardID = id;
        _cardID = Random.Range(min - 1, max - 1);
        _cardSpec = CardDatabase.Instance.Get(_cardID);
        if (_cardSpec == null)
        {
            Debug.LogError($"CardSpec을 찾을 수 없습니다: {_cardID}");
        }

        _cardUI.CostText.text = _cardSpec.cost.ToString();
        _cardUI.InstructionText.text = _cardSpec.instruction;
        _cardUI.NameText.text = _cardSpec.cardName;

        _cardUI.CardImage.sprite = _cardSpec.cardImage;
        _cardUI.CardCategoryBar.sprite = _cardSpec.cardCategoryBar;
    }

    private void PlayAppearAnimation()
    {
        // 초기 상태
        _canvasGroup.alpha = 0;
        _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, -150f);

        // 애니메이션 (0.4초 정도가 자연스러움)
        _canvasGroup.DOFade(1f, 0.4f);
        _rt.DOAnchorPosY(0f, 0.4f).SetEase(Ease.OutCubic);
    }

    public void InsertCardToDeck()
    {
        if (IsActive == true)
            return;
        IsActive = true;
        RunTimeData.Instance.DeckList.Add(_cardID);
        PlayDisappearAnimation();
        CardSelector[] cardSelectors = FindObjectsByType<CardSelector>(FindObjectsSortMode.None);
        foreach (var cardSelector in cardSelectors)
        {
            if (cardSelector != this)
            {
                cardSelector.PlayDisappearDownAnimation();
                cardSelector.IsActive = true;
            }
        }
    }

    private void PlayDisappearAnimation()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        RectTransform rt = GetComponent<RectTransform>();

        if (cg == null)
            cg = gameObject.AddComponent<CanvasGroup>();

        // 부모(Image UI)
        GameObject parentObj = transform.parent.gameObject;

        CanvasGroup parentCg = parentObj.GetComponent<CanvasGroup>();
        if (parentCg == null)
            parentCg = parentObj.AddComponent<CanvasGroup>();

        Sequence seq = DOTween.Sequence();

        // 카드 위로 사라짐 + 페이드 아웃
        seq.Append(rt.DOAnchorPosY(rt.anchoredPosition.y + 120f, 0.35f).SetEase(Ease.OutCubic));
        seq.Join(cg.DOFade(0f, 0.35f));

        // 부모(Image UI)도 페이드 아웃
        seq.Join(parentCg.DOFade(0f, 0.35f));

        seq.OnComplete(() =>
        {
            // 부모 UI 비활성화
            parentObj.SetActive(false);

            // 카드 삭제
            Destroy(gameObject);
        });
    }

    public void PlayDisappearDownAnimation()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        RectTransform rt = GetComponent<RectTransform>();

        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();

        Sequence seq = DOTween.Sequence();

        // ↓ 아래쪽(-Y 방향)으로 이동 + 페이드 아웃
        seq.Append(rt.DOAnchorPosY(rt.anchoredPosition.y - 120f, 0.35f)
                    .SetEase(Ease.InCubic));

        seq.Join(cg.DOFade(0f, 0.35f));

        // 애니메이션 종료 후 삭제
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsActive) return;

        if (_hoverTween != null) _hoverTween.Kill();
        _hoverTween = transform.DOScale(1.08f, 0.15f).SetEase(Ease.OutCubic);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsActive) return;

        if (_hoverTween != null) _hoverTween.Kill();
        _hoverTween = transform.DOScale(1f, 0.15f).SetEase(Ease.OutCubic);
    }
}