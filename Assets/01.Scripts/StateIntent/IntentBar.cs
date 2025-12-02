using UnityEngine;
using System.Collections.Generic;

public class IntentBar : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private Enemy _enemy;

    [Header("Intent 아이콘 프리팹")]
    [SerializeField] private IntentIconUI _intentIconPrefab;
    [SerializeField] private Transform _iconContainer;

    [Header("Intent 아이콘 스프라이트")]
    [SerializeField] private Sprite _attackIcon;
    [SerializeField] private Sprite _defendIcon;
    [SerializeField] private Sprite _healIcon;
    [SerializeField] private Sprite _bleedingIcon;
    [SerializeField] private Sprite _poisonIcon;
    [SerializeField] private Sprite _dodgeIcon;
    [SerializeField] private Sprite _weakenIcon;
    [SerializeField] private Sprite _unknownIcon;

    private List<IntentIconUI> _spawnedIcons = new List<IntentIconUI>();
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    private void Start()
    {
        if(_enemy == null)
            _enemy = GetComponent<Enemy>();

        if (_iconContainer == null)
            _iconContainer = transform;

        if (_enemy != null)
            _enemy.OnIntentChanged += RefreshIntent;

        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += OnTurnChanged;

        RefreshIntent();
    }

    void OnDestroy()
    {
        if(_enemy != null)
            _enemy.OnIntentChanged -= RefreshIntent;

        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged -= OnTurnChanged;
    }

    private void OnTurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Player)
        {
            RefreshIntent();
            SetVisible(true);
        }
        else
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool visible)
    {
        if(_canvasGroup != null)
        {
            _canvasGroup.alpha = visible ? 1f : 0f;
            _canvasGroup.blocksRaycasts = visible;
        }
    }

    /// <summary>
    /// Intent UI 갱신 (다중 Intent 지원)
    /// </summary>
    public void RefreshIntent()
    {
        if (_enemy == null) return;

        // 현재 상태에서 IntentDataSet 가져오기
        IntentDataSet intentSet = _enemy.GetCurrentIntentSet();

        if (intentSet == null || intentSet.Count == 0)
        {
            intentSet = new IntentDataSet(new IntentData(IntentType.Unknown));
        }

        // 필요한 아이콘 수만큼 생성/활성화
        EnsureIconCount(intentSet.Count);

        // 각 Intent에 맞게 아이콘 업데이트
        for (int i = 0; i < intentSet.Count; i++)
        {
            IntentData intent = intentSet.Intents[i];
            Sprite icon = GetIconForType(intent.Type);

            _spawnedIcons[i].UpdateIntent(icon, intent.Value, intent.Description);
            _spawnedIcons[i].gameObject.SetActive(true);
        }

        // 남은 아이콘 비활성화
        for (int i = intentSet.Count; i < _spawnedIcons.Count; i++)
        {
            _spawnedIcons[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 필요한 수만큼 아이콘 확보 (오브젝트 풀링)
    /// </summary>
    private void EnsureIconCount(int count)
    {
        while (_spawnedIcons.Count < count)
        {
            IntentIconUI newIcon = Instantiate(_intentIconPrefab, _iconContainer);
            _spawnedIcons.Add(newIcon);
        }
    }

    private Sprite GetIconForType(IntentType type)
    {
        return type switch
        {
            IntentType.Attack => _attackIcon,
            IntentType.Defend => _defendIcon,
            IntentType.Heal => _healIcon,
            IntentType.Bleeding => _bleedingIcon,
            IntentType.Poison => _poisonIcon,
            IntentType.Dodge => _dodgeIcon,
            IntentType.Weaken => _weakenIcon,
            _ => _unknownIcon
        };
    }


}
