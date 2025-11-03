using UnityEngine;
using UnityEngine.UI;

public class CharacterHPBar : MonoBehaviour
{
    [Header("체력바 Slider")]
    [SerializeField]
    private Slider _HpBar;

    [Header("캐릭터 스크립트")]
    [SerializeField]
    private Character _character;

    void Awake()
    {
        _HpBar = GetComponent<Slider>();

        _HpBar.maxValue = _character.Stats.MaxHP;
        _HpBar.value = _character.Stats.CurrentHP;
    }

    void Update()
    {
        _HpBar.value = _character.Stats.CurrentHP;
    }
}
