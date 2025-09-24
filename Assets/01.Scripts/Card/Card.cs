using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [Header("Card ID")]
    [SerializeField]
    private int _cardID = -1; // 초기값, -1인 상태로 사용되면 예외처리 핸들링
    // public int CardID => _cardID;

    private Canvas _canvas;

    /// <summary>
    /// 카드의 여러 값을 생싱시 세팅
    /// </summary>
    /// <param name="id"></param>
    public void Init(Canvas canvas, int id)
    {
        _canvas = canvas;
        _cardID = id;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cardID.ToString();
    }

    public int GetCardID()
    {
        return _cardID;
    }

    // 드래그 시작 처리
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");


        CardArrowLineMaker.Instance.ActiveLineDrawer(true);
        CardArrowLineMaker.Instance.SetIsDragging(true);

        var cam = Camera.main;
        float depth = 0f - cam.transform.position.z;
        Vector3 sp = new Vector3(eventData.position.x, eventData.position.y, depth);
        Vector3 start = cam.ScreenToWorldPoint(sp);
        start.z = 0f;
        CardArrowLineMaker.Instance.SetStartPoint(start);
        
    }

    // 드래그 중 처리
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");

        var cam = Camera.main;
        float depth = 0f - cam.transform.position.z; // 목표 z=0
        Vector3 sp = new Vector3(eventData.position.x, eventData.position.y, depth);
        Vector3 end = cam.ScreenToWorldPoint(sp);
        end.z = 0f;
        CardArrowLineMaker.Instance.SetEndPoint(end);
    }

    // 드래그 종료 처리
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        CardArrowLineMaker.Instance.SetIsDragging(false);

        CardArrowLineMaker.Instance.ActiveLineDrawer(false);
    }
}
