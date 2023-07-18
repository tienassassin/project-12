using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<BaseUI> preloadUIList; 
    private Dictionary<string, BaseUI> UIDict = new();
    private string uiPath = "Prefabs/UI/";

    private void Start()
    {
        preloadUIList.ForEach(x => ShowUI(x.name));
    }

    public BaseUI GetUI(string key)
    {
        if (!UIDict.ContainsKey(key))
        {
            var uiPref = Resources.Load<BaseUI>(uiPath + key);
            if (uiPref)
            {
                var newUI = Instantiate(uiPref, transform);
                newUI.name = key;
                UIDict.Add(newUI.name, newUI);
            }
            else
            {
                EditorLog.Error($"UI {key} is undefined!");
            } 
        }

        UIDict.TryGetValue(key, out var ui);
        return ui;
    }

    public T GetUI<T>(string key) where T : BaseUI
    {
        return (T)GetUI(key);
    }

    public void ShowUI(string key, params object[] pars)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Show(pars);
        }
    }

    public void HideUI(string key, params object[] pars)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Hide(pars);
        }
    }

    public void HideAllUI(params string[] exceptions)
    {
        foreach (var itm in UIDict)
        {
            if (exceptions.Contains(itm.Key)) continue;
            itm.Value.Hide();
        }
    }
}
