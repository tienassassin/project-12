using UnityEngine;

public class RaceFilterOption : FilterOption
{
    [SerializeField] private Race race;

    public override void OnSelect()
    {
        FilterApplied?.Invoke(race);
    }
}
