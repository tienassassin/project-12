using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

public class GrowthDatabase : Database
{
    [TableList]
    public List<EntityGrowth> entityGrowths = new();
    [TableList]
    public List<EquipmentGrowth> equipmentGrowths = new();

    protected override async void Import()
    {
        var data = this.FetchFromLocal();

        entityGrowths = new List<EntityGrowth>();
        equipmentGrowths = new List<EquipmentGrowth>();

        var watch = new Stopwatch();
        watch.Start();
        await Task.Run(() =>
        {
            var jArrayEnt = JArray.Parse(data[0]);
            foreach (var jToken in jArrayEnt)
            {
                ConvertDataFromJObject((JObject)jToken, out EntityGrowth g);
                entityGrowths.Add(g);
            }

            var jArrayEqm = JArray.Parse(data[1]);
            foreach (var jToken in jArrayEqm)
            {
                ConvertDataFromJObject((JObject)jToken, out EquipmentGrowth g);
                equipmentGrowths.Add(g);
            }
        });

        watch.Stop();
        DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
    }

    [Button]
    protected override void DeleteAll()
    {
        entityGrowths.Clear();
        equipmentGrowths.Clear();
    }

    public float GetGrowth(object obj)
    {
        switch (obj)
        {
            case Tier t:
                return entityGrowths.Find(x => x.tier == t).growth;
            case Rarity r:
                return equipmentGrowths.Find(x => x.rarity == r).growth;
            default:
                return 0;
        }
    }

    private void ConvertDataFromJObject(JObject jObject, out EntityGrowth g)
    {
        Enum.TryParse((string)jObject["tier"], out Tier tier);

        g = new EntityGrowth
        {
            tier = tier,
            growth = Common.Parse<float>((string)jObject["growth(%)"]) / 100f
        };
    }

    private void ConvertDataFromJObject(JObject jObject, out EquipmentGrowth g)
    {
        Enum.TryParse((string)jObject["rarity"], out Rarity rarity);

        g = new EquipmentGrowth
        {
            rarity = rarity,
            growth = Common.Parse<float>((string)jObject["growth(%)"]) / 100f
        };
    }
}

[Serializable]
public struct EntityGrowth
{
    public Tier tier;
    public float growth;
}

[Serializable]
public struct EquipmentGrowth
{
    public Rarity rarity;
    public float growth;
}