using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "HeroDatabase", menuName = "Database/Hero")]
    public class HeroDatabase : ScriptableDatabase
    {
        [TableList,ShowInInspector]
        private List<Hero> _heroes = new();
        
        private readonly Dictionary<string, Hero> _cachedDict = new();

        public override void Import(params string[] data)
        {
            _heroes = new List<Hero>();
            var jArray = JArray.Parse(data[0]);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var h);
                if (h != null) _heroes.Add(h);
            }
        }

        [Button]
        public override void DeleteAll()
        {
            _heroes.Clear();
        }

        public List<Hero> GetHeroes()
        {
            return _heroes;
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
                Id = (string)jObject["ID"],
                Name = (string)jObject["name"],
                Tier = tier,
                Element = element,
                Race = race,
                DamageType = dmgType,
                AttackRange = atkRange,
                Stats = new Stats
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
                    accuracy = Utils.Parse<float>((string)jObject["accuracy"]),
                }
            };
        }

        public Hero GetHeroWithID(string heroId)
        {
            _cachedDict.TryAdd(heroId, _heroes.Find(h => h.Id == heroId));
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

            _heroes.ForEach(h =>
            {
                if ((raceOpts.Contains(h.Race) || acpAllRace)
                    && (elementOpts.Contains(h.Element) || acpAllElement)
                    && (tierOpts.Contains(h.Tier) || acpAllTier))
                {
                    matchHeroes.Add(h);
                }
            });

            return matchHeroes;
        }
    }

    [Serializable]
    public class Hero
    {
        [VerticalGroup("Information")]
        public string Id;

        [VerticalGroup("Information")]
        public string Name;

        [VerticalGroup("Information")]
        public Tier Tier;

        [VerticalGroup("Information")]
        public Element Element;

        [VerticalGroup("Information")]
        public Race Race;

        [VerticalGroup("Information")]
        public DamageType DamageType;

        [VerticalGroup("Information")]
        public AttackRange AttackRange;

        public Stats Stats;
    }
}
