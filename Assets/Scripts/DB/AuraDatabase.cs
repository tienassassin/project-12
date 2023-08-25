using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class AuraDatabase : Database
{
    public List<RaceAura> raceAuras = new();
    public List<ElementAura> elementAuras = new();

    protected override async void Import()
    {
        var data = this.FetchFromLocal();

        raceAuras = new List<RaceAura>();
        elementAuras = new List<ElementAura>();

        var watch = new Stopwatch();
        watch.Start();
        await Task.Run(() =>
        {
            var jArrayRace = JArray.Parse(data[0]);
            foreach (var jToken in jArrayRace)
            {
                ConvertDataFromJObject((JObject)jToken, out Race r, out Aura a);
                var matchRaceAura = raceAuras.Find(x => x.race == r);
                if (matchRaceAura != null) matchRaceAura.auras.Add(a);
                else
                {
                    matchRaceAura = new RaceAura
                    {
                        race = r,
                        auras = new List<Aura> { a }
                    };
                    raceAuras.Add(matchRaceAura);
                }
            }

            var jArrayElement = JArray.Parse(data[1]);
            foreach (var jToken in jArrayElement)
            {
                ConvertDataFromJObject((JObject)jToken, out Element e, out Aura a);
                var matchElementAura = elementAuras.Find(x => x.element == e);
                if (matchElementAura != null) matchElementAura.auras.Add(a);
                else
                {
                    matchElementAura = new ElementAura
                    {
                        element = e,
                        auras = new List<Aura> { a }
                    };
                    elementAuras.Add(matchElementAura);
                }
            }
        });

        watch.Stop();
        DataManager.Instance.NotifyDBLoaded(databaseName, (int)watch.ElapsedMilliseconds);
    }

    [Button]
    protected override void DeleteAll()
    {
        raceAuras.Clear();
        elementAuras.Clear();
    }

    public List<Aura> GetAuras(object obj)
    {
        switch (obj)
        {
            case Race r:
                return raceAuras.Find(x => x.race == r).auras;
            case Element e:
                return elementAuras.Find(x => x.element == e).auras;
            default:
                return null;
        }
    }

    private void ConvertDataFromJObject(JObject jObject, out Race r, out Aura a)
    {
        Enum.TryParse((string)jObject["race"], out r);
        a = new Aura
        {
            rank = Utils.Parse<int>((string)jObject["rank"]),
            name = (string)jObject["name"],
            description = (string)jObject["description"]
        };
    }

    private void ConvertDataFromJObject(JObject jObject, out Element e, out Aura a)
    {
        Enum.TryParse((string)jObject["element"], out e);
        a = new Aura
        {
            rank = Utils.Parse<int>((string)jObject["rank"]),
            name = (string)jObject["name"],
            description = (string)jObject["description"]
        };
    }
}

[Serializable]
public class RaceAura
{
    [HideLabel] public Race race;
    public List<Aura> auras;
}

[Serializable]
public class ElementAura
{
    [HideLabel] public Element element;
    public List<Aura> auras;
}

[Serializable]
public struct Aura
{
    public int rank;
    public string name;
    [TextArea(5, 10)]
    public string description;
}