using UnityEngine;

public static class DebugLog
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Message(string msg)
    {
        Debug.Log(msg);
    }

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Error(string msg)
    {
        Debug.LogError(msg);
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