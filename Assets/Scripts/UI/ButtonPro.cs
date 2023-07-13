using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonPro : Button
{
    public bool allowHold = true;
    public float holdTime = 1;
    public UnityEvent onHold = new ();
    private Coroutine coroutine;
    
    public bool allowDoubleClick = false;
    public float doubleClickInterval = 0.2f;
    public UnityEvent onDoubleClick = new();
    private float lastTimeClick;
    private bool firstClick;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        coroutine = StartCoroutine(InvokeHoldEvent());
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (coroutine != null) StopCoroutine(coroutine);

        if (!firstClick)
        {
            firstClick = true;
            lastTimeClick = Time.unscaledTime;
        }
        else
        {
            if (Time.unscaledTime - lastTimeClick <= doubleClickInterval)
            {
                onDoubleClick?.Invoke();
                firstClick = false;
            }
            else
            {
                lastTimeClick = Time.unscaledTime;
            }
        }
    }

    IEnumerator InvokeHoldEvent()
    {
        yield return new WaitForSeconds(holdTime);
        onHold?.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonPro))]
public class ButtonProEditor : UnityEditor.UI.ButtonEditor
{
    private SerializedProperty allowHold;
    private SerializedProperty holdTime;
    private SerializedProperty onHold;

    private SerializedProperty allowDoubleClick;
    private SerializedProperty doubleClickInterval;
    private SerializedProperty onDoubleClick;

    protected override void OnEnable()
    {
        base.OnEnable();
        
        allowHold = serializedObject.FindProperty("allowHold");
        holdTime = serializedObject.FindProperty("holdTime");
        onHold = serializedObject.FindProperty("onHold");
        allowDoubleClick = serializedObject.FindProperty("allowDoubleClick");
        doubleClickInterval = serializedObject.FindProperty("doubleClickInterval");
        onDoubleClick = serializedObject.FindProperty("onDoubleClick");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonPro component = (ButtonPro)target;
        serializedObject.Update();

        EditorGUILayout.LabelField("[ CUSTOMIZED BUTTON PROPERTIES ]", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("> Hold Event:");
        EditorGUILayout.PropertyField(allowHold);
        if (component.allowHold)
        {
            EditorGUILayout.PropertyField(holdTime, new GUIContent("Hold Time (sec)"));
            EditorGUILayout.PropertyField(onHold);
        }

        EditorGUILayout.LabelField("> Double Click Event:");
        EditorGUILayout.PropertyField(allowDoubleClick);
        if (component.allowDoubleClick)
        {
            EditorGUILayout.PropertyField(doubleClickInterval, new GUIContent("Double Click Interval (sec)"));
            EditorGUILayout.PropertyField(onDoubleClick);
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif