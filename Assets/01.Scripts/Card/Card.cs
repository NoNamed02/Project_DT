using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private int _cardID = -1; // 초기값, -1인 상태로 사용되면 예외처리 핸들링
    [Header("Card ID")]
    public int CardID { get => _cardID; set => _cardID = value; }

    private Canvas _canvas;

    [SerializeField]
    private CardSpec _cardSpec;
    public CardSpec CardSpec => _cardSpec;

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
    private Image _cardImage;
    [SerializeField]
    private Image _cardCategoryBar;

    /// <summary>
    /// 카드의 여러 값을 생성시 세팅
    /// </summary>
    /// <param name="id"></param>
    public void Init(Canvas canvas, int id)
    {
        _canvas = canvas;
        _cardID = id;

        _cardSpec = CardDatabase.Instance.Get(_cardID);
        if (_cardSpec == null)
        {
            Debug.LogError($"CardSpec을 찾을 수 없습니다: {_cardID}");
        }
        _costText.text = _cardSpec.cost.ToString();
        _instructionText.text = _cardSpec.instruction.ToString();
        _nameText.text = _cardSpec.cardName.ToString();
        
        _cardImage.sprite = _cardSpec.cardImage;
        _cardCategoryBar.sprite = _cardSpec.cardCategoryBar;

        for (int i = 0; i < _cardSpec.effect.Length; i++)
        {
            _cardEffects.Add(CardDatabase.Instance.GetCardEffect(_cardSpec.effect[i]));
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

        if (!BattleManager.Instance.Player.CanUseCard(_cardSpec.cost))
        {
            Debug.Log("cost 부족");
            return;
        }
        else
        {
            BattleManager.Instance.Player.Cost -= _cardSpec.cost;
            Debug.Log($"플레이어 코스트 = {BattleManager.Instance.Player.Cost}");
        }
        if (hit.collider != null)
        {
            if (_cardSpec.targeting[0] == "enemy" && hit.collider.gameObject.tag == "Enemy")
            {
                if (_cardSpec.targeting[1] == "single")
                {
                    Debug.Log("Hit to enemy");
                    OnUsingCard.Invoke(this, hit.collider.gameObject.GetComponent<Character>());
                    //Destroy(gameObject); // todo : des는 어쩔 수 없다고 생각해도, 무덤으로 가는걸 여기서 처리하는게 나은가??
                }
                else if (_cardSpec.targeting[1] == "all")
                {
                    //다중 공격 구현
                }
            }
        }
        else if (_cardSpec.targeting[0] == "player")
        {
            OnUsingCard.Invoke(this, GameObject.FindWithTag("Player").GetComponent<Character>());
        }
    }

    public void ActiveCard()
    {
        BattleManager.Instance.EffectBleeding(BattleManager.Instance.Player);
        Destroy(gameObject);
    }
}
