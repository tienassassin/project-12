using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform _panel;
    private Rect _lastSafeArea = new Rect(0f, 0f, 0f, 0f);
    private ScreenOrientation _lastScreenOrientation = ScreenOrientation.AutoRotation;
    
    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
    }
    
    private void Update()
    {
        Refresh();
    }
    
    private void Refresh()
    {
        if (_lastSafeArea != Screen.safeArea || _lastScreenOrientation != Screen.orientation)
        {
            ApplySafeArea(Screen.safeArea);
        }
    }
    
    private void ApplySafeArea(Rect safeArea)
    {
        _lastSafeArea = safeArea;
        _lastScreenOrientation = Screen.orientation;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
    
        anchorMin.x /= Screen.width;
        anchorMax.x /= Screen.width;
    
        _panel.anchorMin = new Vector2(anchorMin.x, _panel.anchorMin.y);
        _panel.anchorMax = new Vector2(anchorMax.x, _panel.anchorMax.y);
    }
}