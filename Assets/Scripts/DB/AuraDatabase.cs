using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DB.System
{
    public class AuraDatabase : Database
    {
        public List<RaceAura> raceAuras = new();
        public List<ElementAura> elementAuras = new();

        protected override void Import()
        {
            var data = this.FetchFromLocal();
            
            raceAuras = new List<RaceAura>();
            elementAuras = new List<ElementAura>();

            var jArrayRace = JArray.Parse(data[0]);
            foreach (var jToken in jArrayRace)
            {
                ConvertDataFromJObject((JObject)jToken, out Race r, out Aura a);
                var matchRaceAura = raceAuras.Find(x => x.Race == r);
                if (matchRaceAura != null) matchRaceAura.Auras.Add(a);
                else
                {
                    matchRaceAura = new RaceAura
                    {
                        Race = r,
                        Auras = new List<Aura> { a }
                    };
                    raceAuras.Add(matchRaceAura);
                }
            }

            var jArrayElement = JArray.Parse(data[1]);
            foreach (var jToken in jArrayElement)
            {
                ConvertDataFromJObject((JObject)jToken, out Element e, out Aura a);
                var matchElementAura = elementAuras.Find(x => x.Element == e);
                if (matchElementAura != null) matchElementAura.Auras.Add(a);
                else
                {
                    matchElementAura = new ElementAura
                    {
                        Element = e,
                        Auras = new List<Aura> { a },
                    };
                    elementAuras.Add(matchElementAura);
                }
            }
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
                    return raceAuras.Find(x => x.Race == r).Auras;
                case Element e:
                    return elementAuras.Find(x => x.Element == e).Auras;
                default:
                    return null;
            }
        }

        private void ConvertDataFromJObject(JObject jObject, out Race r, out Aura a)
        {
            Enum.TryParse((string)jObject["race"], out r);
            a = new Aura
            {
                Rank = Utils.Parse<int>((string)jObject["rank"]),
                Name = (string)jObject["name"],
                Description = (string)jObject["description"],
            };
        }
        
        private void ConvertDataFromJObject(JObject jObject, out Element e, out Aura a)
        {
            Enum.TryParse((string)jObject["element"], out e);
            a = new Aura
            {
                Rank = Utils.Parse<int>((string)jObject["rank"]),
                Name = (string)jObject["name"],
                Description = (string)jObject["description"],
            };
        }
    }

    [Serializable]
    public class RaceAura
    {
        [HideLabel] public Race Race;
        public List<Aura> Auras;
    }

    [Serializable]
    public class ElementAura
    {
        [HideLabel] public Element Element;
        public List<Aura> Auras;
    }

    [Serializable]
    public struct Aura
    {
        public int Rank;
        public string Name;
        [TextArea(5,10)]
        public string Description;
    }
}