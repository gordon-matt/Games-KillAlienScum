using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressSceneLoader : MonoBehaviour
{
    public ProgressBar ProgressBar;

    private AsyncOperation operation;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>(true);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        ProgressBar.SetProgress(0f);
        canvas.gameObject.SetActive(true);

        StartCoroutine(BeginLoad(sceneName));
    }

    private IEnumerator BeginLoad(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            ProgressBar.SetProgress(operation.progress);
            yield return null;
        }

        ProgressBar.SetProgress(operation.progress);
        operation = null;
        canvas.gameObject.SetActive(false);
    }
}