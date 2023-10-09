using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

public class EntityDatabase : DuztineBehaviour
{
    public List<EntityRecord> entities = new();

    public void Init(string data)
    {
        var jArray = JArray.Parse(data);
        foreach (var jToken in jArray)
        {
            var jObjectEntity = (JObject)jToken;
            var entityRcd = new EntityRecord
            {
                id = (string)jObjectEntity["id"],
                canUnlock = (bool)jObjectEntity["canUnlock"],
                name = (string)jObjectEntity["name"],
                alias = (string)jObjectEntity["alias"],
                story = (string)jObjectEntity["story"],
                tier = Enum.Parse<Tier>((string)jObjectEntity["tier"]),
                role = Enum.Parse<Role>((string)jObjectEntity["role"]),
                realm = Enum.Parse<Realm>((string)jObjectEntity["realm"]),
                damageType = Enum.Parse<DamageType>((string)jObjectEntity["damageType"]),
                attackRange = Enum.Parse<AttackRange>((string)jObjectEntity["attackRange"])
            };

            var jObjectStats = (JObject)jObjectEntity["stats"];
            if (jObjectStats != null)
            {
                var stats = new Stats
                {
                    health = Common.Parse<int>((string)jObjectStats["health"]),
                    damage = Common.Parse<int>((string)jObjectStats["damage"]),
                    armor = Common.Parse<int>((string)jObjectStats["armor"]),
                    resistance = Common.Parse<int>((string)jObjectStats["resistance"]),
                    intelligence = Common.Parse<int>((string)jObjectStats["intelligence"]),
                    speed = Common.Parse<int>((string)jObjectStats["speed"]),
                    luck = Common.Parse<int>((string)jObjectStats["luck"]),
                    critDamage = Common.Parse<int>((string)jObjectStats["critDamage"]),
                    lifeSteal = Common.Parse<int>((string)jObjectStats["lifeSteal"]),
                    accuracy = Common.Parse<int>((string)jObjectStats["accuracy"])
                };

                entityRcd.stats = stats;
            }

            entities.Add(entityRcd);
        }
    }

    public List<EntityData> GetAllEntities()
    {
        return null;
        // return entities.Where(x => x.canUnlock).ToList();
    }
    
    public EntityData GetEntityWithID(string id)
    {
        return null;
        // return entities.Find(x => x.info.id.Equals(id));
    }

    /*
    public List<EntityData> GetEntitiesWithConditions(params object[] conditions)
    {
        var raceOpts = new List<Realm>();
        var elementOpts = new List<Role>();
        var tierOpts = new List<Tier>();

        foreach (var condition in conditions)
        {
            switch (condition)
            {
                case Realm race:
                    raceOpts.Add(race);
                    break;
                case Role element:
                    elementOpts.Add(element);
                    break;
                case Tier tier:
                    tierOpts.Add(tier);
                    break;
            }
        }

        return entities.Where(x =>
            (
                (raceOpts.Contains(x.info.realm) || raceOpts.Count < 1) && // race filter
                (elementOpts.Contains(x.info.role) || elementOpts.Count < 1) && // element filter
                (tierOpts.Contains(x.info.tier) || tierOpts.Count < 1) // tier filter
            ))
            .ToList();
    }
    */
}

[Serializable]
public struct EntityRecord
{
    public string id;
    public bool canUnlock;
    public string name;
    public string alias;
    public string story;
    public Tier tier;
    public Role role;
    public Realm realm;
    public DamageType damageType;
    public AttackRange attackRange;

    [FoldoutGroup("Stats")]
    [HideLabel]
    public Stats stats;
} 