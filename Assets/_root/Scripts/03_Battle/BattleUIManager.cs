using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleUIManager : Singleton<BattleUIManager>
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
                DebugLog.Error($"UI {key} is undefined!");
            }
        }

        _uiDict.TryGetValue(key, out var ui);
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
            ui.Open(pars);
        }
    }

    public void HideUI(string key, params object[] pars)
    {
        var ui = GetUI(key);
        if (ui)
        {
            ui.Close(pars);
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