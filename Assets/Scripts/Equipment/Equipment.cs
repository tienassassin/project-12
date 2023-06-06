using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] protected EquipmentData eqmData;

    [ShowInInspector] private Stats stats;
    
    public int level = 1;

    public Stats GetEquipmentStats(Races ownerRace)
    {
        stats = eqmData.stats;
        for (int i = 0; i < level; i++)
        {
            stats *= (1 + eqmData.levelBonus);
        }
        
        if (eqmData.race == ownerRace)
        {
            return stats *= (1 + eqmData.raceBonus);
        }

        return stats;
    }
}
