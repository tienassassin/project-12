using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DB.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private bool isDummy;
        [SerializeField] private HeroDatabase heroDB;
        
        private const string HERO_DB_KEY = "HERO_DB";

        protected override void Awake()
        {
            base.Awake();
            
            Application.targetFrameRate = 144;
            
            if (!isDummy) LoadHeroDB();
            else GenerateDummyData();
        }

        public List<Hero> GetAllHeroes()
        {
            return heroDB.allHeroes;
        }

        public List<Hero> GetReadyHeroes()
        {
            var readyHeroList = new List<Hero>();
            heroDB.readyHeroes.ForEach(x =>
            {
                readyHeroList.Add(heroDB.allHeroes.Find(y => y.heroId == x));
            });

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

        public bool IsHeroUnlocked(string heroId, out Hero hsd)
        {
            hsd = heroDB.allHeroes.Find(h => h.heroId == heroId);
            return hsd != null;
        }

        public void LoadHeroDB()
        {
            if (PlayerPrefs.HasKey(HERO_DB_KEY))
            {
                heroDB = JsonUtility.FromJson<HeroDatabase>(PlayerPrefs.GetString(HERO_DB_KEY));
            }
            else
            {
                heroDB = new HeroDatabase();
                SaveCharacterDB();
            }
        }

        private void GenerateDummyData()
        {
            
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
    public class HeroDatabase
    {
        [TableList(ShowIndexLabels = true)]
        public List<Hero> allHeroes = new();
        
        public List<string> readyHeroes = new();
    }

    [Serializable]
    public class Hero
    {
        [VerticalGroup("Information")]
        public string heroId;
        
        [VerticalGroup("Information")]
        public int totalExp;
        
        [VerticalGroup("Information")]
        public float curHp;
        
        [VerticalGroup("Information")]
        public float energy;
        
        public List<Equipment> eqmList;
    }

    [Serializable]
    public class Equipment
    {
        public string eqmId;
        public int level;
    }
}
