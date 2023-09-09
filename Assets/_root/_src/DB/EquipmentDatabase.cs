// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Threading.Tasks;
// using Newtonsoft.Json.Linq;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities;
// using UnityEngine;
//
// [CreateAssetMenu(fileName = "EquipmentDatabase", menuName = "DB/EquipmentDatabase")]
// public class EquipmentDatabase : ScriptableDatabase
// {
//     [TableList]
//     public List<Equipment> equipments = new();
//
//     private readonly Dictionary<string, Equipment> _cachedDict = new();
//
//     protected override async void Import()
//     {
//         var data = this.FetchFromLocal(0);
//
//         equipments = new List<Equipment>();
//
//         var watch = new Stopwatch();
//         watch.Start();
//         await Task.Run(() =>
//         {
//             var jArray = JArray.Parse(data);
//             foreach (var jToken in jArray)
//             {
//                 ConvertDataFromJObject((JObject)jToken, out var e);
//                 if (e != null) equipments.Add(e);
//             }
//         });
//
//         watch.Stop();
//         DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
//     }
//
//     [Button]
//     protected override void DeleteAll()
//     {
//         equipments.Clear();
//     }
//
//     private void ConvertDataFromJObject(JObject jObject, out Equipment e)
//     {
//         if (((string)jObject["name"]).IsNullOrWhitespace())
//         {
//             e = null;
//             return;
//         }
//
//         string rarityValue = Common.GetNormalizedString((string)jObject["rarity"]);
//
//         Enum.TryParse(rarityValue, out Rarity rarity);
//         Enum.TryParse((string)jObject["race"], out Race race);
//         Enum.TryParse((string)jObject["slot"], out Slot slot);
//         Enum.TryParse((string)jObject["requirement"], out Requirement req);
//
//         e = new Equipment();
//         e.id = (string)jObject["ID"];
//         e.name = (string)jObject["name"];
//         e.set = (string)jObject["set"];
//         e.rarity = rarity;
//         e.slot = slot;
//         e.requirement = req;
//         e.race = race;
//         e.raceBonus = Common.Parse<float>((string)jObject["race bonus(%)"]) / 100f;
//         e.stats = new Stats
//         {
//             minimized = true,
//             health = Common.Parse<float>((string)jObject["health"]),
//             damage = Common.Parse<float>((string)jObject["damage"]),
//             armor = Common.Parse<float>((string)jObject["armor"]),
//             resistance = Common.Parse<float>((string)jObject["resistance"]),
//             intelligence = Common.Parse<float>((string)jObject["intelligence"]),
//             speed = Common.Parse<float>((string)jObject["speed"]),
//             luck = Common.Parse<float>((string)jObject["luck"]),
//             critDamage = Common.Parse<float>((string)jObject["crit damage"]),
//             lifeSteal = Common.Parse<float>((string)jObject["life steal"]),
//             accuracy = Common.Parse<float>((string)jObject["accuracy"])
//         };
//     }
//
//     public Equipment GetEquipmentWithID(string eqmId)
//     {
//         _cachedDict.TryAdd(eqmId, equipments.Find(e => e.id == eqmId));
//         if (_cachedDict[eqmId] == null) EditorLog.Error($"Equipment {eqmId} is not defined");
//         return _cachedDict[eqmId];
//     }
// }
//
// [Serializable]
// public class Equipment
// {
//     [VerticalGroup("Information")]
//     public string id;
//
//     [VerticalGroup("Information")]
//     public string name;
//
//     [VerticalGroup("Information")] [HideIf("@string.IsNullOrWhiteSpace(this.set)")]
//     public string set;
//
//     [VerticalGroup("Information")]
//     public Rarity rarity;
//
//     [VerticalGroup("Information")]
//     public Slot slot;
//
//     [VerticalGroup("Information")]
//     public Requirement requirement;
//
//     [VerticalGroup("Information")]
//     public Race race;
//
//     [VerticalGroup("Information")]
//     public float raceBonus;
//
//     public Stats stats;
// }
//
// public static partial class DataExtensions
// {
//     public static float GetEquipmentGrowth(this Equipment e)
//     {
//         return DataManager.Instance.GetGrowth(e.rarity);
//     }
// }

