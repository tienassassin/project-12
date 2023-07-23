using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ValhallaUI : BaseUI
{
    [SerializeField] private Transform chrCardContainer;
    [SerializeField] private CharacterCard chrCardPref;
    
    [SerializeField] private FilterOption[] tierFilterOptions;
    [SerializeField] private FilterOption[] elementFilterOptions;
    [SerializeField] private FilterOption[] raceFilterOptions;

    [SerializeField] private CharacterDetail charDetail;
    
    private List<Tier> tierOptList = new();
    private List<Element> elementOptList = new();
    private List<Race> raceOptList = new();
    private SortType lvSort;
    private SortType tierSort;

    private List<CharacterCard> cardList = new();
    private List<CharacterCard> activeCardList = new();
    private CharacterCard selectedCard;
    private List<CharacterSaveData> chrSaveDataList = new();

    private const string EMPTY_CARD_MARK = "(empty)";

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

        cardList = new List<CharacterCard>();
        foreach (Transform child in chrCardContainer)
        {
            cardList.Add(child.gameObject.GetComponent<CharacterCard>());
        }
        
        charDetail.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        tierOptList.Clear();
        elementOptList.Clear();
        raceOptList.Clear();

        lvSort = SortType.Descending;
        tierSort = SortType.None;

        chrSaveDataList = UserManager.Instance.chrSaveDataList;
        
        LoadCharacterCards();
        Refresh();
    }

    private void LoadCharacterCards()
    {
        while (chrCardContainer.childCount < chrSaveDataList.Count)
        {
            var o = Instantiate(chrCardPref, chrCardContainer);
            cardList.Add(o);
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            var card = cardList[i];
            if (i >= chrSaveDataList.Count)
            {
                card.gameObject.SetActive(false);
                card.name = EMPTY_CARD_MARK;
                continue;
            }

            card.gameObject.SetActive(true);
            card.Init(chrSaveDataList[i]);
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
            if (c.name == EMPTY_CARD_MARK) return;
            
            bool match = (tierOptList.Contains(c.Tier) || acpAllTier)
                && (elementOptList.Contains(c.Element) || acpAllElement)
                && (raceOptList.Contains(c.Race) || acpAllRace);

            c.gameObject.SetActive(match);

            if (match) activeCardList.Add(c);
        });

        int CompareLevel(CharacterCard c1, CharacterCard c2)
        {
            if (c1.name == EMPTY_CARD_MARK) return 1;
            if (c2.name == EMPTY_CARD_MARK) return -1;

            if (c1.Level > c2.Level) return 1;
            if (c1.Level < c2.Level) return -1;
            return CompareTier(c1, c2);
        }
        
        int CompareTier(CharacterCard c1, CharacterCard c2)
        {
            if (c1.name == EMPTY_CARD_MARK) return 1;
            if (c2.name == EMPTY_CARD_MARK) return -1;

            if ((int)c1.Tier > (int)c2.Tier) return 1;
            if ((int)c1.Tier < (int)c2.Tier) return -1;
            return CompareLevel(c1, c2);
        }
    }

    private void ShowCardDetail(CharacterSaveData saveData)
    {
        charDetail.gameObject.SetActive(true);
        charDetail.Init(saveData);
    }

    public void HideCardDetail()
    {
        charDetail.gameObject.SetActive(false);
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
