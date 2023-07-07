using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(QuestUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(QuestUI));
    }
}
