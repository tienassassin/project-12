using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : Hero
{
    public Action<HeroSaveData> OnShowCardDetail = null;
    
    [SerializeField] private Image elementImg;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Image hpImg;
    [SerializeField] private Image energyImg;

    [SerializeField] private Color[] elementColors;

    public override void Init(HeroSaveData data)
    {
        base.Init(data);
        
        Refresh();
    }

    private void Refresh()
    {
        elementImg.color = elementColors[(int)baseHero.element];
        levelTxt.text = level.ToString();
        hpImg.fillAmount = curHP / baseHero.stats.health;
        energyImg.fillAmount = energy / 100;
    }

    public void OnClickCard()
    {
        OnShowCardDetail?.Invoke(saveData);
    }
}