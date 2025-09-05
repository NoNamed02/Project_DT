using System.Collections;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    private int _hp = 10;
    private bool _acting = false;
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
            StartCoroutine(EnemyTurnEnd());
        }
        else
        {
            Debug.Log("Enemy's Turn End");
            _acting = false;
        }
    }

    private IEnumerator EnemyTurnEnd()
    {
        yield return new WaitForSeconds(5f);
        if (TurnManager.Instance != null)
            TurnManager.Instance.NextTurn();
    }
}