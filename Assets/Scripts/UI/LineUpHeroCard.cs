using System;
using DB.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpHeroCard : Hero
{
    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;
    [SerializeField] private GameObject readyMark;
    
    private Action<DB.Player.Hero> _cardSelected;

    public void Init(DB.Player.Hero data, Action<DB.Player.Hero> cardSelected)
    {
        base.Init(data);

        _cardSelected = cardSelected;
        Refresh();
    }
    
    private void Refresh()
    {
        imgElement.color = ColorPalette.Instance.GetElementColor(BaseData.element);
        txtLevel.text = Level.ToString();
        sldHp.value = Hp / BaseData.stats.health;
        sldEnergy.value = Energy / 100;
    }

    public void UpdateReadyState()
    {
        if (name == Constants.EMPTY_MARK) return;
        bool ready = PlayerManager.Instance.IsHeroReady(SaveData.heroId);
        readyMark.SetActive(ready);
    }

    public void OnClickCard()
    {
        _cardSelected?.Invoke(SaveData);
    }
}