using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCard : Character
{
    public Action<CharacterSaveData> OnShowCardDetail = null;
    
    [SerializeField] private Image elementImg;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Image hpImg;
    [SerializeField] private Image energyImg;

    [SerializeField] private Color[] elementColors;

    public override void Init(CharacterSaveData data)
    {
        base.Init(data);
        
        Refresh();
    }

    private void Refresh()
    {
        elementImg.color = elementColors[(int)baseChr.element];
        levelTxt.text = level.ToString();
        hpImg.fillAmount = curHP / baseChr.stats.health;
        energyImg.fillAmount = energy / 100;
    }

    public void OnClickCard()
    {
        OnShowCardDetail?.Invoke(saveData);
    }
}