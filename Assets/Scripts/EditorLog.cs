using UnityEngine;

public static class EditorLog
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Message(string msg)
    {
        Debug.Log(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Error(string msg)
    {
        Debug.Log(msg);
    }
}