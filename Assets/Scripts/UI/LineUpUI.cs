using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpUI : BaseUI
{
    [SerializeField] private GameObject[] views;
    [SerializeField] private LineUpSlot[] slots;
    [SerializeField] private LineUpDetail detail;

    private int curView;
    
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(LineUpUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(LineUpUI));
    }

    protected override void Awake()
    {
        base.Awake();
        
        
    }

    private void OnEnable()
    {
        RefreshMainView();
        SwitchView(0);
    }

    private void RefreshMainView()
    {
        var readyHeroList = UserManager.Instance.GetReadyHeroes();
        for (int i = 0; i < readyHeroList.Count; i++)
        {
            slots[i].Init(readyHeroList[i]);
            slots[i].OnShowSlotDetail += (slotId, saveData) =>
            {
                RefreshDetailView(slotId, saveData);
                SwitchView(1);
            };
        }
    }

    private void RefreshDetailView(int slotId, HeroSaveData saveData)
    {
        detail.Init(slotId, saveData);
    }

    public void SwitchView(int index)
    {
        index = Mathf.Clamp(index, 0, views.Length - 1);
        curView = index;
        
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == index);
        }
    }

    public void OnClickBack()
    {
        switch (curView)
        {
            case 0:
                LineUpUI.Hide();
                break;
            case 1:
                RefreshMainView();
                SwitchView(0);
                break;
        }

    }
}
