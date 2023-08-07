using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class Database : Singleton<Database>
{
    [HorizontalGroup("ApiGithub"), SerializeField, LabelWidth(125)] private string apiUrl = "https://opensheet.elk.sh/";
    [HorizontalGroup("GoogleSheet"), SerializeField, LabelWidth(125)] private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";
    
    [FoldoutGroup("DB"), SerializeField] private HeroDatabase heroDB;
    [FoldoutGroup("DB"), SerializeField] private DevilDatabase devilDB;
    [FoldoutGroup("DB"), SerializeField] private EquipmentDatabase eqmDB;
    [FoldoutGroup("DB"), SerializeField] private StatsDescriptions statsDesc;
    [FoldoutGroup("DB"), SerializeField] private GrowthDatabase growthDB;
    [FoldoutGroup("DB"), SerializeField] private ExpDatabase expDB;
    [FoldoutGroup("DB"), SerializeField] private BackstoryDatabase bsDB;
    [FoldoutGroup("DB"), SerializeField] private AuraDatabase auraDB;

    private const string HERO_SHEET = "Heroes";
    private const string DEVIL_SHEET = "Devils";
    private const string EQM_SHEET = "Equipments";
    private const string STATS_SHEET = "Stats";
    private const string ENTITY_GROWTH_SHEET = "EntityGrowth";
    private const string EQM_GROWTH_SHEET = "EquipmentGrowth";
    private const string EXP_SHEET = "Exp";
    private const string BACKSTORY_SHEET = "Backstory";
    private const string RACE_AURA_SHEET = "RaceAura";
    private const string ELEMENT_AURA_SHEET = "ElementAura";

    protected override void Awake()
    {
        base.Awake();
        FetchData();
    }

    [HorizontalGroup("ApiGithub", Width = 0.1f), Button("Open")]
    public void OpenApiGithub()
    {
        Application.OpenURL("https://github.com/benborgers/opensheet");
    }

    [HorizontalGroup("GoogleSheet", Width = 0.1f), Button("Open")]
    public void OpenGoogleSheet()
    {
        Application.OpenURL($"https://docs.google.com/spreadsheets/d/{databaseId}/");
    }

    [Button]
    public void FetchData()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;
        
        StartCoroutine(FetchHeroDB());
        StartCoroutine(FetchDevilDB());
        StartCoroutine(FetchEquipmentDB());
        StartCoroutine(FetchStatsDescriptions());
        StartCoroutine(FetchGrowthDB());
        StartCoroutine(FetchExpDB());
        StartCoroutine(FetchBackstoryDB());
        StartCoroutine(FetchAuraDB());
    }

    #region Data fetching
    
    IEnumerator FetchHeroDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{HERO_SHEET}");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            heroDB.Import(uwr.downloadHandler.text);
        }
    }
    
    IEnumerator FetchDevilDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{DEVIL_SHEET}");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(uwr.error);
        }
        else
        {
            devilDB.Import(uwr.downloadHandler.text);
        }
    }
    
    IEnumerator FetchEquipmentDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EQM_SHEET}");
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
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{STATS_SHEET}");
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

    IEnumerator FetchGrowthDB()
    {
        var cUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{ENTITY_GROWTH_SHEET}");
        var eUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EQM_GROWTH_SHEET}");
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

        growthDB.Import(dataC, dataE);
    }
    
    IEnumerator FetchExpDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EXP_SHEET}");
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
    
    IEnumerator FetchBackstoryDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{BACKSTORY_SHEET}");
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
    
    IEnumerator FetchAuraDB()
    {
        var rUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{RACE_AURA_SHEET}");
        var eUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{ELEMENT_AURA_SHEET}");
        var dataR = "";
        yield return rUwr.SendWebRequest();
        if (rUwr.result != UnityWebRequest.Result.Success)
        {
            EditorLog.Error(rUwr.error);
        }
        else
        {
            dataR = rUwr.downloadHandler.text;
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

        auraDB.Import(dataR, dataE);
    }
    
    #endregion

    #region Characters & Equipments
    
    public float GetEntityGrowth(Tier t)
    {
        return growthDB.entityGrowthList.Find(x => x.tier == t).growth;
    }
    public float GetEquipmentGrowth(Rarity r)
    {
        return growthDB.eqmGrowthList.Find(x => x.rarity == r).growth;
    }

    public BaseHero GetHeroWithID(string id)
    {
        return heroDB.GetHeroWithID(id);
    }

    public BaseDevil GetDevilWithID(string id)
    {
        return devilDB.GetDevilWithID(id);
    }

    public BaseEquipment GetEquipmentWithID(string id)
    {
        return eqmDB.GetEquipmentWithID(id);
    }
    
    #endregion

    #region EXP
    
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
    
    #endregion

    #region Stats
    
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
    
    #endregion

    #region Backstory
    
    public string GetHeroAlias(string heroId)
    {
        return bsDB.GetBackstory(heroId).alias;
    }
    
    public string GetHeroStory(string heroId)
    {
        return bsDB.GetBackstory(heroId).story;
    }
    
    #endregion

    #region Aura
    
    public List<Aura> GetRaceAura(Race race)
    {
        return auraDB.raceAuraList.Find(x => x.race == race).auraList;
    }

    public List<Aura> GetElementAura(Element element)
    {
        return auraDB.elementAuraList.Find(x => x.element == element).auraList;
    }
    
    #endregion
}

public static class DatabaseExtensions
{
    public static float GetHeroGrowth(this BaseHero h)
    {
        return Database.Instance.GetEntityGrowth(h.tier);
    }

    public static float GetDevilGrowth(this BaseDevil d)
    {
        return Database.Instance.GetEntityGrowth(d.tier);
    }

    public static string GetHeroAlias(this BaseHero h)
    {
        return Database.Instance.GetHeroAlias(h.id);
    }

    public static string GetHeroStory(this BaseHero h)
    {
        return Database.Instance.GetHeroStory(h.id);
    }

    public static float GetEquipmentGrowth(this BaseEquipment e)
    {
        return Database.Instance.GetEquipmentGrowth(e.rarity);
    }

    public static BaseHero GetHeroWithID(this HeroSaveData hsd)
    {
        return Database.Instance.GetHeroWithID(hsd.heroId);
    }

    public static int GetLevel(this HeroSaveData hsd)
    {
        return Database.Instance.GetLevel(hsd.totalExp);
    }

    public static Tuple<int, int> GetExp(this HeroSaveData hsd)
    {
        return Database.Instance.GetExp(hsd.totalExp);
    }
}
