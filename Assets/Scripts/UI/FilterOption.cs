using System;
using UnityEngine;

public abstract class FilterOption : MonoBehaviour
{
    protected Action<object> FilterApplied;

    public void SetEvent(Action<object> filterApplied)
    {
        FilterApplied = filterApplied;
    }
    
    public abstract void OnSelect();
}
