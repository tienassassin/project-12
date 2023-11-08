using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIContainer : Singleton<UIContainer>
{
    private readonly Dictionary<string, BaseUI> _uiDict = new();
    private const string PATH = "Prefabs/UI/";

    public BaseUI GetUI(string key)
    {
        if (!_uiDict.ContainsKey(key))
        {
            var uiPref = Resources.Load<BaseUI>(PATH + key);
            if (uiPref)
            {
                var newUI = Instantiate(uiPref, transform);
                newUI.name = key;
                _uiDict.Add(newUI.name, newUI);
            }
            else
            {
                EditorLog.Error($"UI {key} is undefined!");
            } 
        }

        _uiDict.TryGetValue(key, out var ui);
        return ui;
    }

    public T GetUI<T>(string key) where T : BaseUI
    {
        return (T)GetUI(key);
    }

    public void ShowUI(string key, params object[] args)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Open(args);
        }
    }

    public void HideUI(string key, params object[] args)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Close(args);
        }
    }

    public void HideAllUI(params string[] exceptions)
    {
        foreach (var itm in _uiDict)
        {
            if (exceptions.Contains(itm.Key)) continue;
            itm.Value.Close();
        }
    }
}

public static class UIManager
{
    public static void Open<T>(params object[] args) where T : BaseUI
    {
        UIContainer.Instance.ShowUI(typeof(T).Name, args);
    }

    public static void Close<T>(params object[] args) where T : BaseUI
    {
        UIContainer.Instance.HideUI(typeof(T).Name, args);
    }

    public static T Get<T>() where T : BaseUI
    {
        return UIContainer.Instance.GetUI<T>(typeof(T).Name);
    }
}
