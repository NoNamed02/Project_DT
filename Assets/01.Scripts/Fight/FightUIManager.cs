using UnityEngine;
using UnityEngine.UI;

public class FightUIManager : MonoBehaviour
{
    [Header("플레이어 턴 종료 버튼")]
    [SerializeField]
    private Button _endTurnButton;

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
}