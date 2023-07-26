using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        return uHeroDB.allHeroList.FindAll(x => uHeroDB.readyHeroList.Contains(x.heroId));
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
public struct HeroSaveData
{
    public string heroId;
    public int totalExp;
    public float curHp;
    public float energy;
    public List<EquipmentSaveData> eqmList;
}

[Serializable]
public struct EquipmentSaveData
{
    public string eqmId;
    public int level;
}