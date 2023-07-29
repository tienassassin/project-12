using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AuraDatabase", menuName = "Database/Aura")]
public class AuraDatabase : ScriptableDatabase
{
    public List<RaceAura> raceAuraList = new();
    public List<ElementAura> elementAuraList = new();

    public override void Import(params string[] data)
    {
        raceAuraList = new List<RaceAura>();
        elementAuraList = new List<ElementAura>();

        var jArrayRace = JArray.Parse(data[0]);
        foreach (var jToken in jArrayRace)
        {
            ConvertDataFromJObject((JObject)jToken, out Race r, out Aura a);
            var matchRaceAura = raceAuraList.Find(x => x.race == r);
            if (matchRaceAura != null) matchRaceAura.auraList.Add(a);
            else
            {
                matchRaceAura = new RaceAura
                {
                    race = r,
                    auraList = new List<Aura> { a }
                };
                raceAuraList.Add(matchRaceAura);
            }
        }

        var jArrayElement = JArray.Parse(data[1]);
        foreach (var jToken in jArrayElement)
        {
            ConvertDataFromJObject((JObject)jToken, out Element e, out Aura a);
            var matchElementAura = elementAuraList.Find(x => x.element == e);
            if (matchElementAura != null) matchElementAura.auraList.Add(a);
            else
            {
                matchElementAura = new ElementAura
                {
                    element = e,
                    auraList = new List<Aura> { a },
                };
                elementAuraList.Add(matchElementAura);
            }
        }
    }

    protected override void DeleteAll()
    {
        raceAuraList.Clear();
        elementAuraList.Clear();
    }

    private void ConvertDataFromJObject(JObject jObject, out Race r, out Aura a)
    {
        Enum.TryParse((string)jObject["race"], out r);
        a = new Aura
        {
            rank = Utils.Parse<int>((string)jObject["rank"]),
            name = (string)jObject["name"],
            description = (string)jObject["description"],
        };
    }
    
    private void ConvertDataFromJObject(JObject jObject, out Element e, out Aura a)
    {
        Enum.TryParse((string)jObject["element"], out e);
        a = new Aura
        {
            rank = Utils.Parse<int>((string)jObject["rank"]),
            name = (string)jObject["name"],
            description = (string)jObject["description"],
        };
    }
}

[Serializable]
public class RaceAura
{
    [HideLabel] public Race race;
    public List<Aura> auraList;
}

[Serializable]
public class ElementAura
{
    [HideLabel] public Element element;
    public List<Aura> auraList;
}

[Serializable]
public struct Aura
{
    public int rank;
    public string name;
    [TextArea(5,10)]
    public string description;
}