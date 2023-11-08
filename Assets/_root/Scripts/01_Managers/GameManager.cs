using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] [FoldoutGroup("Databases")] private GrowthDatabase growthDB;
    [SerializeField] [FoldoutGroup("Databases")] private ExpDatabase expData;
    [SerializeField] [FoldoutGroup("Databases")] private EntityDatabase entityDB;

    public void LoadGameDataFromCloud()
    {
        PlayFabManager.Instance.LoadGameData(dict =>
        {
            foreach (var pair in dict)
            {
                DebugLog.Message(pair.Key + " " + pair.Value);
            }

            growthDB.Init(dict["growth"]);
            expData.Init(dict["exp"]);
            entityDB.Init(dict["entity"]);
        });
    }
}