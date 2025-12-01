using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DarkTonic.MasterAudio;

public class MainUIController : MonoBehaviour
{
    public float duration = 0.3f;
    [SerializeField]
    private GameObject _mainUI;
    [SerializeField]
    List<GameObject> _initUI;
    void Start()
    {
        Init();
        _mainUI.GetComponent<CanvasGroup>().alpha = 0f;
        _mainUI.GetComponent<CanvasGroup>().DOFade(3f, duration).SetUpdate(true);
    }

    void Init()
    {
        foreach(var UI in _initUI)
        {
            UI.SetActive(false);
        }
    }

    public void OnEnableUI(GameObject UI)
    {
        MasterAudio.PlaySound("fnt_ui_use_metal");
        UI.gameObject.SetActive(true);
        UI.GetComponent<CanvasGroup>().alpha = 0f;
        UI.GetComponent<CanvasGroup>().DOFade(1f, duration).SetUpdate(true);
    }

    public void FadeOutAndDisable(GameObject UI)
    {
        MasterAudio.PlaySound("fnt_ui_use_metal");
        UI.GetComponent<CanvasGroup>().DOFade(0f, duration).SetUpdate(true)
            .OnComplete(() => UI.gameObject.SetActive(false));
    }
}
