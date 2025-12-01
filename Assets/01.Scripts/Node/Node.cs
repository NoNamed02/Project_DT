using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using DarkTonic.MasterAudio;

public class Node : MonoBehaviour
{
    public enum NodeTypeList { Start, Fight, Event, Rest }
    public NodeTypeList NodeType;
    public int Layer = 0;
    public int ID = 0;

    [SerializeField] private List<Node> _linkNode = new List<Node>();
    [SerializeField] private Sprite[] _icon = new Sprite[3];

    [Header("Line Settings")]
    public float LineWidth = 0.1f;
    public Color LineColor = Color.white;
    public Material LineMaterial;

    private Tween _scaleTween;
    private Vector3 _originalScale;
    private bool _areChildrenAnimating = false;

    private void Awake()
    {
        _originalScale = transform.localScale;
        
        if (_icon != null && _icon.Length > (int)NodeType && (int)NodeType >= 0)
        {
            var img = gameObject.GetComponent<Image>();
            if(img != null) img.sprite = _icon[(int)NodeType];
        }
    }

    private void Start()
    {
        CreateLines();
        GetComponent<Button>().onClick.AddListener(()=>
            MasterAudio.PlaySound("blade_hit_bind")
        );
    }

    private void CreateLines()
    {
        foreach (Node targetNode in _linkNode)
        {
            if (targetNode == null) continue;

            GameObject lineObj = new GameObject($"Line_To_{targetNode.name}");
            
            lineObj.transform.SetParent(transform);
            lineObj.transform.localPosition = Vector3.zero; // 위치 초기화
            lineObj.transform.localScale = Vector3.one;

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();

            lr.startWidth = LineWidth;
            lr.endWidth = LineWidth;
            lr.material = LineMaterial != null ? LineMaterial : new Material(Shader.Find("Sprites/Default")); // 재질 없으면 기본 할당
            lr.startColor = LineColor;
            lr.endColor = LineColor;
            
            lr.sortingLayerName = "Default"; 
            lr.sortingOrder = -1;
            lr.positionCount = 2;
            lr.SetPosition(0, transform.position); 
            lr.SetPosition(1, targetNode.transform.position);
        }
    }

    private void Update()
    {
        CheckNodeState();
    }

    private void CheckNodeState()
    {
        bool isCurrentNode = (Layer == RunTimeData.Instance.LayerIndex) && 
                             (ID == RunTimeData.Instance.CurrentNodeID);

        if (isCurrentNode)
        {
            if (!_areChildrenAnimating)
            {
                foreach (var node in _linkNode) if (node != null) node.StartPulseAnimation();
                _areChildrenAnimating = true;
            }
        }
        else
        {
            if (_areChildrenAnimating)
            {
                foreach (var node in _linkNode) if (node != null) node.StopPulseAnimation();
                _areChildrenAnimating = false;
            }
        }
    }

    public void StartPulseAnimation()
    {
        if (_scaleTween != null && _scaleTween.IsActive()) return;
        _scaleTween = transform.DOScale(_originalScale * 1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void StopPulseAnimation()
    {
        if (_scaleTween != null) _scaleTween.Kill();
        _scaleTween = null;
        transform.localScale = _originalScale;
    }

    public void SelectNode()
    {
        if (_scaleTween == null || !_scaleTween.IsActive()) return; // 갈 수 없는 노드

        switch(NodeType)
        {
            case NodeTypeList.Fight: GoToFight(); break;
            case NodeTypeList.Event: GoToEvent(); break;
            case NodeTypeList.Rest: GoToRest(); break;
            default: LayerIndexUp(); break;
        }
    }

    private void GoToFight() { UpdateCurrentNodeData(); SceneController.Instance.SceneMove("Fight"); }
    private void GoToRest() { UpdateCurrentNodeData(); } // 로직 추가 필요
    private void GoToEvent() { UpdateCurrentNodeData(); } // 로직 추가 필요

    private void UpdateCurrentNodeData()
    {
        RunTimeData.Instance.LayerIndex = this.Layer;
        RunTimeData.Instance.CurrentNodeID = this.ID;
    }
    public void LayerIndexUp() { UpdateCurrentNodeData(); }

    private void OnDestroy()
    {
        if (_scaleTween != null) _scaleTween.Kill();
    }
}