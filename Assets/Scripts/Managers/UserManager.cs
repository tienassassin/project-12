using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class UserManager : Singleton<UserManager>
{
    public UserHeroDB uHeroDB;

    private const string HERO_DB_KEY = "HERO_DB";

    protected override void Awake()
    {
        base.Awake();
        
        Application.targetFrameRate = 144;
        LoadHeroDB();
    }

    public List<HeroSaveData> GetAllHeroes()
    {
        return uHeroDB.allHeroList;
    }

    public List<HeroSaveData> GetReadyHeroes()
    {
        var readyHeroList = new List<HeroSaveData>();
        uHeroDB.readyHeroList.ForEach(x =>
        {
            readyHeroList.Add(uHeroDB.allHeroList.Find(y => y.heroId == x));
        });

        return readyHeroList;
    }

    public void AddHeroToLineUp(int slotId, string heroId)
    {
        int oldSlotId = uHeroDB.readyHeroList.IndexOf(heroId);
        string oldHeroId = uHeroDB.readyHeroList[slotId];
        if (oldSlotId >= 0)
        {
            uHeroDB.readyHeroList[oldSlotId] = oldHeroId;
        }
        
        uHeroDB.readyHeroList[slotId] = heroId;
        SaveCharacterDB();
        
        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public void RemoveHeroFromLineUp(int slotId)
    {
        uHeroDB.readyHeroList[slotId] = "";
        SaveCharacterDB();
        
        this.PostEvent(EventID.ON_LINEUP_CHANGED);
    }

    public bool IsHeroReady(string heroId)
    {
        return uHeroDB.readyHeroList.Contains(heroId);
    }

    public bool IsHeroUnlocked(string heroId, out HeroSaveData hsd)
    {
        hsd = uHeroDB.allHeroList.Find(h => h.heroId == heroId);
        return hsd != null;
    }

    public void LoadHeroDB()
    {
        if (PlayerPrefs.HasKey(HERO_DB_KEY))
        {
            uHeroDB = JsonUtility.FromJson<UserHeroDB>(PlayerPrefs.GetString(HERO_DB_KEY));
        }
        else
        {
            uHeroDB = new UserHeroDB();
            SaveCharacterDB();
        }
    }

    [Button]
    public void SaveCharacterDB()
    {
        PlayerPrefs.SetString(HERO_DB_KEY, JsonUtility.ToJson(uHeroDB));
    }

    [Button]
    public void DeleteUserDB()
    {
        PlayerPrefs.DeleteKey(HERO_DB_KEY);
    }
}

[Serializable]
public class UserHeroDB
{
    public List<HeroSaveData> allHeroList = new();
    public List<string> readyHeroList = new();
}

[Serializable]
public class HeroSaveData
{
    public string heroId;
    public int totalExp;
    public float curHp;
    public float energy;
    public List<EquipmentSaveData> eqmList;
}

[Serializable]
public class EquipmentSaveData
{
    public string eqmId;
    public int level;
}