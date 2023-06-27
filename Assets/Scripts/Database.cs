using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class Database : MonoBehaviour
{
    [SerializeField] private string apiUrl = "https://opensheet.elk.sh/";
    [SerializeField] private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";
    [SerializeField] private ScriptableDatabase charDB;
    [SerializeField] private ScriptableDatabase eqmDB;
    [SerializeField] private ScriptableDatabase statsDesc;
    [SerializeField] private ScriptableDatabase lvlBonusLeg;

    private void Start()
    {
        FetchData();
    }

    [Button]
    public void FetchData()
    {
        StartCoroutine(FetchCharacterDB());
        StartCoroutine(FetchEquipmentDB());
        StartCoroutine(FetchStatsDescriptions());
        StartCoroutine(FetchLevelBonusLegend());
    }

    IEnumerator FetchCharacterDB()
    {
        var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/Characters");
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(uwr.error);
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
            Debug.LogError(uwr.error);
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
            Debug.LogError(uwr.error);
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
            Debug.LogError(cUwr.error);
        }
        else
        {
            dataC = cUwr.downloadHandler.text;
        }
        
        var dataE = "";
        yield return eUwr.SendWebRequest();
        if (eUwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(eUwr.error);
        }
        else
        {
            dataE = eUwr.downloadHandler.text;
        }

        lvlBonusLeg.Import(dataC, dataE);
    }
}
