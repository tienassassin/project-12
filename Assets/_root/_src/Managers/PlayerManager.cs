using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : Singleton<PlayerManager>
{
    public int level;
    public int exp;
    
    private const string HERO_DB_KEY = "HERO_DB";
    [Header("DUMMY")]
    [SerializeField] private bool isDummy;
    [SerializeField] private int unlockedHeroNum = 4;

    [Header("DATA")]
    [SerializeField] private EntityCollection entityCollection;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 144;
        if (!isDummy) LoadHeroDB();
        else GenerateDummyData();
    }

    [Button]
    public void CheckPlayerID()
    {
        PlayFabManager.Instance.Load(PlayerKey.PLAYER_ID, data =>
        {
            if (data.IsNullOrWhitespace())
            {
                // New player, init default data
                PlayFabManager.Instance.Save(PlayerKey.PLAYER_ID, SystemInfo.deviceUniqueIdentifier);
            }
        });
    }

    public List<MyEntity> GetAllHeroes()
    {
        return entityCollection.myEntities;
    }

    public List<MyEntity> GetReadyHeroes()
    {
        var readyHeroList = new List<MyEntity>();
        entityCollection.readyEntities.ForEach(x =>
        {
            readyHeroList.Add(entityCollection.myEntities.Find(y => y.entityId == x));
        });

        return readyHeroList;
    }

    public void AddHeroToLineUp(int slotId, string heroId)
    {
        int oldSlotId = entityCollection.readyEntities.IndexOf(heroId);
        string oldHeroId = entityCollection.readyEntities[slotId];
        if (oldSlotId >= 0)
        {
            entityCollection.readyEntities[oldSlotId] = oldHeroId;
        }

        entityCollection.readyEntities[slotId] = heroId;
        SaveCharacterDB();

        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public void RemoveHeroFromLineUp(int slotId)
    {
        entityCollection.readyEntities[slotId] = "";
        SaveCharacterDB();

        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public bool IsHeroReady(string heroId)
    {
        return entityCollection.readyEntities.Contains(heroId);
    }

    public bool IsHeroUnlocked(string heroId, out MyEntity hsd)
    {
        hsd = entityCollection.myEntities.Find(h => h.entityId == heroId);
        return hsd != null;
    }

    public void LoadHeroDB()
    {
        if (PlayerPrefs.HasKey(HERO_DB_KEY))
        {
            entityCollection = JsonUtility.FromJson<EntityCollection>(PlayerPrefs.GetString(HERO_DB_KEY));
        }
        else
        {
            entityCollection = new EntityCollection();
            SaveCharacterDB();
        }
    }

    private async void GenerateDummyData()
    {
        await UniTask.WaitUntil(() => DataManager.Ready);
        entityCollection = new EntityCollection();
        var allHeroIds = DataManager.Instance.GetAllEntities().Select(x => x.info.id).ToList();
        for (int i = 0; i < unlockedHeroNum; i++)
        {
            string id = allHeroIds[Random.Range(0, allHeroIds.Count)];
            allHeroIds.Remove(id);
            entityCollection.myEntities.Add(new MyEntity
            {
                entityId = id,
                totalExp = Random.Range(1, 10000),
                currentHp = Random.Range(0, 1),
                energy = Random.Range(0, 1)
                // eqmList = null
            });

            if (i < 4)
            {
                entityCollection.readyEntities.Add(id);
            }
        }
    }

    [Button]
    public void SaveCharacterDB()
    {
        PlayerPrefs.SetString(HERO_DB_KEY, JsonUtility.ToJson(entityCollection));
    }

    [Button]
    public void DeleteUserDB()
    {
        PlayerPrefs.DeleteKey(HERO_DB_KEY);
    }
}

[Serializable]
public class EntityCollection
{
    [TableList(ShowIndexLabels = true)]
    public List<MyEntity> myEntities = new();

    public List<string> readyEntities = new();
}

[Serializable]
public class MyEntity
{
    [VerticalGroup("Information")]
    public string entityId;

    [VerticalGroup("Information")]
    public int totalExp;

    [VerticalGroup("Information")]
    [ProgressBar(0, 100)]
    public int currentHp;

    [VerticalGroup("Information")]
    [ProgressBar(0, 100)]
    public int energy;

    // todo: review later
    // public List<EquipmentData> eqmList;
}

[Serializable]
public class EquipmentData
{
    public string eqmId;
    public int level;
}