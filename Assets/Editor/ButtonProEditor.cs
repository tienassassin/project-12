#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonPro))]
[CanEditMultipleObjects]
public class ButtonProEditor : ButtonEditor
{
    // todo: review later
    /*
     
    private SerializedProperty _holdTime;
    private SerializedProperty _onHold;

    private SerializedProperty _doubleClickInterval;
    private SerializedProperty _onDoubleClick;

    protected override void OnEnable()
    {
        base.OnEnable();

        _holdTime = serializedObject.FindProperty("holdTime");
        _onHold = serializedObject.FindProperty("holdEvent");
        _doubleClickInterval = serializedObject.FindProperty("doubleClickInterval");
        _onDoubleClick = serializedObject.FindProperty("doubleClickEvent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonPro component = (ButtonPro)target;
        serializedObject.Update();

        EditorGUILayout.LabelField("[ CUSTOMIZED BUTTON PROPERTIES ]", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("> Hold Event:");
        EditorGUILayout.PropertyField(_holdTime, new GUIContent("Hold Time (sec)"));
        EditorGUILayout.PropertyField(_onHold);

        EditorGUILayout.LabelField("> Double Click Event:");
        EditorGUILayout.PropertyField(_doubleClickInterval, new GUIContent("Double Click Interval (sec)"));
        EditorGUILayout.PropertyField(_onDoubleClick);

        serializedObject.ApplyModifiedProperties();
    }
    
    */
}
#endif