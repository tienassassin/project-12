using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValhallaUI : BaseUI
{
    [SerializeField] private FilterOption[] tierFilterOptions;
    [SerializeField] private FilterOption[] elementFilterOptions;
    [SerializeField] private FilterOption[] raceFilterOptions;
    
    private List<Tier> tierOptList = new();
    private List<Element> elementOptList = new();
    private List<Race> raceOptList = new();

    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(ValhallaUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(ValhallaUI));
    }

    protected override void Awake()
    {
        base.Awake();

        foreach (var opt in tierFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        foreach (var opt in elementFilterOptions)
        {
            opt.SetEvent((o) =>
            {
                opt.SetEvent(AddOptionToFilter);
            });
        }

        foreach (var opt in raceFilterOptions)
        {
            opt.SetEvent((o) =>
            {
                opt.SetEvent(AddOptionToFilter);
            });
        }
    }

    private void AddOptionToFilter(object o)
    {
        switch (o)
        {
            case Tier t when tierOptList.Contains(t):
                tierOptList.Remove(t);
                break;
            case Tier t:
                tierOptList.Add(t);
                break;
            case Element e when elementOptList.Contains(e):
                elementOptList.Remove(e);
                break;
            case Element e:
                elementOptList.Add(e);
                break;
            case Race r when raceOptList.Contains(r):
                raceOptList.Remove(r);
                break;
            case Race r:
                raceOptList.Add(r);
                break;
            default:
                Debug.LogError($"Object {o} is not a valid filter option");
                break;
        }
    }

    #region Buttons

    public void OnClickBack()
    {
        Debug.Log("back");
        ValhallaUI.Hide();
    }

    #endregion
}
