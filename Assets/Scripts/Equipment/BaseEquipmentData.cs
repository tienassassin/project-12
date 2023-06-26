using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Equipment Data", menuName = "Equipment/Base Equipment Data")]
public class BaseEquipmentData : ScriptableObject
{
    public Slot type;
    public Race race;
    public Stats stats;
    public float levelBonus;
    public float raceBonus;
}

[Serializable]
public class EquipmentData
{
    public BaseEquipmentData baseData;
    public int level = 1;
    
    public Stats GetFullStats(Race ownerRace)
    {
        var stats = baseData.stats;
        for (int i = 0; i < level; i++)
        {
            stats *= (1 + baseData.levelBonus);
        }
        
        if (baseData.race == ownerRace)
        {
            return stats * (1 + baseData.raceBonus);
        }

        return stats;
    }
}