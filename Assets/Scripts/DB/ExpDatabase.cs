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

    public int GetLevel(int totalExp)
    {
        for (int i = expList.Count - 1; i >= 0; i--)
        {
            if (totalExp >= expList[i].totalExp)
            {
                return expList[i].level;
            }
        }

        return 1;
    }

    public Tuple<int, int> GetExp(int totalExp)
    {
        for (int i = expList.Count - 1; i >= 0; i--)
        {
            if (totalExp >= expList[i].totalExp)
            {
                return Tuple.Create(totalExp - expList[i].totalExp, expList[i].exp);
            }
        }

        return Tuple.Create(0, expList[0].exp);
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
