using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    // [SerializeField] [FoldoutGroup("DB")] private EquipmentDatabase eqmDB;
    [SerializeField] [FoldoutGroup("DB")] private StatDatabase statDB;
    [SerializeField] [FoldoutGroup("DB")] private GrowthDatabase growthDB;
    [SerializeField] [FoldoutGroup("DB")] private ExpDatabase expDB;
    [SerializeField] [FoldoutGroup("DB")] private AuraDatabase auraDB;

    private int _loadedDBCount;
    private const int TOTAL_DB_COUNT = 5;

    protected override void Awake()
    {
        base.Awake();
        Ready = false;
    }

    private async void Start()
    {
        await UniTask.Yield();
        Ready = true;
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

    #region Aura ================================================================

    public List<Aura> GetAuras(object obj)
    {
        return auraDB.GetAuras(obj);
    }

    #endregion

    #region Characters & Equipments ==============================================

    public List<EntityData> GetAllEntities()
    {
        return entityDB.GetAllEntities();
    }

    public float GetGrowth(object obj)
    {
        return growthDB.GetGrowth(obj);
    }

    public EntityData GetEntityWithID(string id)
    {
        return entityDB.GetEntityWithID(id);
    }

    // public Equipment GetEquipmentWithID(string id)
    // {
    //     return eqmDB.GetEquipmentWithID(id);
    // }

    #endregion

    #region EXP ==================================================================

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

    #region Stats ================================================================

    public StatInfo GetStatInfo(string id)
    {
        return statDB.GetStatInfo(id);
    }

    #endregion
}

public static partial class DataExtensions
{
    public static EntityData GetEntity(this MyEntity @this)
    {
        return DataManager.Instance.GetEntityWithID(@this.entityId);
    }

    public static EntityData GetEntity(this EnemyData @this)
    {
        return DataManager.Instance.GetEntityWithID(@this.entityId);
    }

    public static int GetLevel(this MyEntity @this)
    {
        return DataManager.Instance.GetLevel(@this.totalExp);
    }

    public static Tuple<int, int> GetExp(this MyEntity @this)
    {
        return DataManager.Instance.GetExp(@this.totalExp);
    }
}