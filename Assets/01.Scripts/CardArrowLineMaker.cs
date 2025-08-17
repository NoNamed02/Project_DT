using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardArrowLineMaker : MonoBehaviour
{
    [SerializeField]
    private Transform[] _targets = new Transform[2];
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
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPointerOverUI("Card"))
        {
            _isDragging = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
        UpdateArrowEndPoint();
        _middlePoint = CalculateMiddlePointVertex();
        DrawLine();
    }

    private void UpdateArrowEndPoint()
    {
        if (_isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _targets[1].GetComponent<RectTransform>(),
                mousePos,
                Camera.main,
                out worldPos
            );

            _targets[1].position = worldPos;
        }
        else
        {
            _targets[1].position = _targets[0].position;
        }
    }
    private Vector3 CalculateMiddlePointVertex()
    {
        _middlePoint = Vector3.Lerp(_targets[0].position, _targets[1].position, 0.5f);
        float Lift = Mathf.Max(Vector3.Distance(_targets[0].position, _targets[1].position) * _liftFactor, _targets[1].position.y);

        _middlePoint.y += Lift;

        return _middlePoint;
    }
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
    private bool IsPointerOverUI(string tag)
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
}