using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : Singleton<PlayerManager>
{
    private const string HERO_DB_KEY = "HERO_DB";
    [Header("DUMMY")]
    [SerializeField] private bool isDummy;
    [SerializeField] private int unlockedHeroNum = 4;

    [Header("DATA")]
    [SerializeField] private HeroSaveData heroDB;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 144;
        if (!isDummy) LoadHeroDB();
        else GenerateDummyData();
    }

    public List<HeroData> GetAllHeroes()
    {
        return heroDB.unlockedHeroes;
    }

    public List<HeroData> GetReadyHeroes()
    {
        var readyHeroList = new List<HeroData>();
        heroDB.readyHeroes.ForEach(x => { readyHeroList.Add(heroDB.unlockedHeroes.Find(y => y.heroId == x)); });

        return readyHeroList;
    }

    public void AddHeroToLineUp(int slotId, string heroId)
    {
        int oldSlotId = heroDB.readyHeroes.IndexOf(heroId);
        string oldHeroId = heroDB.readyHeroes[slotId];
        if (oldSlotId >= 0)
        {
            heroDB.readyHeroes[oldSlotId] = oldHeroId;
        }

        heroDB.readyHeroes[slotId] = heroId;
        SaveCharacterDB();

        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public void RemoveHeroFromLineUp(int slotId)
    {
        heroDB.readyHeroes[slotId] = "";
        SaveCharacterDB();

        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public bool IsHeroReady(string heroId)
    {
        return heroDB.readyHeroes.Contains(heroId);
    }

    public bool IsHeroUnlocked(string heroId, out HeroData hsd)
    {
        hsd = heroDB.unlockedHeroes.Find(h => h.heroId == heroId);
        return hsd != null;
    }

    public void LoadHeroDB()
    {
        if (PlayerPrefs.HasKey(HERO_DB_KEY))
        {
            heroDB = JsonUtility.FromJson<HeroSaveData>(PlayerPrefs.GetString(HERO_DB_KEY));
        }
        else
        {
            heroDB = new HeroSaveData();
            SaveCharacterDB();
        }
    }

    private async void GenerateDummyData()
    {
        await UniTask.WaitUntil(() => DataManager.Ready);
        heroDB = new HeroSaveData();
        var allHeroIds = DataManager.Instance.GetAllHeroes().Select(x => x.id).ToList();
        for (int i = 0; i < unlockedHeroNum; i++)
        {
            string id = allHeroIds[Random.Range(0, allHeroIds.Count)];
            allHeroIds.Remove(id);
            heroDB.unlockedHeroes.Add(new HeroData
            {
                heroId = id,
                totalExp = Random.Range(1, 10000),
                curHp = Random.Range(0, 1),
                energy = Random.Range(0, 1),
                eqmList = null
            });

            if (i < 4)
            {
                heroDB.readyHeroes.Add(id);
            }
        }
    }

    [Button]
    public void SaveCharacterDB()
    {
        PlayerPrefs.SetString(HERO_DB_KEY, JsonUtility.ToJson(heroDB));
    }

    [Button]
    public void DeleteUserDB()
    {
        PlayerPrefs.DeleteKey(HERO_DB_KEY);
    }
}

[Serializable]
public class HeroSaveData
{
    [TableList(ShowIndexLabels = true)]
    public List<HeroData> unlockedHeroes = new();

    public List<string> readyHeroes = new();
}

[Serializable]
public class HeroData
{
    [VerticalGroup("Information")]
    public string heroId;

    [VerticalGroup("Information")]
    public int totalExp;

    [VerticalGroup("Information")]
    public float curHp;

    [VerticalGroup("Information")]
    public float energy;

    public List<EquipmentData> eqmList;
}

[Serializable]
public class EquipmentData
{
    public string eqmId;
    public int level;
}