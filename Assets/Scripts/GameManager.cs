using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public void LoadSceneAsync(int buildIndex)
    {
        StartCoroutine(LoadSceneAsyncByBuildIndex(buildIndex));
    }

    private IEnumerator LoadSceneAsyncByBuildIndex(int buildIndex)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(buildIndex);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}