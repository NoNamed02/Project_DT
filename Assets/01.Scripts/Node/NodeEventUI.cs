using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using Febucci.TextAnimatorForUnity;

public class NodeEventUI : MonoSingleton<NodeEventUI>
{
    [SerializeField] private GameObject _eventUI;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private List<Button> _choiceBtn;
    [SerializeField] private Button _evnetEndBtn;
    [SerializeField] private GameObject _cardSelectUI;

    private TypewriterComponent _typewriter;

    private void Start()
    {
        _typewriter = _text.GetComponent<TypewriterComponent>();
        if (_typewriter == null)
            Debug.LogError("TypewriterComponent가 Text 객체에 없습니다!");

        _eventUI.SetActive(false);
        _canvasGroup.alpha = 0f;

        foreach (var btn in _choiceBtn)
            btn.gameObject.SetActive(false);

        _evnetEndBtn.gameObject.SetActive(false);
    }

    public void StartEvent()
    {
        _eventUI.SetActive(true);

        foreach (var btn in _choiceBtn)
            btn.gameObject.SetActive(false);

        _evnetEndBtn.gameObject.SetActive(false);
        _canvasGroup.alpha = 0f;

        _canvasGroup.DOFade(1f, 0.8f)
            .SetEase(Ease.OutQuad);

        StartTypingDefault();
    }

    // ■■■ 기본 이벤트 텍스트 출력
    private void StartTypingDefault()
    {
        _typewriter.onTextShowed.RemoveAllListeners();
        _typewriter.onTextShowed.AddListener(OnTypingFinished);

        _typewriter.SetTypewriterSpeed(0.2f);

        _typewriter.ShowText(
            "공간이 찢어진 듯 일그러져 있다.\n\n" +
            "흩어진 기억 조각들이 균열 주위를 떠돌며, 마치 잊힌 장면을 억지로 끌어올리려는 듯 미세한 울림을 낸다.\n\n" +
            "손을 대면 무엇인가 변할 것 같은 기묘한 압력이 느껴진다.\n\n" +
            "기억의 균열이 흔들리고 있다...\n\n" +
            "당신은 무엇을 해야 할까?"
        );

        _typewriter.StartShowingText();
    }

    // ■■■ 선택지 버튼 등장
    private void OnTypingFinished()
    {
        foreach (var btn in _choiceBtn)
        {
            btn.gameObject.SetActive(true);

            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg == null) cg = btn.gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;

            RectTransform rt = btn.GetComponent<RectTransform>();
            Vector3 originalPos = rt.localPosition;

            rt.localPosition = originalPos + new Vector3(0, -25f, 0);

            DOTween.Sequence()
                .Append(cg.DOFade(1f, 0.35f).SetEase(Ease.OutQuad))
                .Join(rt.DOLocalMove(originalPos, 0.35f).SetEase(Ease.OutQuad));
        }
    }

    // ■■■ 선택지 결과 처리
    public void CardSelectUIEvent(bool choice)
    {
        foreach (var btn in _choiceBtn)
            btn.gameObject.SetActive(false);

        _typewriter.onTextShowed.RemoveAllListeners();

        if (choice)
        {
            _cardSelectUI.SetActive(true);

            _typewriter.ShowText(
                "당신은 기억의 틈 속에서 무언가를 끌어올렸다.\n\n" +
                "희미한 감정과 형체 없는 잔향이 손끝을 스친다.\n\n" +
                "마치 오래된 기억 한 조각이 되살아나는 듯하다."
            );
        }
        else
        {
            _typewriter.ShowText(
                "당신은 균열을 지나쳤다.\n\n" +
                "그러나 아무 일도 일어나지 않았다."
            );
        }

        _typewriter.onTextShowed.AddListener(ShowExitButton);
        _typewriter.StartShowingText();
    }

    // ■■■ 텍스트 완료 → 나가기 버튼 등장
    private void ShowExitButton()
    {
        _evnetEndBtn.gameObject.SetActive(true);

        CanvasGroup cg = _evnetEndBtn.GetComponent<CanvasGroup>();
        if (cg == null) cg = _evnetEndBtn.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0f;

        cg.DOFade(1f, 0.4f)
            .SetEase(Ease.OutQuad);

        // 버튼 누르면 UI 닫기
        _evnetEndBtn.onClick.RemoveAllListeners();
        _evnetEndBtn.onClick.AddListener(CloseEvent);
    }

    // ■■■ UI 종료 (페이드 아웃)
    private void CloseEvent()
    {
        _canvasGroup.DOFade(0f, 0.6f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                _eventUI.SetActive(false);
                _evnetEndBtn.gameObject.SetActive(false);
            });
    }
}
