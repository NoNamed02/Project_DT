using UnityEngine;

public class Movement : MonoBehaviour
{
    private enum ActionState
    {
        attack,
        defence,
        effect
    }
    private Enemy _enemy;
    [SerializeField]
    private ActionState _actionState = new ActionState();

    [SerializeField]
    private int _actionValue = 3;
    void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
        gameObject.SetActive(false);
    }
    public void Action()
    {
        switch (_actionState)
        {
            case ActionState.attack:
                Debug.Log($"{gameObject.name} Movement: attack Player {_actionValue}");
                BattleManager.Instance.ApplyDamage(BattleManager.Instance.GetPlayer(), _actionValue);
                break;
            case ActionState.defence:
                break;
            case ActionState.effect:
                break;
            default:
                Debug.LogError($"{gameObject.name} = Movement : use undefined ActionState");
                break;
        }
        gameObject.SetActive(false);
    }
}
