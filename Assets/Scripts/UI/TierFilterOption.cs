using UnityEngine;

public class TierFilterOption : FilterOption
{
    [SerializeField] private Tier tier;

    public override void OnSelect()
    {
        FilterApplied?.Invoke(tier);
    }
}
