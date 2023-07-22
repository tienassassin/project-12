using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Equipment : DuztineBehaviour
{
    [TitleGroup("BASE DATA")]
    protected BaseEquipment baseEqm;
    protected int level = 0;

    public void Init(string eqmId, int lv)
    {
        baseEqm = Database.Instance.GetEquipmentWithID(eqmId);
        level = lv;
    }

    public Stats GetStats(Race ownerRace)
    {
        var stats = baseEqm.stats * (baseEqm.race != ownerRace ? 1 : baseEqm.raceBonus);
        stats *= Mathf.Pow(baseEqm.GetEquipmentGrowth(), level);
        return stats;
    }

    public Stats GetEnhancement(Race ownerRace)
    {
        return GetStats(ownerRace) - baseEqm.stats;
    }
}
