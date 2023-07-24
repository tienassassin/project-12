using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "BackstoryDatabase", menuName = "Database/Backstory")]
public class BackstoryDatabase : ScriptableDatabase
{
    [TableList]
    public List<Backstory> storyList = new();

    private Dictionary<string, Backstory> cachedDict = new();

    public override void Import(params string[] data)
    {
        storyList = new List<Backstory>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var b);
            storyList.Add(b);
        }
    }

    [Button]
    protected override void DeleteAll()
    {
        storyList.Clear();
    }

    private void ConvertDataFromJObject(JObject jObject, out Backstory b)
    {
        b = new Backstory
        {
            id = (string)jObject["ID"],
            name = (string)jObject["name"],
            alias = (string)jObject["alias"],
            story = (string)jObject["story"]
        };
    }

    public Backstory GetBackstory(string charId)
    {
        cachedDict.TryAdd(charId, storyList.Find(s => s.id == charId));
        if (cachedDict[charId] == null) EditorLog.Error($"Backstory of {charId} is not defined");
        return cachedDict[charId];
    }
}

[Serializable]
public class Backstory
{
    [VerticalGroup("Information")]
    [TableColumnWidth(200, Resizable = false)] 
    public string id;
    
    [VerticalGroup("Information")] 
    public string name;

    [VerticalGroup("Information")]
    [Multiline(2)]
    public string alias;

    [VerticalGroup("Story")]
    [TextArea(7,10), HideLabel]
    public string story;
}