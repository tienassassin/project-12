using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroDatabase",menuName = "Database/Hero")]
public class HeroDatabase : ScriptableDatabase
{
    [TableList]
    public List<BaseHero> heroList = new();

    private Dictionary<string, BaseHero> cachedDict = new();

    public override void Import(params string[] data)
    {
        heroList = new List<BaseHero>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var h);
            if (h != null) heroList.Add(h);
        }
    }
    
    [Button]
    protected override void DeleteAll()
    {
        heroList.Clear();
    } 

    private void ConvertDataFromJObject(JObject jObject, out BaseHero h)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            h = null;
            return;
        }

        string tierValue = Utils.GetNormalizedString((string)jObject["tier"]);
        
        Enum.TryParse(tierValue, out Tier tier);
        Enum.TryParse((string)jObject["element"], out Element element);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
        Enum.TryParse((string)jObject["range"], out Range range);
        
        h = new BaseHero
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

    public BaseHero GetHeroWithID(string heroId)
    {
        cachedDict.TryAdd(heroId, heroList.Find(h => h.id == heroId));
        if (cachedDict[heroId] == null) EditorLog.Error($"Character {heroId} is not defined");
        return cachedDict[heroId];
    }

    public List<BaseHero> GetHeroesWithConditions(params object[] conditions)
    {
        var matchHeroesList = new List<BaseHero>();
        
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

        heroList.ForEach(h =>
        {
            if ((raceOptList.Contains(h.race) || acpAllRace)
                && (elementOptList.Contains(h.element) || acpAllElement)
                && (tierOptList.Contains(h.tier) || acpAllTier))
            {
                matchHeroesList.Add(h);
            }
        });
        
        return matchHeroesList;
    }
}

[Serializable]
public class BaseHero
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
