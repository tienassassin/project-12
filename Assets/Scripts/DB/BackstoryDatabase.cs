using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "BackstoryDatabase", menuName = "Database/Backstory")]
    public class BackstoryDatabase : ScriptableDatabase
    {
        [TableList, ShowInInspector] 
        private List<Backstory> _stories = new();
        
        private readonly Dictionary<string, Backstory> _cachedDict = new();

        public override void Import(params string[] data)
        {
            _stories = new List<Backstory>();
            var jArray = JArray.Parse(data[0]);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var b);
                _stories.Add(b);
            }
        }

        [Button]
        public override void DeleteAll()
        {
            _stories.Clear();
        }

        private void ConvertDataFromJObject(JObject jObject, out Backstory b)
        {
            b = new Backstory
            {
                Id = (string)jObject["ID"],
                Name = (string)jObject["name"],
                Alias = (string)jObject["alias"],
                Story = (string)jObject["story"]
            };
        }

        public Backstory GetBackstory(string charId)
        {
            _cachedDict.TryAdd(charId, _stories.Find(x => x.Id == charId));
            if (_cachedDict[charId] == null) EditorLog.Error($"Backstory of {charId} is not defined");
            return _cachedDict[charId];
        }
    }

    [Serializable]
    public class Backstory
    {
        [VerticalGroup("Information")]
        [TableColumnWidth(200, Resizable = false)] 
        public string Id;
        
        [VerticalGroup("Information")] 
        public string Name;

        [VerticalGroup("Information")]
        [Multiline(2)]
        public string Alias;

        [VerticalGroup("Story")]
        [TextArea(7,10), HideLabel]
        public string Story;
    }
}