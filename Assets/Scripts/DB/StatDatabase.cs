using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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

        protected override async void Import()
        {
            var data = this.FetchFromLocal(0);
            
            stats = new List<StatDesc>();
            
            var watch = new Stopwatch();
            watch.Start();
            await Task.Run(() =>
            {
                var jArray = JArray.Parse(data);
                foreach (var jToken in jArray)
                {
                    ConvertDataFromJObject((JObject)jToken, out var s);
                    stats.Add(s);
                }
            });
            
            watch.Stop();
            DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
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
                stat = (string)jObject["stat"],
                name = (string)jObject["name"],
                limit = Utils.Parse<float>((string)jObject["limit"]),
                description = (string)jObject["description"]
            };
        }

        public StatDesc GetStatDescription(string stat)
        {
            _cachedDict.TryAdd(stat, stats.Find(s => s.stat == stat));
            if (_cachedDict[stat] == null) EditorLog.Error($"Stat {stat} is not defined");
            return _cachedDict[stat];
        }


    }

    [Serializable]
    public class StatDesc
    {
        [TableColumnWidth(100, Resizable = false)]
        public string stat;

        [TableColumnWidth(100, Resizable = false)]
        public string name;

        [TableColumnWidth(70, Resizable = false), ShowIf("@this.limit > 0")]
        public float limit;

        [TextArea(3, 10)]
        public string description;
    }
}

