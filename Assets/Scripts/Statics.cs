using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Get random result (true/false) from 0-100(%) rate
    /// </summary>
    public static bool GetRandomResult(float rate)
    {
        float num = Random.Range(0f,100f);
        return num <= rate;
    }
}

public static class StaticData
{
    private static Dictionary<string, string> statDescriptionDic = new ()
    {
        ["health"] = "Survivability. When it reaches 0, you will die.",
        ["pDmg"] = "Ability to deal physical damage. Enemy's health will be affected",
        ["mDmg"] = "Ability to deal magical damage. Enemy's health will be affected",
        ["eDmg"] = "Ability to deal elemental damage. Enemy's endurance will be affected",
        ["armor"] = "Ability to reduce physical damage taken. Minimum damage after reduction is 1.",
        ["magicResistance"] = "Ability to reduce magical damage taken. Minimum damage after reduction is 1.",
        ["energyRegen"] = "Ability to regen energy. When the energy is full, you can use the ultimate skill. (max: 100)",
        
        ["speed"] = "Decide the order and frequency of your turns. (max: 100)",
        ["critRate"] = "(%) Your chance of dealing a critical damage. (max: 100)",
        ["critDmg"] = "(%) Damage multiplier when you deal critical damage",
        
        ["lifeSteal"] = "(%) Amount of damage converted to health when you deal damage.",
        ["armorPenetration"] = "(%) The enemy's armor is ignored when you deal physical damage.",
        ["mRPenetration"] = "(%) The enemy's magic resistance is ignored when you deal magical damage.",
    };

    public static string GetStatDescription(string stat)
    {
        if (statDescriptionDic.ContainsKey(stat)) return statDescriptionDic[stat];
        Debug.LogError($"Stat {stat} is not exist!!!");
        return "";
    }
}