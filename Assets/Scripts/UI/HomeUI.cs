using System;
using TMPro;
using UnityEngine;

public class HomeUI : BaseUI
{
    [SerializeField] private TMP_Text fpsTxt;
    [SerializeField] private float logInterval = 0.5f;

    private float curTime = 0;
    private int frameCount = 0;
    
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(HomeUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(HomeUI));
    }

    private void Update()
    {
        curTime += Time.deltaTime;
        frameCount++;
        if (curTime >= logInterval)
        {
            int fps = Mathf.RoundToInt(frameCount / curTime);
            fpsTxt.text = "FPS: " + fps;
            curTime -= logInterval;
            frameCount = 0;
        }
    }

    public void OpenValhalla()
    {
        ValhallaUI.Show();
    }

    public void OpenLineUp()
    {
        LineUpUI.Show();
    }

    public void OpenQuest()
    {
        QuestUI.Show();
    }

    public void OpenInventory()
    {
        InventoryUI.Show();
    }
}