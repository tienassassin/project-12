using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineUpSlot : DuztineBehaviour
{
    [SerializeField] private int slotId;
    [SerializeField] private GameObject hero;
    [SerializeField] private GameObject heroInfo;

    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private Slider sldHp;
    [SerializeField] private Slider sldEnergy;

    [SerializeField] private GameObject highlight;
    private EntityData _entityData;
    private EntitySaveData _saveData;

    private Action<int, EntitySaveData> _slotHighlighted;

    private void OnEnable()
    {
        this.AddListener(EventID.ON_AURA_HIGHLIGHTED, SwitchHighlight);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_AURA_HIGHLIGHTED, SwitchHighlight);
    }

    public void Init(EntitySaveData data, Action<int, EntitySaveData> slotHighlighted)
    {
        _saveData = data;
        _entityData = _saveData?.GetEntity();
        hero.SetActive(_saveData != null);
        heroInfo.SetActive(_saveData != null);
        name = (_entityData != null ? _entityData.name : Constants.EMPTY_MARK);

        _slotHighlighted = slotHighlighted;
        Refresh();
    }

    private void Refresh()
    {
        if (_saveData == null) return;

        txtLevel.text = _saveData.GetLevel().ToString();
        sldHp.value = _saveData.currentHp / _entityData.info.stats.health;
        sldEnergy.value = _saveData.energy / 100;
    }

    private void SwitchHighlight(object condition)
    {
        bool active = false;

        if (_entityData != null)
        {
            switch (condition)
            {
                case Race r:
                    active = (_entityData.info.race == r);
                    break;
                case Element e:
                    active = (_entityData.info.element == e);
                    break;
            }
        }

        highlight.SetActive(active);
    }

    public void OnClickSlot()
    {
        _slotHighlighted?.Invoke(slotId, _saveData);
    }
}