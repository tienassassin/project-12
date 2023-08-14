using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "Database/Equipments")]
    internal class EquipmentDatabase : ScriptableDatabase
    {
        [TableList] private List<Equipment> _equipments = new();
        private readonly Dictionary<string, Equipment> _cachedDict = new();

        internal override void Import(params string[] data)
        {
            _equipments = new List<Equipment>();
            var jArray = JArray.Parse(data[0]);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var e);
                if (e != null) _equipments.Add(e);
            }
        }

        [Button]
        internal override void DeleteAll()
        {
            _equipments.Clear();
        }

        private void ConvertDataFromJObject(JObject jObject, out Equipment e)
        {
            if (((string)jObject["name"]).IsNullOrWhitespace())
            {
                e = null;
                return;
            }

            string rarityValue = Utils.GetNormalizedString((string)jObject["rarity"]);

            Enum.TryParse(rarityValue, out Rarity rarity);
            Enum.TryParse((string)jObject["race"], out Race race);
            Enum.TryParse((string)jObject["slot"], out Slot slot);
            Enum.TryParse((string)jObject["requirement"], out Requirement req);

            e = new Equipment();
            e.Id = (string)jObject["ID"];
            e.Name = (string)jObject["name"];
            e.Set = (string)jObject["set"];
            e.Rarity = rarity;
            e.Slot = slot;
            e.Requirement = req;
            e.Race = race;
            e.RaceBonus = Utils.Parse<float>((string)jObject["race bonus(%)"]) / 100f;
            e.Stats = new Stats
            {
                showFull = false,
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
            };
        }

        public Equipment GetEquipmentWithID(string eqmId)
        {
            _cachedDict.TryAdd(eqmId, _equipments.Find(e => e.Id == eqmId));
            if (_cachedDict[eqmId] == null) EditorLog.Error($"Equipment {eqmId} is not defined");
            return _cachedDict[eqmId];
        }
    }

    [Serializable]
    public class Equipment
    {
        [VerticalGroup("Information")]
        public string Id;

        [VerticalGroup("Information")]
        public string Name;

        [VerticalGroup("Information"), HideIf("@string.IsNullOrWhiteSpace(this.set)")]
        public string Set;

        [VerticalGroup("Information")]
        public Rarity Rarity;

        [VerticalGroup("Information")]
        public Slot Slot;

        [VerticalGroup("Information")]
        public Requirement Requirement;

        [VerticalGroup("Information")]
        public Race Race;

        [VerticalGroup("Information")]
        public float RaceBonus;

        public Stats Stats;
    }
}
