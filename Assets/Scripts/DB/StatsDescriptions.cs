using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsDescriptions", menuName = "Description/Stats")]
public class StatsDescriptions : ScriptableDatabase
{
    [TableList]
    public List<StatDesc> statList = new();

    private Dictionary<string, StatDesc> cachedDic = new();

    public override void Import(params string[] data)
    {
        statList = new List<StatDesc>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var s);
            statList.Add(s);
        }
    }
    
    [Button]
    protected override void DeleteAll()
    {
        statList.Clear();
    }
    
    private void ConvertDataFromJObject(JObject jObject, out StatDesc s)
    {
        s = new StatDesc();
        s.stat = (string)jObject["stat"];
        s.name = (string)jObject["name"];
        s.limit = Utils.Parse<float>((string)jObject["limit"]);
        s.description = (string)jObject["description"];
    }

    public StatDesc GetStatDescription(string stat)
    {
        cachedDic.TryAdd(stat, statList.Find(s => s.stat == stat));
        if (cachedDic[stat] == null) Debug.LogError($"Stat {stat} is not defined");
        return cachedDic[stat];
    }
    
    
}

[Serializable]
public class StatDesc
{
    [TableColumnWidth(100, Resizable = false)] 
    public string stat;
    
    [TableColumnWidth(100, Resizable = false)] 
    public string name;
    
    [TableColumnWidth(70, Resizable = false), ShowIf("@this.limit > 0")] 
    public float limit;
    
    [TextArea(3,10)] 
    public string description;
}

