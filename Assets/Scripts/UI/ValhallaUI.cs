using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ValhallaUI : BaseUI
{
    [SerializeField] private Transform heroCardContainer;
    [SerializeField] private HeroCard heroCardPref;
    
    [SerializeField] private FilterOption[] tierFilterOptions;
    [SerializeField] private FilterOption[] elementFilterOptions;
    [SerializeField] private FilterOption[] raceFilterOptions;

    [SerializeField] private HeroDetail heroDetail;
    
    private List<Tier> tierOptList = new();
    private List<Element> elementOptList = new();
    private List<Race> raceOptList = new();
    private SortType lvSort;
    private SortType tierSort;

    private List<HeroCard> cardList = new();
    private List<HeroCard> activeCardList = new();
    private HeroCard selectedCard;
    private List<HeroSaveData> heroSaveDataList = new();
    
    public static void Show()
    {
        UIManager.Instance.ShowUI(nameof(ValhallaUI));
    }

    public static void Hide()
    {
        UIManager.Instance.HideUI(nameof(ValhallaUI));
    }

    protected override void Awake()
    {
        base.Awake();

        foreach (var opt in tierFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        foreach (var opt in elementFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        foreach (var opt in raceFilterOptions)
        {
            opt.SetEvent(AddOptionToFilter);
        }

        cardList = new List<HeroCard>();
        foreach (Transform child in heroCardContainer)
        {
            cardList.Add(child.gameObject.GetComponent<HeroCard>());
        }
        
        heroDetail.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        tierOptList.Clear();
        elementOptList.Clear();
        raceOptList.Clear();

        lvSort = SortType.Descending;
        tierSort = SortType.None;

        heroSaveDataList = UserManager.Instance.GetAllHeroes();
        
        LoadHeroCards();
        Refresh();
    }

    private void LoadHeroCards()
    {
        while (heroCardContainer.childCount < heroSaveDataList.Count)
        {
            var o = Instantiate(heroCardPref, heroCardContainer);
            cardList.Add(o);
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];
            if (i >= heroSaveDataList.Count)
            {
                card.gameObject.SetActive(false);
                card.name = Constants.EMPTY_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            card.Init(heroSaveDataList[i]);
            card.OnShowCardDetail = (saveData)=>
            {
                ShowCardDetail(saveData);
                selectedCard = card;
            };
        }
    }

    private void Refresh()
    {
        bool acpAllTier = tierOptList.Count < 1;
        bool acpAllElement = elementOptList.Count < 1;
        bool acpAllRace = raceOptList.Count < 1;

        if (lvSort != SortType.None)
        {
            cardList.Sort((c1, c2) =>
            
                lvSort == SortType.Ascending ? CompareLevel(c1, c2) : CompareLevel(c2,c1)
            );
        }
        else if (tierSort != SortType.None)
        {
            cardList.Sort((c1, c2) =>
            
                tierSort == SortType.Ascending ? CompareTier(c1, c2) : CompareTier(c2,c1)
            );
        }
        
        activeCardList.Clear();
        
        cardList.ForEach(c =>
        {
            c.transform.SetAsLastSibling();
            if (c.name == Constants.EMPTY_MARK) return;
            
            bool match = (tierOptList.Contains(c.Tier) || acpAllTier)
                && (elementOptList.Contains(c.Element) || acpAllElement)
                && (raceOptList.Contains(c.Race) || acpAllRace);

            c.gameObject.SetActive(match);

            if (match) activeCardList.Add(c);
        });

        int CompareLevel(HeroCard c1, HeroCard c2, int comparision = 0)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            if (c1.Level > c2.Level) return 1;
            if (c1.Level < c2.Level) return -1;
            return (comparision >= 1) ? 0 : CompareTier(c1, c2, comparision + 1);
        }
        
        int CompareTier(HeroCard c1, HeroCard c2, int comparision = 0)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            if ((int)c1.Tier > (int)c2.Tier) return 1;
            if ((int)c1.Tier < (int)c2.Tier) return -1;
            return (comparision >= 1) ? 0 : CompareLevel(c1, c2, comparision + 1);
        }
    }

    private void ShowCardDetail(HeroSaveData saveData)
    {
        heroDetail.gameObject.SetActive(true);
        heroDetail.Init(saveData);
    }

    public void HideCardDetail()
    {
        heroDetail.gameObject.SetActive(false);
        selectedCard = null;
    }

    public void SelectNextCard()
    {
        if (!selectedCard || activeCardList.Count < 2) return;
        
        int nextIndex = activeCardList.IndexOf(selectedCard) + 1;
        if (nextIndex >= activeCardList.Count) nextIndex = 0;
        activeCardList[nextIndex].OnClickCard();
    }

    public void SelectPreviousCard()
    {
        if (!selectedCard || activeCardList.Count < 2) return;
        
        int nextIndex = activeCardList.IndexOf(selectedCard) - 1;
        if (nextIndex < 0) nextIndex = activeCardList.Count - 1;
        activeCardList[nextIndex].OnClickCard();
    }

    private void AddOptionToFilter(object o)
    {
        switch (o)
        {
            case Tier t when tierOptList.Contains(t):
                tierOptList.Remove(t);
                break;
            case Tier t:
                tierOptList.Add(t);
                break;
            case Element e when elementOptList.Contains(e):
                elementOptList.Remove(e);
                break;
            case Element e:
                elementOptList.Add(e);
                break;
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
        
        Refresh();
    }

    public void SortByLevel(bool asc)
    {
        lvSort = (asc ? SortType.Ascending : SortType.Descending);
        tierSort = SortType.None;
        Refresh();
    }

    public void SortByTier(bool asc)
    {
        lvSort = SortType.None;
        tierSort = (asc ? SortType.Ascending : SortType.Descending);
        Refresh();
    }

    #region Buttons

    public void OnClickBack()
    {
        ValhallaUI.Hide();
    }

    #endregion
}
