using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private CardUI _cardUI;
    public int CardID = -1;
    private Canvas _canvas;
    [SerializeField]
    private CardSpec _cardSpec;
    public CardSpec CardSpec => _cardSpec;

    [SerializeField]
    private List<CardEffect> _cardEffects = new List<CardEffect>();
    public List<CardEffect> cardEffects { get => _cardEffects; }

    // if using card, than call this delegate
    public event System.Action<Card, Character> OnUsingCard;
    public event System.Action<Card> OnDiscardCard;

    private CardAnimator _cardAnimator;
    private bool _isDragging = false;
    public bool IsDragging => _isDragging;
    private bool _isSorted = false;
    public bool IsSorted { get => _isSorted; set => _isSorted = value; }
    public Vector3 OriginalPos { get; set; }

    /// <summary>
    /// 카드의 여러 값을 생성시 세팅
    /// </summary>
    /// <param name="id"></param>
    public void Init(Canvas canvas, int id, CardAnimator cardAnimator)
    {
        _canvas = canvas;
        CardID = id;
        _cardAnimator = cardAnimator;

        _cardSpec = CardDatabase.Instance.Get(CardID);
        if (_cardSpec == null)
        {
            Debug.LogError($"CardSpec을 찾을 수 없습니다: {CardID}");
        }
        _cardUI.CostText.text = _cardSpec.cost.ToString();
        _cardUI.InstructionText.text = _cardSpec.instruction.ToString();
        _cardUI.NameText.text = _cardSpec.cardName.ToString();

        _cardUI.CardImage.sprite = _cardSpec.cardImage;
        _cardUI.CardCategoryBar.sprite = _cardSpec.cardCategoryBar;

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
        if (!_isSorted)
            return;
        _isDragging = true;
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
        if (!_isSorted)
            return;
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
        if (!_isSorted)
            return;
        _isDragging = false;
        CardArrowLineMaker.Instance.SetIsDragging(false);
        CardArrowLineMaker.Instance.ActiveLineDrawer(false);

        // shoot ray to camera space
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (!BattleManager.Instance.Player.CanUseCard(_cardSpec.cost))
        {
            Debug.Log("cost 부족");
            _cardAnimator.ExitHighlight(this);
            return;
        }
        else if (_cardSpec.targeting[0] == "player")
        {
            UsingCard(BattleManager.Instance.Player);
            _cardAnimator.UsingCard(this);
        }
        else if (hit.collider != null)
        {
            if (_cardSpec.targeting[0] == "enemy" && hit.collider.gameObject.tag == "Enemy")
            {
                if (_cardSpec.targeting[1] == "single")
                {
                    Debug.Log("Hit to enemy");
                    UsingCard(hit.collider.gameObject.GetComponent<Character>());
                    //Destroy(gameObject); // todo : des는 어쩔 수 없다고 생각해도, 무덤으로 가는걸 여기서 처리하는게 나은가??
                }
                else if (_cardSpec.targeting[1] == "all")
                {
                    BattleManager.Instance.Player.Cost -= _cardSpec.cost;
                    Debug.Log($"플레이어 코스트 = {BattleManager.Instance.Player.Cost}");
                    //다중 공격 구현
                }
                _cardAnimator.UsingCard(this);
            }
        }
        else
        {
            _cardAnimator.ExitHighlight(this);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isSorted)
            return;
        _cardAnimator.EnterHighlight(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isSorted)
            return;
        _cardAnimator.ExitHighlight(this);
    }

    private void UsingCard(Character target)
    {
        OnUsingCard.Invoke(this, target);
        BattleManager.Instance.Player.Cost -= _cardSpec.cost;
        BattleManager.Instance.Player.EffectBleeding();
        Debug.Log($"플레이어 코스트 = {BattleManager.Instance.Player.Cost}");
    }
    public void ActiveCard()
    {
        _cardAnimator.Kill(this);
        gameObject.SetActive(false);   // 즉시 정렬 대상에서 제거됨
        HandArea.Instance.SortCards();
        Destroy(gameObject, 0.01f);    // 뒤 프레임에서 삭제
    }
    public void DiscardCard()
    {
        OnDiscardCard.Invoke(this);
        _cardAnimator.DiscardCard(this);
    }
    public void InactiveCard()
    {
        _cardAnimator.Kill(this);
        // gameObject.SetActive(false);
        // HandArea.Instance.SortCards();
        Destroy(gameObject, 0.01f);
    }
}
