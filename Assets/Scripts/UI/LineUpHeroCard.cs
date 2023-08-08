using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpHeroCard : Hero
{
    [SerializeField] private Image elementImg;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private GameObject readyMark;
    
    private Action<HeroSaveData> onPickCard = null;

    public void Init(HeroSaveData data, Action<HeroSaveData> onPick)
    {
        base.Init(data);

        onPickCard = onPick;
        Refresh();
    }
    
    private void Refresh()
    {
        elementImg.color = ColorPalette.Instance.GetElementColor(baseHero.element);
        levelTxt.text = level.ToString();
        hpSlider.value = curHP / baseHero.stats.health;
        energySlider.value = energy / 100;
    }

    public void UpdateReadyState()
    {
        if (name == Constants.EMPTY_MARK) return;
        bool ready = UserManager.Instance.IsHeroReady(saveData.heroId);
        readyMark.SetActive(ready);
    }

    public void OnClickCard()
    {
        onPickCard?.Invoke(saveData);
    }
}