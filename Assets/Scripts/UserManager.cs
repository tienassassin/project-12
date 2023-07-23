using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UserManager : Singleton<UserManager>
{
    public UserCharacterDB chrDB;

    private const string CHARACTER_DB_KEY = "CHARACTER_DB";

    protected override void Awake()
    {
        base.Awake();
        
        LoadCharacterDB();
    }

    public List<CharacterSaveData> GetAllCharacters()
    {
        return chrDB.allChrList;
    }

    public List<CharacterSaveData> GetReadyCharacters()
    {
        return chrDB.allChrList.FindAll(x => chrDB.readyChrList.Contains(x.chrId));
    }

    public void LoadCharacterDB()
    {
        if (PlayerPrefs.HasKey(CHARACTER_DB_KEY))
        {
            chrDB = JsonUtility.FromJson<UserCharacterDB>(PlayerPrefs.GetString(CHARACTER_DB_KEY));
        }
        else
        {
            chrDB = new UserCharacterDB();
            SaveCharacterDB();
        }
    }

    [Button]
    public void SaveCharacterDB()
    {
        PlayerPrefs.SetString(CHARACTER_DB_KEY, JsonUtility.ToJson(chrDB));
    }

    [Button]
    public void DeleteUserDB()
    {
        PlayerPrefs.DeleteKey(CHARACTER_DB_KEY);
    }
}

[Serializable]
public class UserCharacterDB
{
    public List<CharacterSaveData> allChrList = new();
    public List<string> readyChrList = new();
}

[Serializable]
public struct CharacterSaveData
{
    public string chrId;
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