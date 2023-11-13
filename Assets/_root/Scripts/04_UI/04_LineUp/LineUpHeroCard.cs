using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpHeroCard : TavernCell
{
    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;
    [SerializeField] private GameObject readyMark;

    private Action<MyEntity> _cardSelected;

    public void Init(MyEntity data, Action<MyEntity> cardSelected)
    {
        // base.Init(data);
        //
        // _cardSelected = cardSelected;
        // Refresh();
    }

    private void Refresh()
    {
        // imgElement.color = ColorPalette.Instance.GetElementColor(EntityData.info.role);
        // txtLevel.text = Level.ToString();
        // sldHp.value = Hp / EntityData.info.stats.health;
        // sldEnergy.value = Energy / 100;
    }

    public void UpdateReadyState()
    {
        // if (name == Constants.EMPTY_MARK) return;
        // bool ready = PlayerManager.Instance.IsHeroReady(SaveData.entityId);
        // readyMark.SetActive(ready);
    }

    public void OnClickCard()
    {
        // _cardSelected?.Invoke(SaveData);
    }
}