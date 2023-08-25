using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class HeroDatabase : Database
{
    [TableList]
    public List<Hero> heroes = new();

    private readonly Dictionary<string, Hero> _cachedDict = new();

    protected override async void Import()
    {
        var data = this.FetchFromLocal(0);

        heroes = new List<Hero>();

        var watch = new Stopwatch();
        watch.Start();
        await Task.Run(() =>
        {
            var jArray = JArray.Parse(data);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var h);
                if (h != null) heroes.Add(h);
            }
        });

        watch.Stop();
        DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
    }

    [Button]
    protected override void DeleteAll()
    {
        heroes.Clear();
    }

    public List<Hero> GetHeroes()
    {
        return heroes;
    }

    private void ConvertDataFromJObject(JObject jObject, out Hero h)
    {
        if (((string)jObject["name"]).IsNullOrWhitespace())
        {
            h = null;
            return;
        }

        string tierValue = Utils.GetNormalizedString((string)jObject["tier"]);

        Enum.TryParse(tierValue, out Tier tier);
        Enum.TryParse((string)jObject["element"], out Element element);
        Enum.TryParse((string)jObject["race"], out Race race);
        Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
        Enum.TryParse((string)jObject["attack range"], out AttackRange atkRange);

        h = new Hero
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
                showFull = true,
                health = Utils.Parse<float>((string)jObject["health"]),
                damage = Utils.Parse<float>((string)jObject["damage"]),
                armor = Utils.Parse<float>((string)jObject["armor"]),
                resistance = Utils.Parse<float>((string)jObject["resistance"]),
                intelligence = Utils.Parse<float>((string)jObject["intelligence"]),
                speed = Utils.Parse<float>((string)jObject["speed"]),
                luck = Utils.Parse<float>((string)jObject["luck"]),
                critDamage = Utils.Parse<float>((string)jObject["crit damage"]),
                lifeSteal = Utils.Parse<float>((string)jObject["life steal"]),
                accuracy = Utils.Parse<float>((string)jObject["accuracy"])
            }
        };
    }

    public Hero GetHeroWithID(string heroId)
    {
        _cachedDict.TryAdd(heroId, heroes.Find(h => h.id == heroId));
        if (_cachedDict[heroId] == null) EditorLog.Error($"Character {heroId} is not defined");
        return _cachedDict[heroId];
    }

    public List<Hero> GetHeroesWithConditions(params object[] conditions)
    {
        var matchHeroes = new List<Hero>();

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

        heroes.ForEach(h =>
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
public class Hero : Entity
{
}

public static partial class DataExtensions
{
    public static float GetHeroGrowth(this Hero h)
    {
        return DataManager.Instance.GetGrowth(h.tier);
    }

    public static string GetHeroAlias(this Hero h)
    {
        return DataManager.Instance.GetHeroAlias(h.id);
    }

    public static string GetHeroStory(this Hero h)
    {
        return DataManager.Instance.GetHeroStory(h.id);
    }
}