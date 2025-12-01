using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections; // IEnumerator 사용을 위해 명시

public class SceneController : MonoSingleton<SceneController>
{
    private GameObject LoadingUI;
    private CanvasGroup loadingGroup;

    void Start()
    {
        if (transform.childCount > 0)
        {
            LoadingUI = transform.GetChild(0).gameObject;
            loadingGroup = LoadingUI.GetComponent<CanvasGroup>();
            if (loadingGroup == null)
                loadingGroup = LoadingUI.AddComponent<CanvasGroup>();

            LoadingUI.SetActive(false);
            loadingGroup.alpha = 0;
        }
    }

    public void SceneMove(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        LoadingUI.SetActive(true);
        loadingGroup.alpha = 0;
        yield return loadingGroup.DOFade(1f, 0.5f).WaitForCompletion();

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        
        while (op.progress < 0.9f || timer < 2.0f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime; 
        }

        op.allowSceneActivation = true;

        yield return null; 

        loadingGroup.DOFade(0f, 0.5f).OnComplete(() =>
        {
            LoadingUI.SetActive(false);
        });
    }
}