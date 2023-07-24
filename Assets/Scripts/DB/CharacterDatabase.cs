using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase",menuName = "Database/Characters")]
public class CharacterDatabase : ScriptableDatabase
{
    [TableList]
    public List<BaseCharacter> charList = new();

    private Dictionary<string, BaseCharacter> cachedDict = new();

    public override void Import(params string[] data)
    {
        charList = new List<BaseCharacter>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var c);
            if (c != null) charList.Add(c);
        }
    }
    
    [Button]
    protected override void DeleteAll()
    {
        charList.Clear();
    } 

    private void ConvertDataFromJObject(JObject jObject, out BaseCharacter c)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            c = null;
            return;
        }

        string tierValue = Utils.GetNormalizedString((string)jObject["tier"]);
        
        Enum.TryParse(tierValue, out Tier tier);
        Enum.TryParse((string)jObject["element"], out Element element);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
        Enum.TryParse((string)jObject["range"], out Range range);
        
        c = new BaseCharacter
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

    public BaseCharacter GetCharacterWithID(string charId)
    {
        cachedDict.TryAdd(charId, charList.Find(c => c.id == charId));
        if (cachedDict[charId] == null) EditorLog.Error($"Character {charId} is not defined");
        return cachedDict[charId];
    }

    public List<BaseCharacter> GetCharactersWithConditions(params object[] conditions)
    {
        var matchCharList = new List<BaseCharacter>();
        
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

        charList.ForEach(c =>
        {
            if ((raceOptList.Contains(c.race) || acpAllRace)
                && (elementOptList.Contains(c.element) || acpAllElement)
                && (tierOptList.Contains(c.tier) || acpAllTier))
            {
                matchCharList.Add(c);
            }
        });
        
        return matchCharList;
    }
}

[Serializable]
public class BaseCharacter
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
