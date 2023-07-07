using UnityEngine;

public class TierFilterOption : FilterOption
{
    [SerializeField] private Tier tier;

    public override void OnSelect()
    {
        OnApplyFilter?.Invoke(tier);
    }
}
