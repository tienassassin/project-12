using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDatabase", menuName = "DB/StatDatabase")]
public class StatDatabase : ScriptableDatabase
{
    [TableList]
    public List<StatInfo> stats = new();

    public override void Import()
    {
    }

    public override void Delete()
    {
        stats.Clear();
    }

    public StatInfo GetStatInfo(string id)
    {
        return stats.Find(x => x.id.Equals(id));
    }
}

[Serializable]
public struct StatInfo
{
    [TableColumnWidth(100, Resizable = false)]
    public string id;

    [TableColumnWidth(100, Resizable = false)]
    public string name;

    [TableColumnWidth(70, Resizable = false)]
    public float limit;

    [TextArea(3, 10)]
    public string description;
}