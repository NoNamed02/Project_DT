using DG.Tweening;
using TMPro;
using UnityEngine;

public class TurnChangeUI : MonoBehaviour
{
    private RectTransform _rt;
    private CanvasGroup _cg;
    public TextMeshProUGUI tmp;
    private Vector3 startPos;
    private Vector3 endPos;
    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _cg = GetComponent<CanvasGroup>();

        startPos = new Vector3(-100f, 0f, 0f);
        endPos = new Vector3(0f, 0f, 0f);
    }
    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;
        TurnChanged(TurnManager.Instance.CurrentOwner);
    }

    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        DOTween.Kill(_rt);  
        DOTween.Kill(_cg);

        if (owner == TurnManager.TurnOwner.Player)
            tmp.text = "Player's Turn";
        else
            tmp.text = "Enemy's Turn";

        _rt.localPosition = startPos;
        _cg.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(_rt.DOLocalMove(endPos, 0.5f).SetEase(Ease.OutCubic));
        seq.Join(_cg.DOFade(1f, 0.4f));
        seq.AppendInterval(0.8f);
        seq.Append(_cg.DOFade(0f, 0.4f).SetEase(Ease.InCubic));

        seq.Play();
    }
}
