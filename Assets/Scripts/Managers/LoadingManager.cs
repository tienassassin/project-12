using System;
using System.Collections;
using DB.System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingManager : Singleton<LoadingManager>
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMP_Text progressTxt;

    private Func<bool> _wait;

    protected override void Awake()
    {
        base.Awake();

        SceneManager.sceneLoaded += (_, _) =>
        {
            ResetCanvas();
        };
    }

    private void Start()
    {
        StartGame();
    }

    [Button]
    private void StartGame()
    {
        LoadScene(SceneName.HOME_SCENE, () => DataManager.Instance.EverythingLoaded);
    }

    private void ResetCanvas()
    {
        EditorLog.Message("Loading canvas: reset camera");
        loadingPanel.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    
    /// <summary>
    /// Load scene async
    /// </summary>
    /// <param name="sceneName">Target scene (use SceneName.&lt;scene&gt;)</param>
    /// <param name="wait">
    /// Condition needs to be met before scene is activated&#xA;
    /// <b>[WARNING]</b> Invalid condition can lead to <b>INFINITY</b> wait
    /// </param>
    public void LoadScene(string sceneName, Func<bool> wait = null)
    {
        _wait = wait;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float breakPoint = Random.Range(0.8f, 0.99f);
        DOVirtual.Float(0f, breakPoint, 2f, value =>
        {
            loadingBar.value = value;
            progressTxt.text = $"{value * 100:F2}%";
        }).OnComplete(() =>
        {
            asyncLoad.allowSceneActivation = true;
        });
        
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (_wait != null) yield return new WaitUntil(_wait);
        
        DOVirtual.Float(breakPoint, 1f, 1f, value =>
        {
            loadingBar.value = value;
            progressTxt.text = $"{value * 100:F2}%";
        }).OnComplete(() =>
        {
            loadingPanel.SetActive(false);
            this.PostEvent(EventID.ON_BATTLE_SCENE_LOADED);
        });
    }
}

public static class SceneName
{
    public const string HOME_SCENE = "HomeScene";
    public const string BATTLE_SCENE = "BattleScene";
}