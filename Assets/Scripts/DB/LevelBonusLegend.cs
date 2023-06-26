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
    public List<CharacterLevelBonus> charLvlBonusList = new();

    [TableList]
    public List<EquipmentLevelBonus> eqmLvlBonusList = new();

    public override void Import(params string[] data)
    {
        GUIUtility.systemCopyBuffer = data[0] + "\n" + data[1];
        
        charLvlBonusList = new List<CharacterLevelBonus>();
        eqmLvlBonusList = new List<EquipmentLevelBonus>();

        var jArrayChar = JArray.Parse(data[0]);
        foreach (var jToken in jArrayChar)
        {
            ConvertDataFromJObject((JObject)jToken, out CharacterLevelBonus c);
            charLvlBonusList.Add(c);
        }
        
        var jArrayEqm = JArray.Parse(data[1]);
        foreach (var jToken in jArrayEqm)
        {
            ConvertDataFromJObject((JObject)jToken, out EquipmentLevelBonus e);
            eqmLvlBonusList.Add(e);
        }
    }

    [Button]
    protected override void DeleteAll()
    {
        charLvlBonusList.Clear();
        eqmLvlBonusList.Clear();
    }

    private void ConvertDataFromJObject(JObject jObject, out CharacterLevelBonus c)
    {
        Enum.TryParse((string)jObject["tier"], out Tier tier);
        
        c = new CharacterLevelBonus
        {
            tier = tier,
            bonus = Utils.Parse<float>((string)jObject["bonus"])
        };
    }
    
    private void ConvertDataFromJObject(JObject jObject, out EquipmentLevelBonus e)
    {
        Enum.TryParse((string)jObject["rarity"], out Rarity rarity);
        
        e = new EquipmentLevelBonus
        {
            rarity = rarity,
            bonus = Utils.Parse<float>((string)jObject["bonus"])
        };
    }
    
    
}

[Serializable]
public struct CharacterLevelBonus
{
    public Tier tier;
    public float bonus;
}

[Serializable]
public struct EquipmentLevelBonus
{
    public Rarity rarity;
    public float bonus;
}
