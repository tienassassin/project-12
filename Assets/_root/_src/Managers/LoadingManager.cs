using System;
using System.Collections;
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

    private Action _sceneLoaded;

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
        LoadScene(SceneName.HOME_SCENE);
    }

    private void ResetCanvas()
    {
        EditorLog.Message("Loading canvas: reset camera");
        loadingPanel.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    
    public void LoadScene(string sceneName, Action sceneLoaded = null)
    {
        _sceneLoaded = sceneLoaded;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        float breakPoint = Random.Range(0.5f, 0.9f);
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

        DOVirtual.Float(breakPoint, 1f, 1f, value =>
        {
            loadingBar.value = value;
            progressTxt.text = $"{value * 100:F2}%";
        }).OnComplete(() =>
        {
            loadingPanel.SetActive(false);
            _sceneLoaded?.Invoke();
        });
    }
}

public static class SceneName
{
    public const string HOME_SCENE = "HomeScene";
    public const string BATTLE_SCENE = "BattleScene";
}