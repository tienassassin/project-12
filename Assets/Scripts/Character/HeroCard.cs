using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : Hero
{
    public bool IsLocked => isLocked;
    public Action<HeroSaveData> OnShowCardDetail = null;
    
    [SerializeField] private Image elementImg;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private GameObject lockedMark;

    private bool isLocked;

    public void Init(HeroSaveData data)
    {
        base.Init(data);

        isLocked = false;
        Refresh();
    }

    public void Init(BaseHero data)
    {
        baseHero = data;
        name = baseHero.name + " (locked)";
        isLocked = true;
        Refresh();
    }

    private void Refresh()
    {
        lockedMark.SetActive(isLocked);
        elementImg.color = ColorPalette.Instance.GetElementColor(baseHero.element);
        levelTxt.text = isLocked ? "1" : level.ToString();
        hpSlider.value = isLocked ? 1 : (curHP / baseHero.stats.health);
        energySlider.value = isLocked ? 1:  (energy / 100);
    }

    public void OnClickCard()
    {
        OnShowCardDetail?.Invoke(saveData);
    }
}