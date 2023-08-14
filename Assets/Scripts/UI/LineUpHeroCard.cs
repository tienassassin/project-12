using System;
using Player.DB;
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
    
    private Action<Player.DB.Hero> _cardSelected;

    public void Init(Player.DB.Hero data, Action<Player.DB.Hero> cardSelected)
    {
        base.Init(data);

        _cardSelected = cardSelected;
        Refresh();
    }
    
    private void Refresh()
    {
        imgElement.color = ColorPalette.Instance.GetElementColor(BaseData.Element);
        txtLevel.text = Level.ToString();
        sldHp.value = Hp / BaseData.Stats.health;
        sldEnergy.value = Energy / 100;
    }

    public void UpdateReadyState()
    {
        if (name == Constants.EMPTY_MARK) return;
        bool ready = UserManager.Instance.IsHeroReady(SaveData.heroId);
        readyMark.SetActive(ready);
    }

    public void OnClickCard()
    {
        _cardSelected?.Invoke(SaveData);
    }
}