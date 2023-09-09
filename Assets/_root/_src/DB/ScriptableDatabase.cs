using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ScriptableDatabase : ScriptableObject, IDatabase
{
    [SerializeField] private string databaseName;
    [SerializeField] private string sheet;

    public string DatabaseName => databaseName;
    public string Sheet => sheet;

    [Button]
    public abstract void Import();

    [Button]
    public abstract void Delete();
}

public interface IDatabase
{
    string DatabaseName { get; }
    string Sheet { get; }

    void Import();
    void Delete();
}

public static class DatabaseExtensions
{
    public static string FetchFromLocal(this ScriptableDatabase db)
    {
        return Resources.Load<TextAsset>(db.Sheet)?.text ?? null;
    }

    public static string FetchFromCloud(this ScriptableDatabase db)
    {
        return null;
    }
}