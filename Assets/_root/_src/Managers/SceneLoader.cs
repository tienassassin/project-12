using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private string _sceneName;
    private float _expectedTime;
    private Action _sceneLoaded;

    public void LoadScene(string sceneName, float expectedTime = -1f, Action sceneLoaded = null)
    {
        _sceneName = sceneName;
        _expectedTime = expectedTime;
        _sceneLoaded = sceneLoaded;
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        var asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
        GlobalUI.Instance.ShowLoading();
        float elapsedTime = 0;

        while (!asyncLoad.isDone || elapsedTime < _expectedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        GlobalUI.Instance.FinishLoading(_sceneLoaded);
    }
}

public static class SceneName
{
    public const string LOGIN = "0_Login";
    public const string HOME = "1_Home";
    public const string BATTLE = "2_Battle";
}