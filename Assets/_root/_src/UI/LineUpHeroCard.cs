using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpHeroCard : HeroCard
{
    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;
    [SerializeField] private GameObject readyMark;

    private Action<EntitySaveData> _cardSelected;

    public void Init(EntitySaveData data, Action<EntitySaveData> cardSelected)
    {
        base.Init(data);

        _cardSelected = cardSelected;
        Refresh();
    }

    private void Refresh()
    {
        imgElement.color = ColorPalette.Instance.GetElementColor(Info.element);
        txtLevel.text = Level.ToString();
        sldHp.value = Hp / Info.stats.health;
        sldEnergy.value = Energy / 100;
    }

    public void UpdateReadyState()
    {
        if (name == Constants.EMPTY_MARK) return;
        bool ready = PlayerManager.Instance.IsHeroReady(SaveData.entityId);
        readyMark.SetActive(ready);
    }

    public void OnClickCard()
    {
        _cardSelected?.Invoke(SaveData);
    }
}