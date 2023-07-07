using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFilterOption : FilterOption
{
    [SerializeField] private Race race;

    public override void OnSelect()
    {
        OnApplyFilter?.Invoke(race);
    }
}
