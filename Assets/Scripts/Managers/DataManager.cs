using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace DB.System
{
    public class DataManager : Singleton<DataManager>
    {
        public bool AllDBLoaded { get; private set; }

        [HorizontalGroup("ApiGithub"), SerializeField, LabelWidth(125)] private string apiUrl = "https://opensheet.elk.sh/";
        [HorizontalGroup("GoogleSheet"), SerializeField, LabelWidth(125)] private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";
        
        [FoldoutGroup("DB"), SerializeField] private HeroDatabase heroDB;
        [FoldoutGroup("DB"), SerializeField] private DevilDatabase devilDB;
        [FoldoutGroup("DB"), SerializeField] private EquipmentDatabase eqmDB;
        [FoldoutGroup("DB"), SerializeField] private StatDatabase statDB;
        [FoldoutGroup("DB"), SerializeField] private GrowthDatabase growthDB;
        [FoldoutGroup("DB"), SerializeField] private ExpDatabase expDB;
        [FoldoutGroup("DB"), SerializeField] private BackstoryDatabase storyDB;
        [FoldoutGroup("DB"), SerializeField] private AuraDatabase auraDB;

        private void Start()
        {
            AllDBLoaded = true;
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


        #region Data fetching

        private async UniTask<string[]> FetchDataAsync(string[] sheets, Action<string[]> finish = null)
        {
            var tasks = sheets.Select(sheet => FetchDataAsync(sheet)).ToList();
            var results = await UniTask.WhenAll(tasks);
            finish?.Invoke(results);
            return results;
        }

        private async UniTask<string> FetchDataAsync(string sheet, Action<string> finish = null)
        {
            string result = null;
            var request = UnityWebRequest.Get($"{apiUrl}{databaseId}/{sheet}");
            var operation = await request.SendWebRequest();
            if (operation.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(operation.error);
            }
            else
            {
                result = operation.downloadHandler.text;
            }

            EditorLog.Message(result);
            finish?.Invoke(result);
            return result;
        }

        #endregion

        
        #region Characters & Equipments

        public List<Hero> GetAllHeroes()
        {
            return heroDB.GetHeroes();
        }
        
        public float GetGrowth(object obj)
        {
            return growthDB.GetGrowth(obj);
        }

        public Hero GetHeroWithID(string id)
        {
            return heroDB.GetHeroWithID(id);
        }

        public Devil GetDevilWithID(string id)
        {
            return devilDB.GetDevilWithID(id);
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

        #region Aura
        
        public List<Aura> GetAuras(object obj)
        {
            return auraDB.GetAuras(obj);
        }
        
        #endregion
    }
    
    public static partial class DatabaseExtensions
    {
        public static float GetHeroGrowth(this Hero h)
        {
            return DataManager.Instance.GetGrowth(h.tier);
        }

        public static float GetDevilGrowth(this Devil d)
        {
            return DataManager.Instance.GetGrowth(d.tier);
        }

        public static string GetHeroAlias(this Hero h)
        {
            return DataManager.Instance.GetHeroAlias(h.id);
        }

        public static string GetHeroStory(this Hero h)
        {
            return DataManager.Instance.GetHeroStory(h.id);
        }

        public static float GetEquipmentGrowth(this Equipment e)
        {
            return DataManager.Instance.GetGrowth(e.rarity);
        }

        public static Hero GetHeroWithID(this DB.Player.Hero hsd)
        {
            return DataManager.Instance.GetHeroWithID(hsd.heroId);
        }

        public static int GetLevel(this DB.Player.Hero hsd)
        {
            return DataManager.Instance.GetLevel(hsd.totalExp);
        }

        public static Tuple<int, int> GetExp(this DB.Player.Hero hsd)
        {
            return DataManager.Instance.GetExp(hsd.totalExp);
        }
    }
}
