using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private List<BaseUI> preloadUIList; 
    private readonly Dictionary<string, BaseUI> _uiDict = new();
    private const string PATH = "Prefabs/UI/";

    private void Start()
    {
        preloadUIList.ForEach(x => ShowUI(x.name));
    }

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
            ui.Show(args);
        }
    }

    public void HideUI(string key, params object[] args)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Hide(args);
        }
    }

    public void HideAllUI(params string[] exceptions)
    {
        foreach (var itm in _uiDict)
        {
            if (exceptions.Contains(itm.Key)) continue;
            itm.Value.Hide();
        }
    }
}

public static class UIController
{
    public static void Open<T>(params object[] args) where T : BaseUI
    {
        UIManager.Instance.ShowUI(typeof(T).Name, args);
    }

    public static void Close<T>(params object[] args) where T : BaseUI
    {
        UIManager.Instance.HideUI(typeof(T).Name, args);
    }

    public static T Get<T>() where T : BaseUI
    {
        return UIManager.Instance.GetUI<T>(typeof(T).Name);
    }
}
