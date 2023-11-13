using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityDatabase : AssassinBehaviour
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
                    health = Utils.Parse<int>((string)jObjectStats["health"]),
                    damage = Utils.Parse<int>((string)jObjectStats["damage"]),
                    armor = Utils.Parse<int>((string)jObjectStats["armor"]),
                    resistance = Utils.Parse<int>((string)jObjectStats["resistance"]),
                    intelligence = Utils.Parse<int>((string)jObjectStats["intelligence"]),
                    speed = Utils.Parse<int>((string)jObjectStats["speed"]),
                    luck = Utils.Parse<int>((string)jObjectStats["luck"]),
                    critDamage = Utils.Parse<int>((string)jObjectStats["critDamage"]),
                    lifeSteal = Utils.Parse<int>((string)jObjectStats["lifeSteal"]),
                    accuracy = Utils.Parse<int>((string)jObjectStats["accuracy"])
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

[Serializable]
public struct Stats
{
    public int health;
    public int damage;
    public int armor;
    public int resistance;

    public int intelligence;
    public int speed;
    public int luck;
    public int critDamage;

    public int lifeSteal;
    public int accuracy;

    public Stats GetStatsByLevel(int level, float growth)
    {
        return new Stats
        {
            health = (int)(health * Mathf.Pow(1 + growth, level - 1)),
            damage = (int)(damage * Mathf.Pow(1 + growth, level - 1)),
            armor = (int)(armor * Mathf.Pow(1 + growth, level - 1)),
            resistance = (int)(resistance * Mathf.Pow(1 + growth, level - 1)),

            intelligence = intelligence,
            speed = speed,
            luck = luck,
            critDamage = critDamage,

            lifeSteal = lifeSteal,
            accuracy = accuracy
        };
    }

    public static Stats operator +(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health + st2.health,
            damage = st1.damage + st2.damage,
            armor = st1.armor + st2.armor,
            resistance = st1.resistance + st2.resistance,

            intelligence = Clamp(st1.intelligence + st2.intelligence, 100),
            speed = Clamp(st1.speed + st2.speed, 100),
            luck = Clamp(st1.luck + st2.luck, 100),
            critDamage = st1.critDamage + st2.critDamage,

            lifeSteal = st1.lifeSteal + st2.lifeSteal,
            accuracy = Clamp(st1.accuracy + st2.accuracy, 80)
        };
    }

    public static Stats operator -(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health - st2.health,
            damage = st1.damage - st2.damage,
            armor = st1.armor - st2.armor,
            resistance = st1.resistance - st2.resistance,

            intelligence = st1.intelligence - st2.intelligence,
            speed = st1.speed - st2.speed,
            luck = st1.luck - st2.luck,
            critDamage = st1.critDamage - st2.critDamage,

            lifeSteal = st1.lifeSteal - st2.lifeSteal,
            accuracy = st1.accuracy - st2.accuracy
        };
    }

    public static Stats operator *(Stats st1, float rate)
    {
        return new Stats
        {
            health = (int)(st1.health * rate),
            damage = (int)(st1.damage * rate),
            armor = (int)(st1.armor * rate),
            resistance = (int)(st1.resistance * rate),

            intelligence = Clamp((int)(st1.intelligence * rate), 100),
            speed = Clamp((int)(st1.speed * rate), 100),
            luck = Clamp((int)(st1.luck * rate), 100),
            critDamage = (int)(st1.critDamage * rate),

            lifeSteal = (int)(st1.lifeSteal * rate),
            accuracy = Clamp((int)(st1.accuracy * rate), 80)
        };
    }

    private static int Clamp(int value, int limit)
    {
        return Mathf.Min(value, limit);
    }
}