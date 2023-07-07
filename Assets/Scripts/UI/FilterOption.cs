using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilterOption : MonoBehaviour
{
    protected Action<object> OnApplyFilter;

    public void SetEvent(Action<object> cb)
    {
        OnApplyFilter = cb;
    }
    
    public abstract void OnSelect();
}
