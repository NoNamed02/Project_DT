using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneController : MonoSingleton<SceneController>
{
    private GameObject LoadingUI;
    private CanvasGroup loadingGroup;

    void Start()
    {
        LoadingUI = transform.GetChild(0).gameObject;

        // CanvasGroup 캐싱
        loadingGroup = LoadingUI.GetComponent<CanvasGroup>();
        if (loadingGroup == null)
            loadingGroup = LoadingUI.AddComponent<CanvasGroup>();

        LoadingUI.SetActive(false);
        loadingGroup.alpha = 0;
    }

    public void SceneMove(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private System.Collections.IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 로딩 UI 활성화
        LoadingUI.SetActive(true);

        // 페이드인 (0 → 1)
        loadingGroup.alpha = 0;
        loadingGroup.DOFade(1f, 0.5f);   // 필요하면 시간 조절

        // 씬 비동기 로드 시작
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 로딩 진행률 대기
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 충분히 로딩된 시점 → 씬 이동
        op.allowSceneActivation = true;
        LoadingUI.SetActive(false);
    }
}
