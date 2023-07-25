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

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition)
    {
        if (!condition) throw new UnityException();
    }
    
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Assert(bool condition, string msg)
    {
        if (!condition) throw new UnityException(msg);
    }
}