using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowthDatabase", menuName = "Database/Growth")]
public class GrowthDatabase : ScriptableDatabase
{
    [TableList]
    public List<HeroGrowth> heroGrowthList = new();

    [TableList]
    public List<EquipmentGrowth> eqmGrowthList = new();

    public override void Import(params string[] data)
    {
        heroGrowthList = new List<HeroGrowth>();
        eqmGrowthList = new List<EquipmentGrowth>();

        var jArrayChar = JArray.Parse(data[0]);
        foreach (var jToken in jArrayChar)
        {
            ConvertDataFromJObject((JObject)jToken, out HeroGrowth h);
            heroGrowthList.Add(h);
        }
        
        var jArrayEqm = JArray.Parse(data[1]);
        foreach (var jToken in jArrayEqm)
        {
            ConvertDataFromJObject((JObject)jToken, out EquipmentGrowth e);
            eqmGrowthList.Add(e);
        }
    }

    [Button]
    protected override void DeleteAll()
    {
        heroGrowthList.Clear();
        eqmGrowthList.Clear();
    }

    private void ConvertDataFromJObject(JObject jObject, out HeroGrowth h)
    {
        Enum.TryParse((string)jObject["tier"], out Tier tier);
        
        h = new HeroGrowth
        {
            tier = tier,
            growth = Utils.Parse<float>((string)jObject["growth(%)"]) / 100f,
        };
    }
    
    private void ConvertDataFromJObject(JObject jObject, out EquipmentGrowth e)
    {
        Enum.TryParse((string)jObject["rarity"], out Rarity rarity);
        
        e = new EquipmentGrowth
        {
            rarity = rarity,
            growth = Utils.Parse<float>((string)jObject["growth(%)"]) / 100f,
        };
    }
}

[Serializable]
public struct HeroGrowth
{
    public Tier tier;
    public float growth;
}

[Serializable]
public struct EquipmentGrowth
{
    public Rarity rarity;
    public float growth;
}
