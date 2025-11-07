using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [Header("체력바 Slider")]
    [SerializeField]
    private Slider _hpBar;
    public Slider HPBar
    {
        get => _hpBar;
        set => _hpBar = value;
    }
    [SerializeField]
    private TextMeshProUGUI _hpPointText;

    [Header("방어력 관련 UI")]
    [SerializeField]
    private GameObject _shieldGUI;
    [SerializeField]
    private TextMeshProUGUI _shieldPointText;

    [Header("캐릭터 스크립트")]
    [SerializeField]
    private Character _character;

    void Awake()
    {
        _hpBar = GetComponent<Slider>();

        _hpBar.maxValue = _character.Stats.MaxHP;
        _hpBar.value = _character.Stats.CurrentHP;
    }

    void Update()
    {
        UpdateStatusGUI();
    }

    public void UpdateStatusGUI()
    {
        _hpPointText.text = $"{_character.Stats.CurrentHP} / {_character.Stats.MaxHP}";
        _hpBar.value = _character.Stats.CurrentHP;

        bool isShieldPointExist = _character.Stats.Shield > 0 ? true : false;
        _shieldGUI.gameObject.SetActive(isShieldPointExist);
        if (isShieldPointExist)
        {
            _shieldPointText.text = $"{_character.Stats.Shield}";
        }

    }
}
