using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntentIconUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI 요소")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _valueText;

    [Header("툴팁 (옵션)")]
    [SerializeField] private GameObject _tooltip;
    [SerializeField] private TextMeshProUGUI _tooltipText;

    private IntentData _currentIntent;

    /// <summary>
    /// Intent 정보 업데이트
    /// </summary>
    public void UpdateIntent(Sprite icon, int value, string description = "")
    {
        if (_iconImage != null)
        {
            _iconImage.sprite = icon;
            _iconImage.gameObject.SetActive(icon != null);
        }

        if (_valueText != null)
        {
            if (value > 0)
            {
                _valueText.gameObject.SetActive(true);
                _valueText.text = value.ToString();
            }
            else
            {
                _valueText.gameObject.SetActive(false);
            }
        }

        _currentIntent = new IntentData(IntentType.Unknown, value, description);

        if (_tooltip != null)
            _tooltip.SetActive(false);
    }

    /// <summary>
    /// 마우스 호버 시 툴팁 표시
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltip != null && _currentIntent != null && !string.IsNullOrEmpty(_currentIntent.Description))
        {
            _tooltip.SetActive(true);
            if (_tooltipText != null)
            {
                _tooltipText.text = _currentIntent.Description;
            }
        }
    }

    /// <summary>
    /// 마우스 벗어나면 툴팁 숨김
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltip != null)
        {
            _tooltip.SetActive(false);
        }
    }
}
