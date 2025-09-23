using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Card ID")]
    [SerializeField]
    private int _cardID = -1; // 초기값, -1인 상태로 사용되면 예외처리 핸들링

    /// <summary>
    /// 카드의 id를 세팅 = card manager에서 call해서 사용
    /// </summary>
    /// <param name="id"></param>
    public void SetCardID(int id)
    {
        _cardID = id;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cardID.ToString();
    }
}
