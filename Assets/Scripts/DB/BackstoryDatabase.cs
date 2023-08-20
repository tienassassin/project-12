using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DB.System
{
    public class BackstoryDatabase : Database
    {
        [TableList]
        public List<Backstory> stories = new();
        
        private readonly Dictionary<string, Backstory> _cachedDict = new();

        protected override void Import()
        {
            var data = this.FetchFromLocal(0);
            
            stories = new List<Backstory>();
            var jArray = JArray.Parse(data);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var b);
                stories.Add(b);
            }
        }

        [Button]
        protected override void DeleteAll()
        {
            stories.Clear();
        }

        private void ConvertDataFromJObject(JObject jObject, out Backstory b)
        {
            b = new Backstory
            {
                id = (string)jObject["ID"],
                name = (string)jObject["name"],
                alias = (string)jObject["alias"],
                story = (string)jObject["story"]
            };
        }

        public Backstory GetBackstory(string charId)
        {
            _cachedDict.TryAdd(charId, stories.Find(x => x.id == charId));
            if (_cachedDict[charId] == null) EditorLog.Error($"Backstory of {charId} is not defined");
            return _cachedDict[charId];
        }
    }

    [Serializable]
    public class Backstory
    {
        [VerticalGroup("Information")]
        [TableColumnWidth(200, Resizable = false)] 
        public string id;
        
        [VerticalGroup("Information")] 
        public string name;

        [VerticalGroup("Information")]
        [Multiline(2)]
        public string alias;

        [VerticalGroup("Story")]
        [TextArea(7,10), HideLabel]
        public string story;
    }
}