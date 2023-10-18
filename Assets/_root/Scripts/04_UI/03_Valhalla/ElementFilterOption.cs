using UnityEngine;

public class ElementFilterOption : FilterOption
{
    [SerializeField] private Role role;

    public override void OnSelect()
    {
        FilterApplied?.Invoke(role);
    }
}
