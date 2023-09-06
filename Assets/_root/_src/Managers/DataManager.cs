using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public static bool Ready { get; private set; }

    [SerializeField] [HorizontalGroup("ApiGithub")] [LabelWidth(125)]
    private string apiUrl = "https://opensheet.elk.sh/";
    [SerializeField] [HorizontalGroup("GoogleSheet")] [LabelWidth(125)]
    private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";

    [SerializeField] [FoldoutGroup("DB")] private EntityDatabase entityDB;
    [SerializeField] [FoldoutGroup("DB")] private EquipmentDatabase eqmDB;
    [SerializeField] [FoldoutGroup("DB")] private StatDatabase statDB;
    [SerializeField] [FoldoutGroup("DB")] private GrowthDatabase growthDB;
    [SerializeField] [FoldoutGroup("DB")] private ExpDatabase expDB;
    [SerializeField] [FoldoutGroup("DB")] private BackstoryDatabase storyDB;
    [SerializeField] [FoldoutGroup("DB")] private AuraDatabase auraDB;

    private int _loadedDBCount;
    private const int TOTAL_DB_COUNT = 7;

    protected override void Awake()
    {
        base.Awake();
        Ready = false;
    }

    [HorizontalGroup("ApiGithub", Width = 0.1f)] [Button("Open")]
    public void OpenApiGithub()
    {
        Application.OpenURL("https://github.com/benborgers/opensheet");
    }

    [HorizontalGroup("GoogleSheet", Width = 0.1f)] [Button("Open")]
    public void OpenGoogleSheet()
    {
        Application.OpenURL($"https://docs.google.com/spreadsheets/d/{databaseId}/");
    }

    public void NotifyDBLoaded(string dbName, int time)
    {
        _loadedDBCount++;
        // EditorLog.Message($"({_loadedDBCount}/{TOTAL_DB_COUNT}) Loaded {dbName}, elapsed time: {time}ms");
        if (_loadedDBCount >= TOTAL_DB_COUNT)
        {
            EditorLog.Message("All Databases loaded!");
            Ready = true;
        }
    }

    #region Aura

    public List<Aura> GetAuras(object obj)
    {
        return auraDB.GetAuras(obj);
    }

    #endregion

    #region Characters & Equipments

    public List<Entity> GetAllEntities()
    {
        return entityDB.GetAllEntities();
    }

    public float GetGrowth(object obj)
    {
        return growthDB.GetGrowth(obj);
    }

    public Entity GetEntityWithID(string id)
    {
        return entityDB.GetEntityWithID(id);
    }

    public Equipment GetEquipmentWithID(string id)
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
        return statDB.GetStatDescription(key).description;
    }

    public string GetStatName(string key)
    {
        return statDB.GetStatDescription(key).name;
    }

    public float GetStatLimit(string key)
    {
        return statDB.GetStatDescription(key).limit;
    }

    #endregion

    #region Backstory

    public string GetHeroAlias(string heroId)
    {
        return storyDB.GetBackstory(heroId).alias;
    }

    public string GetHeroStory(string heroId)
    {
        return storyDB.GetBackstory(heroId).story;
    }

    #endregion
}

public static partial class DataExtensions
{
    public static Entity GetEntity(this EntitySaveData h)
    {
        return DataManager.Instance.GetEntityWithID(h.entityId);
    }

    public static Entity GetEntity(this EnemyData d)
    {
        return DataManager.Instance.GetEntityWithID(d.entityId);
    }

    public static int GetLevel(this EntitySaveData hsd)
    {
        return DataManager.Instance.GetLevel(hsd.totalExp);
    }

    public static Tuple<int, int> GetExp(this EntitySaveData hsd)
    {
        return DataManager.Instance.GetExp(hsd.totalExp);
    }
}