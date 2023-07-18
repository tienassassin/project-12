using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentDatabase",menuName = "Database/Equipments")]
public class EquipmentDatabase : ScriptableDatabase
{
    [TableList]
    public List<BaseEquipment> eqmList = new();
    
    public override void Import(params string[] data)
    {
        eqmList = new List<BaseEquipment>();
        var jArray = JArray.Parse(data[0]);
        foreach (var jToken in jArray)
        {
            ConvertDataFromJObject((JObject)jToken, out var e);
            if (e != null) eqmList.Add(e);
        }
    }

    [Button]
    protected override void DeleteAll()
    {
        eqmList.Clear();
    }
    
    private void ConvertDataFromJObject(JObject jObject, out BaseEquipment e)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            e = null;
            return;
        }

        string rarityValue = Utils.GetNormalizedString((string)jObject["rarity"]);
        
        Enum.TryParse(rarityValue, out Rarity rarity);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["slot"], out Slot slot);
        Enum.TryParse((string)jObject["requirement"], out Requirement req);

        e = new BaseEquipment();
        e.id = (string)jObject["ID"];
        e.name = (string)jObject["name"];
        e.set = (string)jObject["set"];
        e.rarity = rarity;
        e.slot = slot;
        e.requirement = req;
        e.race = race;
        e.raceBonus = Utils.Parse<float>((string)jObject["race bonus(%)"]) / 100f;
        e.stats = new Stats
        {
            showFull = false,
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
        };
    }
}

[Serializable]
public class BaseEquipment
{
    [VerticalGroup("Information")]
    public string id;
    
    [VerticalGroup("Information")] 
    public string name;
    
    [VerticalGroup("Information"), HideIf("@string.IsNullOrWhiteSpace(this.set)")] 
    public string set;
    
    [VerticalGroup("Information")] 
    public Rarity rarity;
    
    [VerticalGroup("Information")] 
    public Slot slot;
    
    [VerticalGroup("Information")] 
    public Requirement requirement;
    
    [VerticalGroup("Information")] 
    public Race race;
    
    [VerticalGroup("Information")] 
    public float raceBonus;
    
    public Stats stats;
}
