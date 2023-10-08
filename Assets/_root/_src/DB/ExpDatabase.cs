using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;

public class ExpDatabase : DuztineBehaviour
{
    public int levelMax;

    [TableList]
    public List<ExpData> expList = new();

    public void Init(string data)
    {
        var jArray = JArray.Parse(data);
        var totalExp = 0;
        foreach (var jToken in jArray)
        {
            var jObject = (JObject)jToken;
            var expData = new ExpData
            {
                level = Common.Parse<int>((string)jObject["level"]),
                exp = Common.Parse<int>((string)jObject["exp"]),
                totalExp = totalExp
            };

            expList.Add(expData);
            totalExp += expData.exp;
        }
    }

    /*
    [Button]
    public void AutoGen(int num)
    {
        // =100+10*(A3-1)+pow(A3-1;2)+100*((INT(A3/10)))
        int sum = 0;
        int lastLevelExp = 0;
        for (int i = 0; i < num; i++)
        {
            int exp = 100 + 10 * i + (int)Mathf.Pow(i, 2) + 100 * ((i + 1) / 10);
            sum += lastLevelExp;
            lastLevelExp = exp;
            expList.Add(new ExpData
            {
                level = i + 1,
                exp = exp,
                totalExp = sum
            });
        }
    }
    */

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
}