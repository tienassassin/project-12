using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBonusLegend", menuName = "Legend/LevelBonus")]
public class LevelBonusLegend : ScriptableDatabase
{
    [TableList]
    public List<CharacterGrowth> chrGrowthList = new();

    [TableList]
    public List<EquipmentGrowth> eqmGrowthList = new();

    public override void Import(params string[] data)
    {
        GUIUtility.systemCopyBuffer = data[0] + "\n" + data[1];
        
        chrGrowthList = new List<CharacterGrowth>();
        eqmGrowthList = new List<EquipmentGrowth>();

        var jArrayChar = JArray.Parse(data[0]);
        foreach (var jToken in jArrayChar)
        {
            ConvertDataFromJObject((JObject)jToken, out CharacterGrowth c);
            chrGrowthList.Add(c);
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
        chrGrowthList.Clear();
        eqmGrowthList.Clear();
    }

    private void ConvertDataFromJObject(JObject jObject, out CharacterGrowth c)
    {
        Enum.TryParse((string)jObject["tier"], out Tier tier);
        
        c = new CharacterGrowth
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
public struct CharacterGrowth
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
