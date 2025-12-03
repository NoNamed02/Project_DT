using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    [Header("플레이어 턴 종료 버튼")]
    [SerializeField]
    private Button _endTurnButton;

    [SerializeField]
    private TextMeshProUGUI _coutCountText;

    [SerializeField]
    private GameObject _cardSelectorUI;
    [SerializeField]
    private Button _backToNodeMapBtn;

    void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
            PlayerTurnEnd()
        );

        if (TurnManager.Instance != null)
        {
            TurnManager.Instance.OnTurnChanged += HandleTurnChanged;
            // 초기 상태도 반영
            HandleTurnChanged(TurnManager.Instance.CurrentOwner);
        }
        _backToNodeMapBtn.onClick.AddListener(() =>
            SceneController.Instance.SceneMove("NodeMap")
        );
    }
    void Update()
    {
        _coutCountText.text = BattleManager.Instance.Player.Cost.ToString();
    }

    private void HandleTurnChanged(TurnManager.TurnOwner owner)
    {
        _endTurnButton.interactable = (owner == TurnManager.TurnOwner.Player);
    }
    private void PlayerTurnEnd()
    {
        _endTurnButton.interactable = false;
        if (TurnManager.Instance.CurrentOwner == TurnManager.TurnOwner.Player)
        {
            TurnManager.Instance.NextTurn();
        }
    }
    public void ActiveSelector()
    {
        _cardSelectorUI.SetActive(true);
    }
}