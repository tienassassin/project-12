using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpSlot : DuztineBehaviour
{
    [SerializeField] private int slotId;
    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject heroInfo;

    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider energySlider;

    [SerializeField] private GameObject highlight;

    private Action<int, HeroSaveData> onShowSlotDetail = null;
    private HeroSaveData saveData;
    private BaseHero baseHero;

    private void OnEnable()
    {
        this.AddListener(EventID.ON_HIGHLIGHT_AURA, SwitchHighlight);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_HIGHLIGHT_AURA, SwitchHighlight);
    }

    public void Init(HeroSaveData data, Action<int, HeroSaveData> onShow)
    {
        saveData = data;
        baseHero = saveData?.GetHeroWithID();
        hero.SetActive(saveData != null);
        heroInfo.SetActive(saveData != null);
        name = (baseHero != null ? baseHero.name : Constants.EMPTY_MARK);

        onShowSlotDetail = onShow;
        Refresh();
    }

    private void Refresh()
    {
        if (saveData == null) return;
        
        levelTxt.text = saveData.GetLevel().ToString();
        hpSlider.value = saveData.curHp / baseHero.stats.health;
        energySlider.value = saveData.energy / 100;
    }

    private void SwitchHighlight(object condition)
    {
        bool active = false;
        
        if (baseHero != null)
        {
            switch (condition)
            {
                case Race r:
                    active = (baseHero.race == r);
                    break;
                case Element e:
                    active = (baseHero.element == e);
                    break;
            }
        }

        highlight.SetActive(active);
    }
    
    public void OnClickSlot()
    {
        onShowSlotDetail?.Invoke(slotId, saveData);
    }
}