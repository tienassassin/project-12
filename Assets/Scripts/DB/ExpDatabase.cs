using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpDatabase",menuName = "Database/Exp")]
public class ExpDatabase : ScriptableDatabase
{
    public int levelMax;
    
    [TableList]
    public List<ExpData> expList = new();

    public override void Import(params string[] data)
    {
        int totalExp = 0;
        int lastLevelExp = 0;
        expList = new List<ExpData>();
        var jArray = JArray.Parse(data[0]);
        levelMax = jArray.Count;
        for (int i=0; i<levelMax; i++)
        {
            var jObject = (JObject)jArray[i];
            int exp = Utils.Parse<int>((string)jObject["exp"]);
            if (i > 0) totalExp += lastLevelExp;
            lastLevelExp = exp;
            expList.Add(new ExpData(i + 1, exp, totalExp));
        }
    }

    [Button]
    protected override void DeleteAll()
    {
        expList.Clear();
    }
}

[Serializable]
public struct ExpData
{
    public int level;
    public int exp;
    public int totalExp;

    public ExpData(int level, int exp, int totalExp)
    {
        this.level = level;
        this.exp = exp;
        this.totalExp = totalExp;
    }
}
