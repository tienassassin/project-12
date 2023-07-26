using UnityEngine;

public abstract class ScriptableDatabase : ScriptableObject
{
    public abstract void Import(params string[] data);
    protected abstract void DeleteAll();
}
