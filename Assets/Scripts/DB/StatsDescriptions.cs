using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "StatsDescriptions", menuName = "Description/Stats")]
    public class StatsDescriptions : ScriptableDatabase
    {
        [TableList, ShowInInspector] 
        private List<StatDesc> _statDescriptions = new();
        
        private readonly Dictionary<string, StatDesc> _cachedDict = new();

        internal override void Import(params string[] data)
        {
            _statDescriptions = new List<StatDesc>();
            var jArray = JArray.Parse(data[0]);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var s);
                _statDescriptions.Add(s);
            }
        }

        [Button]
        internal override void DeleteAll()
        {
            _statDescriptions.Clear();
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

        internal StatDesc GetStatDescription(string stat)
        {
            _cachedDict.TryAdd(stat, _statDescriptions.Find(s => s.Stat == stat));
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

