using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    enum StatPoint
    {
        Health,
        Defence,
        Speed,
        DrawCount
    }
    
    [SerializeField]
    private List<TextMeshProUGUI> _statCountText;
    [SerializeField]
    private List<Button> _statUpBtn;

    void Start()
    {
        _statUpBtn[(int)StatPoint.Health].onClick.AddListener(() =>
            RunTimeData.Instance.MaxHP += 5
        );
        _statUpBtn[(int)StatPoint.Defence].onClick.AddListener(() =>
            RunTimeData.Instance.Defence += 2
        );
        _statUpBtn[(int)StatPoint.Speed].onClick.AddListener(() =>
            RunTimeData.Instance.Speed += 3
        );
        _statUpBtn[(int)StatPoint.DrawCount].onClick.AddListener(() =>
            RunTimeData.Instance.DrawCount += 1
        );
    }
    void Update()
    {
        _statCountText[(int)StatPoint.Health].text = RunTimeData.Instance.MaxHP.ToString();
        _statCountText[(int)StatPoint.Defence].text = RunTimeData.Instance.Defence.ToString();
        _statCountText[(int)StatPoint.Speed].text = RunTimeData.Instance.Speed.ToString();
        _statCountText[(int)StatPoint.DrawCount].text = RunTimeData.Instance.DrawCount.ToString();

        foreach (var btn in _statUpBtn)
        {
            btn.interactable = RunTimeData.Instance.StatPoint > 0;
        }
    }
}
