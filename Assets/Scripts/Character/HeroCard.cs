using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : Hero
{
    public Action<HeroSaveData> OnShowCardDetail = null;
    
    [SerializeField] private Image elementImg;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider hpImg;
    [SerializeField] private Slider energyImg;

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
        hpImg.value = curHP / baseHero.stats.health;
        energyImg.value = energy / 100;
    }

    public void OnClickCard()
    {
        OnShowCardDetail?.Invoke(saveData);
    }
}