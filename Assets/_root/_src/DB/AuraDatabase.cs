using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AuraDatabase", menuName = "DB/AuraDatabase")]
public class AuraDatabase : ScriptableDatabase
{
    public List<RaceAura> raceAuras = new();
    public List<ElementAura> elementAuras = new();

    public override void Import()
    {
    }

    public override void Delete()
    {
        raceAuras.Clear();
        elementAuras.Clear();
    }

    public List<Aura> GetAuras(object obj)
    {
        return obj switch
        {
            Realm r => raceAuras.Find(x => x.realm == r).auras,
            Role e => elementAuras.Find(x => x.role == e).auras,
            _ => null
        };
    }
}

[Serializable]
public struct RaceAura
{
    [HideLabel] public Realm realm;
    public List<Aura> auras;
}

[Serializable]
public struct ElementAura
{
    [HideLabel] public Role role;
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