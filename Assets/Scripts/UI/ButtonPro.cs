using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonPro : Button
{
    // hold handler
    public bool allowHold = true;
    public float holdTime = 1;
    public UnityEvent holdEvent = new ();
    private Coroutine _coroutine;
    
    // double click handler
    public bool allowDoubleClick;
    public float doubleClickInterval = 0.2f;
    public UnityEvent doubleClickEvent = new();
    private float _lastTimeClick;
    private bool _firstClick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _coroutine = StartCoroutine(InvokeHoldEvent());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (_coroutine != null) StopCoroutine(_coroutine);

        if (!_firstClick)
        {
            _firstClick = true;
            _lastTimeClick = Time.unscaledTime;
        }
        else
        {
            if (Time.unscaledTime - _lastTimeClick <= doubleClickInterval)
            {
                doubleClickEvent?.Invoke();
                _firstClick = false;
            }
            else
            {
                _lastTimeClick = Time.unscaledTime;
            }
        }
    }

    IEnumerator InvokeHoldEvent()
    {
        yield return new WaitForSeconds(holdTime);
        holdEvent?.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonPro))]
public class ButtonProEditor : UnityEditor.UI.ButtonEditor
{
    private SerializedProperty _allowHold;
    private SerializedProperty _holdTime;
    private SerializedProperty _onHold;

    private SerializedProperty _allowDoubleClick;
    private SerializedProperty _doubleClickInterval;
    private SerializedProperty _onDoubleClick;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        _allowHold = serializedObject.FindProperty("allowHold");
        _holdTime = serializedObject.FindProperty("holdTime");
        _onHold = serializedObject.FindProperty("onHold");
        _allowDoubleClick = serializedObject.FindProperty("allowDoubleClick");
        _doubleClickInterval = serializedObject.FindProperty("doubleClickInterval");
        _onDoubleClick = serializedObject.FindProperty("onDoubleClick");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonPro component = (ButtonPro)target;
        serializedObject.Update();

        EditorGUILayout.LabelField("[ CUSTOMIZED BUTTON PROPERTIES ]", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("> Hold Event:");
        EditorGUILayout.PropertyField(_allowHold);
        if (component.allowHold)
        {
            EditorGUILayout.PropertyField(_holdTime, new GUIContent("Hold Time (sec)"));
            EditorGUILayout.PropertyField(_onHold);
        }

        EditorGUILayout.LabelField("> Double Click Event:");
        EditorGUILayout.PropertyField(_allowDoubleClick);
        if (component.allowDoubleClick)
        {
            EditorGUILayout.PropertyField(_doubleClickInterval, new GUIContent("Double Click Interval (sec)"));
            EditorGUILayout.PropertyField(_onDoubleClick);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif