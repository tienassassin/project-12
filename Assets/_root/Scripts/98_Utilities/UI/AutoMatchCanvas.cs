using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class AutoMatchCanvas : MonoBehaviour
{
    [SerializeField] private int defaultWidth = 1920;
    [SerializeField] private int defaultHeight = 1080;

    private void Awake()
    {
        float currentRatio = (float) Screen.width / Screen.height;
        float defaultRatio = (float) defaultWidth / defaultHeight;
        if (currentRatio > defaultRatio)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
    }
}