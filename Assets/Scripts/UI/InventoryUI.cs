using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(InventoryUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(InventoryUI));
    }
}
