using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineUpUI : MonoBehaviour
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(LineUpUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(LineUpUI));
    }
}
