using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int _hp = 30;

    void Start()
    {
        // if (TurnManager.Instance != null)
        //     TurnManager.Instance.OnTurnChanged += TurnChanged;
    }

    // private void TurnChanged(TurnManager.TurnOwner owner)
    // {
    //     if (owner == TurnManager.TurnOwner.Player)
    //     {
    //         Debug.Log("Player's Turn Start");
    //     }
    //     else
    //     {
    //         Debug.Log("Player's Turn End");
    //     }
    // }
}
