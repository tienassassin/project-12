using UnityEngine;

public class RaceFilterOption : FilterOption
{
    [SerializeField] private Realm realm;

    public override void OnSelect()
    {
        FilterApplied?.Invoke(realm);
    }
}
