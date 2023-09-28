using System.Collections;
using TMPro;
using UnityEngine;

public class GlobalUI : Singleton<GlobalUI>
{
    [SerializeField] private GameObject notification;
    [SerializeField] private TMP_Text txtNotiContent;

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
}