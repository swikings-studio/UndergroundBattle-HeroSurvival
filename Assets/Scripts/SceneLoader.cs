using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    private void Start()
    {
        StartCoroutine(LoadSceneAsync("Game"));
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!loadingOperation.isDone)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            yield return null;
        }

    }
}