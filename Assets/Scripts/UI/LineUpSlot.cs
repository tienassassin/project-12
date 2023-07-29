using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpSlot : DuztineBehaviour
{
    public Action<int, HeroSaveData> OnShowSlotDetail = null;

    [SerializeField] private int slotId;
    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject heroInfo;

    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider curHp;
    [SerializeField] private Slider energy;

    private HeroSaveData saveData;
    private BaseHero baseHero;

    public void Init(HeroSaveData data)
    {
        saveData = data;
        baseHero = saveData?.GetHeroWithID();
        hero.SetActive(saveData != null);
        heroInfo.SetActive(saveData != null);
        name = (baseHero != null ? baseHero.name : "(empty)");
        
        Refresh();
    }

    private void Refresh()
    {
        if (saveData == null) return;
        
        levelTxt.text = saveData.GetLevel().ToString();
        curHp.value = saveData.curHp / baseHero.stats.health;
        energy.value = saveData.energy / 100;
    }
    
    public void OnClickSlot()
    {
        OnShowSlotDetail?.Invoke(slotId, saveData);
    }
}