using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<BaseHero> heroList = new();
    
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

        heroList = Database.Instance.GetAllBaseHeroes();
        
        LoadHeroCards();
        Refresh();
    }

    private void LoadHeroCards()
    {
        while (heroCardContainer.childCount < heroList.Count)
        {
            var o = Instantiate(heroCardPref, heroCardContainer);
            cardList.Add(o);
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];
            if (i >= heroList.Count)
            {
                card.gameObject.SetActive(false);
                card.name = Constants.EMPTY_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            if (UserManager.Instance.IsHeroUnlocked(heroList[i].id, out var hsd))
            {
                // unlocked hero
                card.Init(hsd);
                card.OnShowCardDetail = (saveData) =>
                {
                    ShowCardDetail(saveData);
                    selectedCard = card;
                };
            }
            else
            {
                // locked hero
                card.Init(heroList[i]);
                card.OnShowCardDetail = null;
            }
            
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
            
                CompareLevel(c1, c2, lvSort != SortType.Descending)
            );
        }
        else if (tierSort != SortType.None)
        {
            cardList.Sort((c1, c2) =>

                CompareTier(c1, c2, tierSort != SortType.Descending)
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

        int CompareLevel(HeroCard c1, HeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;
            
            int lockComparision = c1.IsLocked.CompareTo(c2.IsLocked);
            if (lockComparision != 0) return lockComparision;
            int levelComparision = c1.Level.CompareTo(c2.Level);
            if (levelComparision != 0) return ascending ? levelComparision : -levelComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            return ascending ? tierComparision : -tierComparision;
        }
        
        int CompareTier(HeroCard c1, HeroCard c2, bool ascending)
        {
            if (c1.name == Constants.EMPTY_MARK) return 1;
            if (c2.name == Constants.EMPTY_MARK) return -1;

            int lockComparision = c1.IsLocked.CompareTo(c2.IsLocked);
            if (lockComparision != 0) return lockComparision;
            int tierComparision = c1.Tier.CompareTo(c2.Tier);
            if (tierComparision != 0) return ascending ? tierComparision : -tierComparision;
            int levelComparision = c1.Level.CompareTo(c2.Level);
            return ascending ? levelComparision : -levelComparision;
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
