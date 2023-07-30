using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class HomeUI : BaseUI
{
    [SerializeField] private TMP_Text fpsTxt;
    [SerializeField] private float logInterval = 0.5f;

    [SerializeField] private HeroAvatar[] avatars;

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

    private void OnEnable()
    {
        this.AddListener(EventID.ON_LINEUP_CHANGED, RefreshHeroAvatars);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_LINEUP_CHANGED, RefreshHeroAvatars);
    }

    private void Start()
    {
        RefreshHeroAvatars();
    }

    private void RefreshHeroAvatars(object _ = null)
    {
        var readyHeroList = UserManager.Instance.GetReadyHeroes().FindAll(x=>x != null);
        for (int i = 0; i < avatars.Length; i++)
        {
            if (i >= readyHeroList.Count)
            {
                avatars[i].gameObject.SetActive(false);
                continue;
            }

            avatars[i].gameObject.SetActive(true);
            avatars[i].Init(readyHeroList[i]);
        }
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