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
        
        Enum.TryParse((string)jObject["tier"], out Tier tier);
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

    
}

[Serializable]
public class BaseCharacter
{
    [TableColumnWidth(40, Resizable = false)] 
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
