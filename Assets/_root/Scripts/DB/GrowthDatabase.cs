using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

public class GrowthDatabase : DuztineBehaviour
{
    [TableList]
    public List<EntityGrowth> entityGrowths = new();

    [TableList]
    public List<EquipmentGrowth> equipmentGrowths = new();

    public void Init(string data)
    {
        var jObjectData = JObject.Parse(data);
        var jArrayEntityGrowth = (JArray)jObjectData["entity"];
        var jArrayEqmGrowth = (JArray)jObjectData["equipment"];

        if (jArrayEntityGrowth != null)
        {
            foreach (var jToken in jArrayEntityGrowth)
            {
                var jObject = (JObject)jToken;
                entityGrowths.Add(new EntityGrowth
                {
                    tier = Enum.Parse<Tier>((string)jObject["tier"]),
                    growth = Utils.Parse<float>((string)jObject["growth"])
                });
            }
        }

        if (jArrayEqmGrowth != null)
        {
            foreach (var jToken in jArrayEqmGrowth)
            {
                var jObject = (JObject)jToken;
                equipmentGrowths.Add(new EquipmentGrowth
                {
                    rarity = Enum.Parse<Rarity>((string)jObject["rarity"]),
                    growth = Utils.Parse<float>((string)jObject["growth"])
                });
            }
        }
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