using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LineUpUI : BaseUI
{
    [SerializeField] private GameObject[] views;
    [SerializeField] private LineUpSlot[] slots;
    [SerializeField] private LineUpAura[] raceAuras;
    [SerializeField] private LineUpAura[] elementAuras;
    [SerializeField] private LineUpDetail detail;

    private int curView;
    private Dictionary<Race, int> raceCount = new();
    private Dictionary<Element, int> elementCount = new();
    
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(LineUpUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(LineUpUI));
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
            slots[i].Init(readyHeroList[i], (slotId, saveData) =>
                {
                    RefreshDetailView(slotId, saveData);
                    SwitchView(1);
                });
        }

        GetRaceAura(readyHeroList);
        GetElementAura(readyHeroList);

        string auraStatistics = "Aura statistics: ";
        foreach (var kv in raceCount) auraStatistics += $"\n{kv.Key} x {kv.Value}";
        foreach (var kv in elementCount) auraStatistics += $"\n{kv.Key} x {kv.Value}";
        EditorLog.Message(auraStatistics);
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

    private void GetRaceAura(List<HeroSaveData> heroList)
    {
        var raceList = heroList.Where(x => x != null).Select(x => x.GetHeroWithID().race).ToList();
        
        raceCount = raceList.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        int index = 0;
        foreach (var kv in raceCount)
        {
            if (kv.Value < 3) continue;
            raceAuras[index].gameObject.SetActive(true);
            raceAuras[index].Init(kv.Key, kv.Value);
            index++;
        }

        while (index < raceAuras.Length)
        {
            raceAuras[index++].gameObject.SetActive(false);
        }
    }
    
    private void GetElementAura(List<HeroSaveData> heroList)
    {
        var elementList = heroList.Where(x => x != null).Select(x => x.GetHeroWithID().element).ToList();
        
        elementCount = elementList.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());
        
        int index = 0;
        foreach (var kv in elementCount)
        {
            if (kv.Value < 2) continue;
            elementAuras[index].gameObject.SetActive(true);
            elementAuras[index].Init(kv.Key, kv.Value);
            index++;
        }

        while (index < elementAuras.Length)
        {
            elementAuras[index++].gameObject.SetActive(false);
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
