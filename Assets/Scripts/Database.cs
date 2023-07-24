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
    [SerializeField] private ExpDatabase expDB;
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
            expDB.Import(uwr.downloadHandler.text);
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
        return charDB.GetCharacterWithID(id);
    }

    public BaseEquipment GetEquipmentWithID(string id)
    {
        return eqmDB.GetEquipmentWithID(id);
    }

    [Button]
    public int GetLevel(int totalExp)
    {
        return expDB.GetLevel(totalExp);
    }
    
    [Button]
    public Tuple<int, int> GetExp(int totalExp)
    {
        return expDB.GetExp(totalExp);
    }

    public int GetLevelMax()
    {
        return expDB.levelMax;
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

    public string GetCharacterAlias(string charId)
    {
        return bsDB.GetBackstory(charId).alias;
    }
    
    public string GetCharacterStory(string charId)
    {
        return bsDB.GetBackstory(charId).story;
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
