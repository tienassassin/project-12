using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

namespace System.DB
{
    public class Database : Singleton<Database>
    {
        public bool AllDBLoaded { get; private set; }

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
        private const string EQUIPMENT_SHEET = "Equipments";
        private const string STATS_SHEET = "Stats";
        private const string ENTITY_GROWTH_SHEET = "EntityGrowth";
        private const string EQUIPMENT_GROWTH_SHEET = "EquipmentGrowth";
        private const string EXP_SHEET = "Exp";
        private const string BACKSTORY_SHEET = "Backstory";
        private const string RACE_AURA_SHEET = "RaceAura";
        private const string ELEMENT_AURA_SHEET = "ElementAura";

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
        public async void FetchData()
        {
            bool hasInternet = await TestConnection();
            if (!hasInternet)
            {
                AllDBLoaded = true;
                return;
            }
            
            await FetchDataAsync(
                new[] {
                    HERO_SHEET,
                    DEVIL_SHEET,
                    EQUIPMENT_SHEET,
                    STATS_SHEET,
                    ENTITY_GROWTH_SHEET,
                    EQUIPMENT_GROWTH_SHEET,
                    EXP_SHEET,
                    BACKSTORY_SHEET,
                    RACE_AURA_SHEET,
                    ELEMENT_AURA_SHEET,
                }
            , (results) =>
            {
                heroDB.Import(results[0]);
                devilDB.Import(results[1]);
                eqmDB.Import(results[2]);
                statsDesc.Import(results[3]);
                growthDB.Import(results[4], results[5]);
                expDB.Import(results[6]);
                bsDB.Import(results[7]);
                auraDB.Import(results[8], results[9]);
                
                AllDBLoaded = true;
            });
        }

        
        #region Data fetching

        private async UniTask<bool> TestConnection()
        {
            var request = UnityWebRequest.Get("https://www.google.com/");
            var operation = await request.SendWebRequest();
            return operation.result == UnityWebRequest.Result.Success;
        }
        
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
