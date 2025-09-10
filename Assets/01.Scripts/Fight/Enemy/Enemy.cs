using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : Character
{
    private bool _acting = false;
    [SerializeField]
    private List<Movement> _movements = new List<Movement>();
    [SerializeField]
    private int _moveIndex = 0;
    void Awake()
    {
        _movements = GetComponentsInChildren<Movement>(true).ToList();
    }
    void Start()
    {
        if (TurnManager.Instance != null)
            TurnManager.Instance.OnTurnChanged += TurnChanged;
    }

    private void TurnChanged(TurnManager.TurnOwner owner)
    {
        if (owner == TurnManager.TurnOwner.Enemy)
        {
            Debug.Log("Enemy's Turn Start");
            if (_acting) return;
            _acting = true;
            if (_movements[_moveIndex] != null)
            {
                _movements[_moveIndex].gameObject.SetActive(true);
                _movements[_moveIndex].Action();
                StartCoroutine(EnemyTurnEnd());
            }
            }
            else
            {
                Debug.Log("Enemy's Turn End");
                _acting = false;
            }
    }

    public IEnumerator EnemyTurnEnd()
    {
        yield return new WaitForSeconds(2f);
        if (TurnManager.Instance != null)
        {
            _moveIndex = (_moveIndex + 1) % _movements.Count;
            TurnManager.Instance.NextTurn();
        }
    }
}