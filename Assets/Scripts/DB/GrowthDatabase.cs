using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "GrowthDatabase", menuName = "Database/Growth")]
    public class GrowthDatabase : ScriptableDatabase
    {
        [TableList, ShowInInspector]
        private List<EntityGrowth> _entityGrowths = new();

        [TableList, ShowInInspector]
        private List<EquipmentGrowth> _equipmentGrowths = new();

        internal override void Import(params string[] data)
        {
            _entityGrowths = new List<EntityGrowth>();
            _equipmentGrowths = new List<EquipmentGrowth>();

            var jArrayChar = JArray.Parse(data[0]);
            foreach (var jToken in jArrayChar)
            {
                ConvertDataFromJObject((JObject)jToken, out EntityGrowth h);
                _entityGrowths.Add(h);
            }

            var jArrayEqm = JArray.Parse(data[1]);
            foreach (var jToken in jArrayEqm)
            {
                ConvertDataFromJObject((JObject)jToken, out EquipmentGrowth e);
                _equipmentGrowths.Add(e);
            }
        }

        [Button]
        internal override void DeleteAll()
        {
            _entityGrowths.Clear();
            _equipmentGrowths.Clear();
        }

        internal float GetGrowth(object obj)
        {
            switch (obj)
            {
                case Tier t:
                    return _entityGrowths.Find(x => x.Tier == t).Growth;
                case Rarity r:
                    return _equipmentGrowths.Find(x => x.Rarity == r).Growth;
                default:
                    return 0;
            }
        }

        private void ConvertDataFromJObject(JObject jObject, out EntityGrowth g)
        {
            Enum.TryParse((string)jObject["tier"], out Tier tier);

            g = new EntityGrowth
            {
                Tier = tier,
                Growth = Utils.Parse<float>((string)jObject["growth(%)"]) / 100f,
            };
        }

        private void ConvertDataFromJObject(JObject jObject, out EquipmentGrowth g)
        {
            Enum.TryParse((string)jObject["rarity"], out Rarity rarity);

            g = new EquipmentGrowth
            {
                Rarity = rarity,
                Growth = Utils.Parse<float>((string)jObject["growth(%)"]) / 100f,
            };
        }
    }

    [Serializable]
    public struct EntityGrowth
    {
        public Tier Tier;
        public float Growth;
    }

    [Serializable]
    public struct EquipmentGrowth
    {
        public Rarity Rarity;
        public float Growth;
    }
}
