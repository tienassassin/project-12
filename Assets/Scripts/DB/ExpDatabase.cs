using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace System.DB
{
    [CreateAssetMenu(fileName = "ExpDatabase", menuName = "Database/Exp")]
    public class ExpDatabase : ScriptableDatabase
    {
        internal int LevelMax;
        
        [TableList, ShowInInspector] 
        private List<ExpData> _expList = new();

        internal override void Import(params string[] data)
        {
            int totalExp = 0;
            int lastLevelExp = 0;
            _expList = new List<ExpData>();
            var jArray = JArray.Parse(data[0]);
            LevelMax = jArray.Count;
            for (int i = 0; i < LevelMax; i++)
            {
                var jObject = (JObject)jArray[i];
                int exp = Utils.Parse<int>((string)jObject["exp"]);
                if (i > 0) totalExp += lastLevelExp;
                lastLevelExp = exp;
                _expList.Add(new ExpData(i + 1, exp, totalExp));
            }
        }

        [Button]
        internal override void DeleteAll()
        {
            _expList.Clear();
        }

        internal int GetLevel(int totalExp)
        {
            for (int i = _expList.Count - 1; i >= 0; i--)
            {
                if (totalExp >= _expList[i].TotalExp)
                {
                    return _expList[i].Level;
                }
            }

            return 1;
        }

        internal Tuple<int, int> GetExp(int totalExp)
        {
            for (int i = _expList.Count - 1; i >= 0; i--)
            {
                if (totalExp >= _expList[i].TotalExp)
                {
                    return Tuple.Create(totalExp - _expList[i].TotalExp, _expList[i].Exp);
                }
            }

            return Tuple.Create(0, _expList[0].Exp);
        }
    }

    [Serializable]
    public struct ExpData
    {
        public int Level;
        public int Exp;
        public int TotalExp;

        public ExpData(int level, int exp, int totalExp)
        {
            Level = level;
            Exp = exp;
            TotalExp = totalExp;
        }
    }
}
