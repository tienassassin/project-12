using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace DB.System
{
    public class DevilDatabase : Database
    {
        [TableList] 
        public List<Devil> devils = new();
        
        private readonly Dictionary<string, Devil> _cachedDict = new();

        protected override async void Import()
        {
            var data = this.FetchFromLocal(0);
            
            devils = new List<Devil>();
            
            var watch = new Stopwatch();
            watch.Start();
            await Task.Run(() =>
            {
                var jArray = JArray.Parse(data);
                foreach (var jToken in jArray)
                {
                    ConvertDataFromJObject((JObject)jToken, out var d);
                    if (d != null) devils.Add(d);
                }
            });
            
            watch.Stop();
            DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
        }
        
        [Button]
        protected override void DeleteAll()
        {
            devils.Clear();
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
                    accuracy = Utils.Parse<float>((string)jObject["accuracy"]),
                }
            };
        }

        public Devil GetDevilWithID(string devilId)
        {
            _cachedDict.TryAdd(devilId, devils.Find(x => x.id == devilId));
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

            devils.ForEach(d =>
            {
                if ((raceOptList.Contains(d.race) || acpAllRace)
                    && (elementOptList.Contains(d.element) || acpAllElement)
                    && (tierOptList.Contains(d.tier) || acpAllTier))
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
}
