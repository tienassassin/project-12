using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LineUpUI : BaseUI
{
    [SerializeField] private GameObject[] views;
    [SerializeField] private LineUpSlot[] slots;
    [SerializeField] private LineUpAura[] raceAuras;
    [SerializeField] private LineUpAura[] elementAuras;
    [SerializeField] private LineUpDetail detail;

    private int _curView;
    private Dictionary<Role, int> _elementCountDict = new();
    private Dictionary<Realm, int> _raceCountDict = new();

    private void OnEnable()
    {
        RefreshMainView();
        SwitchView(0);
    }

    public static void Show()
    {
        UIContainer.Instance.ShowUI(nameof(LineUpUI));
    }

    public static void Hide()
    {
        UIContainer.Instance.HideUI(nameof(LineUpUI));
    }

    private void RefreshMainView()
    {
        var readyHeroList = PlayerManager.Instance.GetReadyHeroes();
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
        foreach (var kv in _raceCountDict) auraStatistics += $"\n{kv.Key} x {kv.Value}";
        foreach (var kv in _elementCountDict) auraStatistics += $"\n{kv.Key} x {kv.Value}";
        DebugLog.Message(auraStatistics);
    }

    private void RefreshDetailView(int slotId, MyEntity saveData)
    {
        detail.Init(slotId, saveData);
    }

    public void SwitchView(int index)
    {
        index = Mathf.Clamp(index, 0, views.Length - 1);
        _curView = index;

        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == index);
        }
    }

    private void GetRaceAura(List<MyEntity> heroList)
    {
        var raceList = heroList.Where(x => x != null).Select(x => x.GetEntity().info.realm).ToList();

        _raceCountDict = raceList.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        int index = 0;
        foreach (var kv in _raceCountDict)
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

    private void GetElementAura(List<MyEntity> heroList)
    {
        var elementList = heroList.Where(x => x != null).Select(x => x.GetEntity().info.role).ToList();

        _elementCountDict = elementList.GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        int index = 0;
        foreach (var kv in _elementCountDict)
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
        switch (_curView)
        {
            case 0:
                Hide();
                break;
            case 1:
                RefreshMainView();
                SwitchView(0);
                break;
        }
    }
}