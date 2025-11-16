using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArrowLineMaker : MonoSingleton<CardArrowLineMaker>
{
    [SerializeField]
    private Transform[] _targets = new Transform[2];
    public Transform[] Targets => _targets;
    private Vector3 _middlePoint;
    private LineRenderer _arrowLine;

    [SerializeField]
    private float _liftFactor = 0.2f;

    // min line count 
    [Min(8)]
    public int LineCount = 8;


    private bool _isDragging = false;

    private readonly List<Vector3> _points = new List<Vector3>(64);

    private Vector3[] _positionsBuffer;
    void Start()
    {
        _arrowLine = GetComponent<LineRenderer>();
        _positionsBuffer = new Vector3[128]; // max point

        _arrowLine.sortingLayerName = "Default"; // 혹은 별도의 Layer
        _arrowLine.sortingOrder = 100; // 큰 숫자일수록 앞
    }
    void Update()
    {
        if (_isDragging)
        {
            _middlePoint = CalculateMiddlePointVertex();
            DrawLine();
        }
    }

    // 베지에 곡선 중점 찾아서 반환하는 함수
    private Vector3 CalculateMiddlePointVertex()
    {
        _middlePoint = Vector3.Lerp(_targets[0].position, _targets[1].position, 0.5f);
        float Lift = Mathf.Max(Vector3.Distance(_targets[0].position, _targets[1].position) * _liftFactor, _targets[1].position.y);

        _middlePoint.y += Lift;

        return _middlePoint;
    }

    // 곡선을 그리는 함수
    private void DrawLine()
    {
        _points.Clear();
        float step = 1f / (LineCount - 1);
        for (float t = 0; t <= 1f; t += step)
        {
            Vector3 tangent1 = Vector3.Lerp(_targets[0].position, _middlePoint, t);
            Vector3 tangent2 = Vector3.Lerp(_middlePoint, _targets[1].position, t);
            Vector3 curve = Vector3.Lerp(tangent1, tangent2, t);

            _points.Add(curve);
        }

        _points.CopyTo(_positionsBuffer);

        _arrowLine.positionCount = _points.Count;
        _arrowLine.SetPositions(_positionsBuffer);
    }

    // 마우스로 누른곳에 해당 태그 객체가 있는지 bool로 확인 return
    public bool IsPointerOverUI(string tag)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
                return true;
        }

        return false;
    }

    /// <summary>
    /// set linedrawer start point == selected card
    /// </summary>
    public void SetStartPoint(Vector3 cardPoint)
    {
        _targets[0].position = cardPoint;
    }

    /// <summary>
    /// set end point to mouse point
    /// </summary>
    /// <param name="mousePoint"></param>
    public void SetEndPoint(Vector3 mousePoint)
    {
        _targets[1].position = mousePoint;
    }

    public void SetIsDragging(bool value)
    {
        _isDragging = value;
    }

    public void ActiveLineDrawer(bool value)
    {
        _arrowLine.enabled = value;
    }
}