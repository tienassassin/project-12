using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace System.DB
{
    public class Database : Singleton<Database>
    {
        public bool IsAllDBLoaded => _dbLoaded >= DB_COUNT;
        
        [HorizontalGroup("ApiGithub"), SerializeField, LabelWidth(125)] private string apiUrl = "https://opensheet.elk.sh/";
        [HorizontalGroup("GoogleSheet"), SerializeField, LabelWidth(125)] private string databaseId = "18y2sbmIKSfbg055IocVDvkR7oZsrPbBnE1kZcmChXIY";
        
        [FoldoutGroup("DB"), SerializeField] private HeroDatabase heroDB;
        [FoldoutGroup("DB"), SerializeField] private DevilDatabase devilDB;
        [FoldoutGroup("DB"), SerializeField] private EquipmentDatabase eqmDB;
        [FoldoutGroup("DB"), SerializeField] private StatsDescriptions statsDesc;
        [FoldoutGroup("DB"), SerializeField] private GrowthDatabase growthDB;
        [FoldoutGroup("DB"), SerializeField] private ExpDatabase expDB;
        [FoldoutGroup("DB"), SerializeField] private BackstoryDatabase bsDB;
        [FoldoutGroup("DB"), SerializeField] private AuraDatabase auraDB;

        private const string HERO_SHEET = "Heroes";
        private const string DEVIL_SHEET = "Devils";
        private const string EQM_SHEET = "Equipments";
        private const string STATS_SHEET = "Stats";
        private const string ENTITY_GROWTH_SHEET = "EntityGrowth";
        private const string EQM_GROWTH_SHEET = "EquipmentGrowth";
        private const string EXP_SHEET = "Exp";
        private const string BACKSTORY_SHEET = "Backstory";
        private const string RACE_AURA_SHEET = "RaceAura";
        private const string ELEMENT_AURA_SHEET = "ElementAura";

        private int _dbLoaded = 0;
        private const int DB_COUNT = 10;

        protected override void Awake()
        {
            base.Awake();
            FetchData();
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

        [Button]
        public void FetchData()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) return;
            
            StartCoroutine(FetchHeroDB());
            StartCoroutine(FetchDevilDB());
            StartCoroutine(FetchEquipmentDB());
            StartCoroutine(FetchStatsDescriptions());
            StartCoroutine(FetchGrowthDB());
            StartCoroutine(FetchExpDB());
            StartCoroutine(FetchBackstoryDB());
            StartCoroutine(FetchAuraDB());
        }

        #region Data fetching
        
        IEnumerator FetchHeroDB()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{HERO_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                heroDB.Import(uwr.downloadHandler.text);
            }

            _dbLoaded++;
        }
        
        IEnumerator FetchDevilDB()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{DEVIL_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                devilDB.Import(uwr.downloadHandler.text);
            }
            
            _dbLoaded++;
        }
        
        IEnumerator FetchEquipmentDB()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EQM_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                eqmDB.Import(uwr.downloadHandler.text);
            }
            
            _dbLoaded++;
        }

        IEnumerator FetchStatsDescriptions()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{STATS_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                statsDesc.Import(uwr.downloadHandler.text);
            }
            
            _dbLoaded++;
        }

        IEnumerator FetchGrowthDB()
        {
            var cUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{ENTITY_GROWTH_SHEET}");
            var eUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EQM_GROWTH_SHEET}");
            var dataC = "";
            yield return cUwr.SendWebRequest();
            if (cUwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(cUwr.error);
            }
            else
            {
                dataC = cUwr.downloadHandler.text;
            }
            
            _dbLoaded++;
            
            var dataE = "";
            yield return eUwr.SendWebRequest();
            if (eUwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(eUwr.error);
            }
            else
            {
                dataE = eUwr.downloadHandler.text;
            }
            
            _dbLoaded++;

            growthDB.Import(dataC, dataE);
        }
        
        IEnumerator FetchExpDB()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{EXP_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                expDB.Import(uwr.downloadHandler.text);
            }
            
            _dbLoaded++;
        }
        
        IEnumerator FetchBackstoryDB()
        {
            var uwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{BACKSTORY_SHEET}");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(uwr.error);
            }
            else
            {
                bsDB.Import(uwr.downloadHandler.text);
            }
            
            _dbLoaded++;
        }
        
        IEnumerator FetchAuraDB()
        {
            var rUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{RACE_AURA_SHEET}");
            var eUwr = UnityWebRequest.Get($"{apiUrl}{databaseId}/{ELEMENT_AURA_SHEET}");
            var dataR = "";
            yield return rUwr.SendWebRequest();
            if (rUwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(rUwr.error);
            }
            else
            {
                dataR = rUwr.downloadHandler.text;
            }
            
            _dbLoaded++;
            
            var dataE = "";
            yield return eUwr.SendWebRequest();
            if (eUwr.result != UnityWebRequest.Result.Success)
            {
                EditorLog.Error(eUwr.error);
            }
            else
            {
                dataE = eUwr.downloadHandler.text;
            }
            
            _dbLoaded++;

            auraDB.Import(dataR, dataE);
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
            return expDB.LevelMax;
        }
        
        #endregion

        #region Stats
        
        public string GetStatDescription(string key)
        {
            return statsDesc.GetStatDescription(key).Description;
        }
        
        public string GetStatName(string key)
        {
            return statsDesc.GetStatDescription(key).Name;
        }
        
        public float GetStatLimit(string key)
        {
            return statsDesc.GetStatDescription(key).Limit;
        }
        
        #endregion

        #region Backstory
        
        public string GetHeroAlias(string heroId)
        {
            return bsDB.GetBackstory(heroId).Alias;
        }
        
        public string GetHeroStory(string heroId)
        {
            return bsDB.GetBackstory(heroId).Story;
        }
        
        #endregion

        #region Aura
        
        public List<Aura> GetAuras(object obj)
        {
            return auraDB.GetAuras(obj);
        }
        
        #endregion
    }
    
    public static class DatabaseExtensions
    {
        public static float GetHeroGrowth(this Hero h)
        {
            return Database.Instance.GetGrowth(h.Tier);
        }

        public static float GetDevilGrowth(this Devil d)
        {
            return Database.Instance.GetGrowth(d.Tier);
        }

        public static string GetHeroAlias(this Hero h)
        {
            return Database.Instance.GetHeroAlias(h.Id);
        }

        public static string GetHeroStory(this Hero h)
        {
            return Database.Instance.GetHeroStory(h.Id);
        }

        public static float GetEquipmentGrowth(this Equipment e)
        {
            return Database.Instance.GetGrowth(e.Rarity);
        }

        public static Hero GetHeroWithID(this Player.DB.Hero hsd)
        {
            return Database.Instance.GetHeroWithID(hsd.heroId);
        }

        public static int GetLevel(this Player.DB.Hero hsd)
        {
            return Database.Instance.GetLevel(hsd.totalExp);
        }

        public static Tuple<int, int> GetExp(this Player.DB.Hero hsd)
        {
            return Database.Instance.GetExp(hsd.totalExp);
        }
    }
}
