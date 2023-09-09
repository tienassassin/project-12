using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowthDatabase", menuName = "DB/GrowthDatabase")]
public class GrowthDatabase : ScriptableDatabase
{
    [TableList]
    public List<EntityGrowth> entityGrowths = new();

    [TableList]
    public List<EquipmentGrowth> equipmentGrowths = new();

    public override void Import()
    {
    }

    public override void Delete()
    {
        entityGrowths.Clear();
        equipmentGrowths.Clear();
    }

    public float GetGrowth(object obj)
    {
        return obj switch
        {
            Tier t => entityGrowths.Find(x => x.tier == t).growth,
            Rarity r => equipmentGrowths.Find(x => x.rarity == r).growth,
            _ => 0
        };
    }
}

[Serializable]
public struct EntityGrowth
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