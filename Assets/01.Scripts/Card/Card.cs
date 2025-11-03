using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private int _cardID = -1; // 초기값, -1인 상태로 사용되면 예외처리 핸들링
    [Header("Card ID")]
    public int CardID { get => _cardID; set => _cardID = value; }

    private Canvas _canvas;

    [SerializeField]
    private CardSpec _cardSpec;
    public CardSpec CardSpec { get => _cardSpec; }
    [SerializeField]
    private List<CardEffect> _cardEffects = new List<CardEffect>();
    public List<CardEffect> cardEffects { get => _cardEffects; }
    // public List<CardEffect> cardEffects { get; }

    // if using card, than call this delegate
    public event System.Action<Card, Character> OnUsingCard;

    [SerializeField]
    private TextMeshProUGUI _costText;
    [SerializeField]
    private TextMeshProUGUI _instructionText;
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private TextMeshProUGUI _idText;

    void Awake()
    {
        _costText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _instructionText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _nameText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _idText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 카드의 여러 값을 생성시 세팅
    /// </summary>
    /// <param name="id"></param>
    public void Init(Canvas canvas, int id)
    {
        _canvas = canvas;
        _cardID = id;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _cardID.ToString();

        _cardSpec = CardDatabase.Instance.Get(_cardID);
        if (_cardSpec == null)
        {
            Debug.LogError($"CardSpec을 찾을 수 없습니다: {_cardID}");
        }
        _costText.text = _cardSpec.cost.ToString();
        _instructionText.text = _cardSpec.instruction.ToString();
        _nameText.text = _cardSpec.cardName.ToString();
        _idText.text = _cardSpec.id.ToString();

        for (int i = 0; i < _cardSpec.effect.Length; i++)
        {
            Debug.Log(i + "번째 effect add");
            _cardEffects.Add(CardDatabase.Instance.GetCardEffect(
                _cardSpec.effect[i], _cardSpec.effectAmount[i], _cardSpec.effectHoldingTime[i]));
        }
        // foreach (var effect in _cardEffects)
        // {
        //     Debug.Log($"Card에 등록된 Effect: {effect.name}");
        // }
        if (_cardEffects[0] == null)
        {
            Debug.Log($"{CardID} : 뭔가 잘못되었다");
        }
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
