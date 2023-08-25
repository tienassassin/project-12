using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class EquipmentDatabase : Database
{
    [TableList]
    public List<Equipment> equipments = new();

    private readonly Dictionary<string, Equipment> _cachedDict = new();

    protected override async void Import()
    {
        var data = this.FetchFromLocal(0);

        equipments = new List<Equipment>();

        var watch = new Stopwatch();
        watch.Start();
        await Task.Run(() =>
        {
            var jArray = JArray.Parse(data);
            foreach (var jToken in jArray)
            {
                ConvertDataFromJObject((JObject)jToken, out var e);
                if (e != null) equipments.Add(e);
            }
        });

        watch.Stop();
        DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
    }

    [Button]
    protected override void DeleteAll()
    {
        equipments.Clear();
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
        e.id = (string)jObject["ID"];
        e.name = (string)jObject["name"];
        e.set = (string)jObject["set"];
        e.rarity = rarity;
        e.slot = slot;
        e.requirement = req;
        e.race = race;
        e.raceBonus = Utils.Parse<float>((string)jObject["race bonus(%)"]) / 100f;
        e.stats = new Stats
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
            accuracy = Utils.Parse<float>((string)jObject["accuracy"])
        };
    }

    public Equipment GetEquipmentWithID(string eqmId)
    {
        _cachedDict.TryAdd(eqmId, equipments.Find(e => e.id == eqmId));
        if (_cachedDict[eqmId] == null) EditorLog.Error($"Equipment {eqmId} is not defined");
        return _cachedDict[eqmId];
    }
}

[Serializable]
public class Equipment
{
    [VerticalGroup("Information")]
    public string id;

    [VerticalGroup("Information")]
    public string name;

    [VerticalGroup("Information")] [HideIf("@string.IsNullOrWhiteSpace(this.set)")]
    public string set;

    [VerticalGroup("Information")]
    public Rarity rarity;

    [VerticalGroup("Information")]
    public Slot slot;

    [VerticalGroup("Information")]
    public Requirement requirement;

    [VerticalGroup("Information")]
    public Race race;

    [VerticalGroup("Information")]
    public float raceBonus;

    public Stats stats;
}