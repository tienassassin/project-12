using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "DevilDatabase",menuName = "Database/Devil")]
    public class DevilDatabase : ScriptableDatabase
    {
        [TableList, ShowInInspector] 
        private List<Devil> _devils = new();
        
        private readonly Dictionary<string, Devil> _cachedDict = new();

        public override void Import(params string[] data)
        {
            _devils = new List<Devil>();
            var jArray = JArray.Parse(data[0]);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var d);
                if (d != null) _devils.Add(d);
            }
        }
        
        [Button]
        public override void DeleteAll()
        {
            _devils.Clear();
        } 

        private void ConvertDataFromJObject(JObject jObject, out Devil d)
        {
            if (((string)jObject["name"]).IsNullOrWhitespace())
            {
                d = null;
                return;
            }

            string tierValue = Utils.GetNormalizedString((string)jObject["tier"]);
            
            Enum.TryParse(tierValue, out Tier tier);
            Enum.TryParse((string)jObject["element"], out Element element);
            Enum.TryParse((string)jObject["race"], out Race race);
            Enum.TryParse((string)jObject["damage type"], out DamageType dmgType);
            Enum.TryParse((string)jObject["attack range"], out AttackRange atkRange);
            
            d = new Devil
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

        public Devil GetDevilWithID(string devilId)
        {
            _cachedDict.TryAdd(devilId, _devils.Find(x => x.Id == devilId));
            if (_cachedDict[devilId] == null) EditorLog.Error($"Devil {devilId} is not defined");
            return _cachedDict[devilId];
        }

        public List<Devil> GetDevilsWithConditions(params object[] conditions)
        {
            var matchDevilsList = new List<Devil>();
            
            var raceOptList = new List<Race>();
            var elementOptList = new List<Element>();
            var tierOptList = new List<Tier>();
            bool acpAllRace = true;
            bool acpAllElement = true;
            bool acpAllTier = true;
            
            foreach (var condition in conditions)
            {
                switch (condition)
                {
                    case Race race:
                        raceOptList.Add(race);
                        acpAllRace = false;
                        break;
                    case Element element:
                        elementOptList.Add(element);
                        acpAllElement = false;
                        break;
                    case Tier tier:
                        tierOptList.Add(tier);
                        acpAllTier = false;
                        break;
                }
            }

            _devils.ForEach(d =>
            {
                if ((raceOptList.Contains(d.Race) || acpAllRace)
                    && (elementOptList.Contains(d.Element) || acpAllElement)
                    && (tierOptList.Contains(d.Tier) || acpAllTier))
                {
                    matchDevilsList.Add(d);
                }
            });
            
            return matchDevilsList;
        }
    }

    [Serializable]
    public class Devil
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
