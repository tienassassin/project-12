using System;
using DB.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCard : Hero
{
    public bool IsLocked { get; private set; }

    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;
    [SerializeField] private GameObject lockedMark;

    private Action<DB.Player.Hero> _cardSelected;

    public void Init(DB.Player.Hero saveData, Action<DB.Player.Hero> cardSelected)
    {
        base.Init(saveData);

        IsLocked = false;
        _cardSelected = cardSelected;
        Refresh();
    }

    public void Init(DB.System.Hero baseData)
    {
        BaseData = baseData;
        name = BaseData.Name + " (locked)";
        IsLocked = true;
        _cardSelected = null;
        Refresh();
    }

    private void Refresh()
    {
        lockedMark.SetActive(IsLocked);
        imgElement.color = ColorPalette.Instance.GetElementColor(BaseData.Element);
        txtLevel.text = IsLocked ? "1" : Level.ToString();
        sldHp.value = IsLocked ? 1 : (Hp / BaseData.Stats.health);
        sldEnergy.value = IsLocked ? 1:  (Energy / 100);
    }

    public void SelectCard()
    {
        _cardSelected?.Invoke(SaveData);
    }
}