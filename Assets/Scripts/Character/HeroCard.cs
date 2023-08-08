using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : Hero
{
    public bool IsLocked => isLocked;
    
    [SerializeField] private Image elementImg;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private GameObject lockedMark;

    private Action<HeroSaveData> onShowCardDetail = null;
    private bool isLocked;

    public void Init(HeroSaveData data, Action<HeroSaveData> onShow)
    {
        base.Init(data);

        isLocked = false;
        onShowCardDetail = onShow;
        Refresh();
    }

    public void Init(BaseHero data)
    {
        baseHero = data;
        name = baseHero.name + " (locked)";
        isLocked = true;
        onShowCardDetail = null;
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
        onShowCardDetail?.Invoke(saveData);
    }
}