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

    private List<CharacterCard> cardList = new();
    private List<CharacterSaveData> chrSaveDataList = new();

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
            if (i >= chrSaveDataList.Count)
            {
                cardList[i].gameObject.SetActive(false);
                continue;
            }

            cardList[i].gameObject.SetActive(true);
            cardList[i].Init(chrSaveDataList[i]);
            cardList[i].OnShowCardDetail = ShowCardDetail;
        }
    }

    private void Refresh()
    {
        bool acpAllTier = tierOptList.Count < 1;
        bool acpAllElement = elementOptList.Count < 1;
        bool acpAllRace = raceOptList.Count < 1;
        
        //todo: refresh characters based on sort/filter
    }

    private void ShowCardDetail(CharacterSaveData saveData)
    {
        charDetail.gameObject.SetActive(true);
        charDetail.Init(saveData);
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

    private void SortByLevel(bool asc)
    {
        //todo: sort characters
        
        Refresh();
    }

    private void SortByTier(bool asc)
    {
        //todo: sort characters
        
        Refresh();
    }

    #region Buttons

    public void OnClickBack()
    {
        ValhallaUI.Hide();
    }

    #endregion
}
