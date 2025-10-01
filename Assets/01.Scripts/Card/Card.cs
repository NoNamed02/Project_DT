using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private int _cardID = -1; // 초기값, -1인 상태로 사용되면 예외처리 핸들링
    [Header("Card ID")]
    public int CardID { get => _cardID; set => _cardID = value; }

    private Canvas _canvas;

    // if using card, than call this delegate
    public event System.Action<Card, Character> OnUsingCard;

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

    // 드래그 시작 처리
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");

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
        //Debug.Log("OnDrag");

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
        //Debug.Log("OnEndDrag");
        CardArrowLineMaker.Instance.SetIsDragging(false);

        CardArrowLineMaker.Instance.ActiveLineDrawer(false);

        // shoot ray to camera space
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(eventData.position);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
        if (hit.collider.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit to enemy");
            OnUsingCard.Invoke(this, hit.collider.gameObject.GetComponent<Character>());
            //Destroy(gameObject); // todo : des는 어쩔 수 없다고 생각해도, 무덤으로 가는걸 여기서 처리하는게 나은가??
        }
    }

    public void ActiveCard()
    {
        Destroy(gameObject);
    }
}
