using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "DevilDatabase",menuName = "Database/Devil")]
public class DevilDatabase : ScriptableDatabase
{
    [TableList]
    public List<BaseDevil> devilList = new();

    private Dictionary<string, BaseDevil> cachedDict = new();

    public override void Import(params string[] data)
    {
        devilList = new List<BaseDevil>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var d);
            if (d != null) devilList.Add(d);
        }
    }
    
    [Button]
    protected override void DeleteAll()
    {
        devilList.Clear();
    } 

    private void ConvertDataFromJObject(JObject jObject, out BaseDevil d)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            d = null;
            return;
        }

        string tierValue = Utils.GetNormalizedString((string)jObject["tier"]);
        
        Enum.TryParse(tierValue, out Tier tier);
        Enum.TryParse((string)jObject["element"], out Element element);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
        Enum.TryParse((string)jObject["range"], out Range range);
        
        d = new BaseDevil
        {
            id = (string)jObject["ID"],
            name = (string)jObject["name"],
            tier = tier,
            element = element,
            race = race,
            damageType = dmgType,
            range = range,
            stats = new Stats
            {
                showFull = true,
                health = Utils.Parse<float>((string)jObject["health"]),
                damage = Utils.Parse<float>((string)jObject["damage"]),
                armor = Utils.Parse<float>((string)jObject["armor"]),
                resistance = Utils.Parse<float>((string)jObject["resistance"]),
                intelligence = Utils.Parse<float>((string)jObject["intelligence"]),
                speed = Utils.Parse<float>((string)jObject["speed"]),
                luck = Utils.Parse<float>((string)jObject["luck"]),
                critDamage = Utils.Parse<float>((string)jObject["crit damage"]),
                lifeSteal = Utils.Parse<float>((string)jObject["life steal"]),
                accuracy = Utils.Parse<float>((string)jObject["accuracy"]),
            }
        };
    }

    public BaseDevil GetDevilWithID(string devilId)
    {
        cachedDict.TryAdd(devilId, devilList.Find(d => d.id == devilId));
        if (cachedDict[devilId] == null) EditorLog.Error($"Devil {devilId} is not defined");
        return cachedDict[devilId];
    }

    public List<BaseDevil> GetDevilsWithConditions(params object[] conditions)
    {
        var matchDevilsList = new List<BaseDevil>();
        
        var raceOptList = new List<Race>();
        var elementOptList = new List<Element>();
        var tierOptList = new List<Tier>();
        bool acpAllRace = true;
        bool acpAllElement = true;
        bool acpAllTier = true;
        
        foreach (var condition in conditions)
        {
            switch (condition)
            {
                case Race race:
                    raceOptList.Add(race);
                    acpAllRace = false;
                    break;
                case Element element:
                    elementOptList.Add(element);
                    acpAllElement = false;
                    break;
                case Tier tier:
                    tierOptList.Add(tier);
                    acpAllTier = false;
                    break;
            }
        }

        devilList.ForEach(d =>
        {
            if ((raceOptList.Contains(d.race) || acpAllRace)
                && (elementOptList.Contains(d.element) || acpAllElement)
                && (tierOptList.Contains(d.tier) || acpAllTier))
            {
                matchDevilsList.Add(d);
            }
        });
        
        return matchDevilsList;
    }
}

[Serializable]
public class BaseDevil
{
    [VerticalGroup("Information")] 
    public string id;
    
    [VerticalGroup("Information")] 
    public string name;
    
    [VerticalGroup("Information")] 
    public Tier tier;
    
    [VerticalGroup("Information")] 
    public Element element;
    
    [VerticalGroup("Information")] 
    public Race race;
    
    [VerticalGroup("Information")] 
    public DamageType damageType;
    
    [VerticalGroup("Information")] 
    public Range range;
    
    public Stats stats;
}
