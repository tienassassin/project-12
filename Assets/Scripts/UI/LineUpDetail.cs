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

    private int curSlotId;
    private HeroSaveData saveData;
    private BaseHero baseHero;
    private List<LineUpHeroCard> heroCardList = new();
    private List<EquipmentCard> eqmCardList = new();

    private List<HeroSaveData> heroSaveDataList = new();

    private void Awake()
    {
        heroCardList = new List<LineUpHeroCard>();
        foreach (Transform child in heroCardContainer)
        {
            heroCardList.Add(child.gameObject.GetComponent<LineUpHeroCard>());
        }
    }

    private void OnEnable()
    {
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
            card.Init(heroSaveDataList[i]);
            card.OnShowCardDetail = (data)=>
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
                
                RefreshHeroCards();
            };
        }
        
        RefreshHeroCards();
    }

    private void RefreshHeroCards()
    {
        heroCardList.ForEach(x =>
        {
            x.UpdateReadyState();
        });
    }

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
}