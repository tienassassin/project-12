using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GlobalUI : Singleton<GlobalUI>
{
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text txtNotiContent;

    [SerializeField] private GameObject loading;
    [SerializeField] private Slider sldProgress;

    private Coroutine _notiCoroutine;

    public void ShowNotification(string content)
    {
        txtNotiContent.text = content;
        notification.SetActive(true);
        if (_notiCoroutine != null) StopCoroutine(_notiCoroutine);
        _notiCoroutine = StartCoroutine(HideNotiRoutine());
    }

    private IEnumerator HideNotiRoutine()
    {
        yield return new WaitForSeconds(2);
        notification.SetActive(false);
    }

    public void ShowLoading()
    {
        loading.SetActive(true);
        sldProgress.value = 0;
        sldProgress.DOValue(Random.Range(0.1f, 0.9f), 1f);
    }

    public void FinishLoading(Action finish = null)
    {
        sldProgress.DOKill();
        sldProgress.DOValue(1f, 1f).OnComplete(() =>
        {
            loading.SetActive(false);
            finish?.Invoke();
        });
    }
}