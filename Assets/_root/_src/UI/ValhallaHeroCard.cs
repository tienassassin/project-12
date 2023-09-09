using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValhallaHeroCard : HeroCard
{
    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;
    [SerializeField] private GameObject lockedMark;

    private Action<EntitySaveData> _cardSelected;
    public bool IsLocked { get; private set; }

    public void Init(EntitySaveData saveData, Action<EntitySaveData> cardSelected)
    {
        base.Init(saveData);

        IsLocked = false;
        _cardSelected = cardSelected;
        Refresh();
    }

    public void Init(EntityData data)
    {
        EntityData = data;
        name = EntityData.name + " (locked)";
        IsLocked = true;
        _cardSelected = null;
        Refresh();
    }

    private void Refresh()
    {
        lockedMark.SetActive(IsLocked);
        imgElement.color = ColorPalette.Instance.GetElementColor(EntityData.info.element);
        txtLevel.text = IsLocked ? "1" : Level.ToString();
        sldHp.value = IsLocked ? 1 : (Hp / EntityData.info.stats.health);
        sldEnergy.value = IsLocked ? 1 : (Energy / 100);
    }

    public void SelectCard()
    {
        _cardSelected?.Invoke(SaveData);
    }
}