using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class Database : Singleton<Database>
{
    [SerializeField] private string apiUrl = "https://opensheet.elk.sh/";
    [SerializeField] private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";
    [SerializeField] private CharacterDatabase charDB;
    [SerializeField] private EquipmentDatabase eqmDB;
    [SerializeField] private StatsDescriptions statsDesc;
    [SerializeField] private LevelBonusLegend lvlBonusLeg;
    [SerializeField] private ExpDatabase expDatabase;
    [SerializeField] private BackstoryDatabase bsDB;

    protected override void Awake()
    {
        base.Awake();
        FetchData();
    }

    [Button]
    public void OpenGoogleSheet()
    {
        Application.OpenURL($"https://docs.google.com/spreadsheets/d/{databaseId}/");
    }

    [Button]
    public void FetchData()
    {
        StartCoroutine(FetchCharacterDB());
        StartCoroutine(FetchEquipmentDB());
        StartCoroutine(FetchStatsDescriptions());
        StartCoroutine(FetchLevelBonusLegend());
        StartCoroutine(FetchExpDatabase());
        StartCoroutine(FetchBackstoryDatabase());
    }

    #region Data fetching
    
    IEnumerator FetchCharacterDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Characters");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            charDB.Import(uwr.downloadHandler.text);
        }
    }
    
    IEnumerator FetchEquipmentDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Equipments");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            eqmDB.Import(uwr.downloadHandler.text);
        }
    }

    IEnumerator FetchStatsDescriptions()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Stats");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            statsDesc.Import(uwr.downloadHandler.text);
        }
    }

    IEnumerator FetchLevelBonusLegend()
    {
        var cUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/CharacterGrowth");
        var eUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/EquipmentGrowth");
        var dataC = "";
        yield return cUwr.SendWebRequest();
        if (cUwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(cUwr.error);
        }
        else
        {
            dataC = cUwr.downloadHandler.text;
        }
        
        var dataE = "";
        yield return eUwr.SendWebRequest();
        if (eUwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(eUwr.error);
        }
        else
        {
            dataE = eUwr.downloadHandler.text;
        }

        lvlBonusLeg.Import(dataC, dataE);
    }
    
    IEnumerator FetchExpDatabase()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Exp");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            expDatabase.Import(uwr.downloadHandler.text);
        }
    }
    
    IEnumerator FetchBackstoryDatabase()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Backstory");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            bsDB.Import(uwr.downloadHandler.text);
        }
    }
    
    #endregion
    
    public float GetCharacterGrowth(Tier t)
    {
        return lvlBonusLeg.chrGrowthList.Find(x => x.tier == t).growth;
    }
    public float GetEquipmentGrowth(Rarity r)
    {
        return lvlBonusLeg.eqmGrowthList.Find(x => x.rarity == r).growth;
    }

    public BaseCharacter GetCharacterWithID(string id)
    {
        return charDB.charList.Find(x => x.id == id);
    }

    public BaseEquipment GetEquipmentWithID(string id)
    {
        return eqmDB.eqmList.Find(x => x.id == id);
    }

    [Button]
    public int GetLevel(int totalExp)
    {
        var expList = expDatabase.expList;
        for (int i = expList.Count - 1; i >= 0; i--)
        {
            if (totalExp >= expList[i].totalExp)
            {
                return expList[i].level;
            }
        }

        return 1;
    }
    
    [Button]
    public Tuple<int, int> GetExp(int totalExp)
    {
        var expList = expDatabase.expList;
        for (int i = expList.Count - 1; i >= 0; i--)
        {
            if (totalExp >= expList[i].totalExp)
            {
                return new Tuple<int, int>(totalExp - expList[i].totalExp, expList[i].exp);
            }
        }

        return new Tuple<int, int>(0, expList[0].exp);
    }

    public int GetLevelMax()
    {
        return expDatabase.levelMax;
    }

    public string GetStatDescription(string key)
    {
        return statsDesc.GetStatDescription(key).description;
    }

    public string GetStatName(string key)
    {
        return statsDesc.GetStatDescription(key).name;
    }
    
    public float GetStatLimit(string key)
    {
        return statsDesc.GetStatDescription(key).limit;
    }
}

public static class DatabaseExtension
{
    public static float GetCharacterGrowth(this BaseCharacter chr)
    {
        return Database.Instance.GetCharacterGrowth(chr.tier);
    }

    public static float GetEquipmentGrowth(this BaseEquipment eqm)
    {
        return Database.Instance.GetEquipmentGrowth(eqm.rarity);
    }

    public static int GetLevel(this CharacterSaveData csd)
    {
        return Database.Instance.GetLevel(csd.totalExp);
    }

    public static Tuple<int, int> GetExp(this CharacterSaveData csd)
    {
        return Database.Instance.GetExp(csd.totalExp);
    }
}
