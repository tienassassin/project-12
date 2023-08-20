using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DB.System
{
    public class StatDatabase : Database
    {
        [TableList] 
        public List<StatDesc> stats = new();
        
        private readonly Dictionary<string, StatDesc> _cachedDict = new();

        protected override void Import()
        {
            var data = this.FetchFromLocal(0);
            
            stats = new List<StatDesc>();
            var jArray = JArray.Parse(data);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var s);
                stats.Add(s);
            }
        }

        [Button]
        protected override void DeleteAll()
        {
            stats.Clear();
        }

        private void ConvertDataFromJObject(JObject jObject, out StatDesc s)
        {
            s = new StatDesc
            {
                Stat = (string)jObject["stat"],
                Name = (string)jObject["name"],
                Limit = Utils.Parse<float>((string)jObject["limit"]),
                Description = (string)jObject["description"]
            };
        }

        public StatDesc GetStatDescription(string stat)
        {
            _cachedDict.TryAdd(stat, stats.Find(s => s.Stat == stat));
            if (_cachedDict[stat] == null) EditorLog.Error($"Stat {stat} is not defined");
            return _cachedDict[stat];
        }


    }

    [Serializable]
    public class StatDesc
    {
        [TableColumnWidth(100, Resizable = false)]
        public string Stat;

        [TableColumnWidth(100, Resizable = false)]
        public string Name;

        [TableColumnWidth(70, Resizable = false), ShowIf("@this.Limit > 0")]
        public float Limit;

        [TextArea(3, 10)]
        public string Description;
    }
}

