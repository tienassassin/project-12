using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class EntityDatabase : Database
{
    [TableList]
    public List<Entity> entities = new();

    private readonly Dictionary<string, Entity> _cachedDict = new();

    protected override async void Import()
    {
        var data = this.FetchFromLocal(0);

        entities = new List<Entity>();

        var watch = new Stopwatch();
        watch.Start();
        await Task.Run(() =>
        {
            var jArray = JArray.Parse(data);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var e);
                if (e != null) entities.Add(e);
            }
        });

        watch.Stop();
        DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
    }

    [Button]
    protected override void DeleteAll()
    {
        entities.Clear();
    }

    public List<Entity> GetAllEntities()
    {
        return entities;
    }

    private void ConvertDataFromJObject(JObject jObject, out Entity e)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            e = null;
            return;
        }

        string tierValue = Common.GetNormalizedString((string)jObject["tier"]);

        Enum.TryParse(tierValue, out Tier tier);
        Enum.TryParse((string)jObject["element"], out Element element);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
        Enum.TryParse((string)jObject["attack range"], out AttackRange atkRange);

        e = new Entity
        {
            id = (string)jObject["ID"],
            name = (string)jObject["name"],
            tier = tier,
            element = element,
            race = race,
            damageType = dmgType,
            attackRange = atkRange,
            stats = new Stats
            {
                health = Common.Parse<float>((string)jObject["health"]),
                damage = Common.Parse<float>((string)jObject["damage"]),
                armor = Common.Parse<float>((string)jObject["armor"]),
                resistance = Common.Parse<float>((string)jObject["resistance"]),
                intelligence = Common.Parse<float>((string)jObject["intelligence"]),
                speed = Common.Parse<float>((string)jObject["speed"]),
                luck = Common.Parse<float>((string)jObject["luck"]),
                critDamage = Common.Parse<float>((string)jObject["crit damage"]),
                lifeSteal = Common.Parse<float>((string)jObject["life steal"]),
                accuracy = Common.Parse<float>((string)jObject["accuracy"])
            }
        };
    }

    public Entity GetEntityWithID(string id)
    {
        _cachedDict.TryAdd(id, entities.Find(x => x.id == id));
        if (_cachedDict[id] == null) EditorLog.Error($"Character {id} is not defined");
        return _cachedDict[id];
    }

    public List<Entity> GetEntitiesWithConditions(params object[] conditions)
    {
        var matchHeroes = new List<Entity>();

        var raceOpts = new List<Race>();
        var elementOpts = new List<Element>();
        var tierOpts = new List<Tier>();
        bool acpAllRace = true;
        bool acpAllElement = true;
        bool acpAllTier = true;

        foreach (var condition in conditions)
        {
            switch (condition)
            {
                case Race race:
                    raceOpts.Add(race);
                    acpAllRace = false;
                    break;
                case Element element:
                    elementOpts.Add(element);
                    acpAllElement = false;
                    break;
                case Tier tier:
                    tierOpts.Add(tier);
                    acpAllTier = false;
                    break;
            }
        }

        entities.ForEach(h =>
        {
            if ((raceOpts.Contains(h.race) || acpAllRace)
                && (elementOpts.Contains(h.element) || acpAllElement)
                && (tierOpts.Contains(h.tier) || acpAllTier))
            {
                matchHeroes.Add(h);
            }
        });

        return matchHeroes;
    }
}

[Serializable]
public class Entity
{
    [VerticalGroup("Information")]
    public string id;

    [VerticalGroup("Information")]
    public string name;

    [VerticalGroup("Information")]
    public Tier tier;

    [VerticalGroup("Information")]
    public Element element;

    [VerticalGroup("Information")]
    public Race race;

    [VerticalGroup("Information")]
    public DamageType damageType;

    [VerticalGroup("Information")]
    public AttackRange attackRange;

    public Stats stats;
}

[Serializable]
public struct Stats
{
    [HideInInspector]
    public bool minimized;

    [ShowIf("@!minimized || health > 0")]
    public float health;
    [ShowIf("@!minimized || damage > 0")]
    public float damage;
    [ShowIf("@!minimized || armor > 0")]
    public float armor;
    [ShowIf("@!minimized || resistance > 0")]
    public float resistance;

    [ShowIf("@!minimized || intelligence > 0")]
    public float intelligence;
    [ShowIf("@!minimized || speed > 0")]
    public float speed;
    [ShowIf("@!minimized || luck > 0")]
    public float luck;
    [ShowIf("@!minimized || critDamage > 0")]
    public float critDamage;

    [ShowIf("@!minimized || lifeSteal > 0")]
    public float lifeSteal;
    [ShowIf("@!minimized || accuracy > 0")]
    public float accuracy;

    public Stats GetStatsByLevel(int level, float growth)
    {
        return new Stats
        {
            health = health * Mathf.Pow(1 + growth, level - 1),
            damage = damage * Mathf.Pow(1 + growth, level - 1),
            armor = armor * Mathf.Pow(1 + growth, level - 1),
            resistance = resistance * Mathf.Pow(1 + growth, level - 1),

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
            health = st1.health * rate,
            damage = st1.damage * rate,
            armor = st1.armor * rate,
            resistance = st1.resistance * rate,

            intelligence = Clamp(st1.intelligence * rate, 100),
            speed = Clamp(st1.speed * rate, 100),
            luck = Clamp(st1.luck * rate, 100),
            critDamage = st1.critDamage * rate,

            lifeSteal = st1.lifeSteal * rate,
            accuracy = Clamp(st1.accuracy * rate, 80)
        };
    }

    private static float Clamp(float value, float limit)
    {
        return Mathf.Min(value, limit);
    }
}

public static partial class DataExtensions
{
    public static float GetEntityGrowth(this Entity e)
    {
        return DataManager.Instance.GetGrowth(e.tier);
    }

    public static string GetEntityAlias(this Entity e)
    {
        return DataManager.Instance.GetHeroAlias(e.id);
    }

    public static string GetEntityStory(this Entity e)
    {
        return DataManager.Instance.GetHeroStory(e.id);
    }
}