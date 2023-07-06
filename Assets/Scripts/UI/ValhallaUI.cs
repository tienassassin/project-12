using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValhallaUI : BaseUI
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(ValhallaUI));
    }

    public static void Hide()
    {
        UIManager.Instance.ShowUI(nameof(ValhallaUI));
    }
}
