using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(InventoryUI));
    }

    public static void Hide()
    {
        UIManager.Instance.ShowUI(nameof(InventoryUI));
    }
}
