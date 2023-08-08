using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpDetail : DuztineBehaviour
{
    [SerializeField] private GameObject info;
    [SerializeField] private GameObject equipmentGroup;
    
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private Image raceImg;
    [SerializeField] private Image elementImg;

    [SerializeField] private Transform heroCardContainer;
    [SerializeField] private LineUpHeroCard heroCardPref;
    [SerializeField] private Transform eqmCardContainer;
    [SerializeField] private EquipmentCard eqmCardPref;

    [SerializeField] private FilterOption[] raceFilterOptions;

    private int curSlotId;
    private HeroSaveData saveData;
    private BaseHero baseHero;
    private List<HeroSaveData> heroSaveDataList = new();
    
    private List<LineUpHeroCard> heroCardList = new();
    private List<EquipmentCard> eqmCardList = new();

    private List<Race> raceOptList = new();

    private void Awake()
    {
        foreach (var opt in raceFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }
        
        heroCardList = new List<LineUpHeroCard>();
        foreach (Transform child in heroCardContainer)
        {
            heroCardList.Add(child.gameObject.GetComponent<LineUpHeroCard>());
        }
    }

    private void OnEnable()
    {
        raceOptList.Clear();
        
        heroSaveDataList = UserManager.Instance.GetAllHeroes();
        
        LoadHeroCards();
    }

    public void Init(int slotId, HeroSaveData data)
    {
        curSlotId = slotId;
        saveData = data;
        baseHero = saveData?.GetHeroWithID();
        info.SetActive(saveData != null);
        equipmentGroup.SetActive(saveData != null);
        
        Refresh();
    }

    #region Hero Cards

    private void LoadHeroCards()
    {
        while (heroCardContainer.childCount < heroSaveDataList.Count)
        {
            var o = Instantiate(heroCardPref, heroCardContainer);
            heroCardList.Add(o);
        }
        
        for (int i = 0; i < heroCardList.Count; i++)
        {
            var card = heroCardList[i];
            if (i >= heroSaveDataList.Count)
            {
                card.gameObject.SetActive(false);
                card.name = Constants.EMPTY_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            card.Init(heroSaveDataList[i], (data)=>
                {
                    if (data != saveData)
                    {
                        AddHeroToLineUp(data.heroId);
                        Init(curSlotId, data);
                    }
                    else
                    {
                        RemoveHeroFromLineUp();
                        Init(curSlotId, null);
                    }
                    
                    UpdateHeroCards();
                });
        }
        
        UpdateHeroCards();
        SortHeroCards();
    }

    private void UpdateHeroCards()
    {
        heroCardList.ForEach(x =>
        {
            x.UpdateReadyState();
        });
    }

    private void ApplyHeroCardFilter()
    {
        bool acpAllRace = raceOptList.Count < 1;
        
        heroCardList.ForEach(c =>
        {
            if (c.name == Constants.EMPTY_MARK) return;

            bool match = (raceOptList.Contains(c.Race) || acpAllRace);
            c.gameObject.SetActive(match);
        });
    }
    
    private void SortHeroCards()
    {
        heroCardList.Sort((c1, c2) => CompareLevel(c1, c2, false));
        
        heroCardList.ForEach(c =>
        {
            c.transform.SetAsLastSibling();
        });
        
        int CompareLevel(LineUpHeroCard c1, LineUpHeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            int levelComparision = c1.Level.CompareTo(c2.Level);
            if (levelComparision != 0) return ascending ? levelComparision : -levelComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            return ascending ? tierComparision : -tierComparision;
        }
    }
    
    #endregion

    
    #region Equipment Cards

    // todo    

    #endregion

    private void Refresh()
    {
        if (saveData == null) return;

        nameTxt.text = baseHero.name;
    }

    private void AddHeroToLineUp(string heroId)
    {
        UserManager.Instance.AddHeroToLineUp(curSlotId, heroId);
    }

    private void RemoveHeroFromLineUp()
    {
        UserManager.Instance.RemoveHeroFromLineUp(curSlotId);
    }
    
    private void AddOptionToFilter(object o)
    {
        switch (o)
        {
            case Race r when raceOptList.Contains(r):
                raceOptList.Remove(r);
                break;
            case Race r:
                raceOptList.Add(r);
                break;
            default:
                EditorLog.Error($"Object {o} is not a valid filter option");
                return;
        }
        
       ApplyHeroCardFilter();
    }
}