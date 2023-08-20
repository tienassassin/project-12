using System;
using DB.System;
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

    private Action<int, DB.Player.Hero> _slotHighlighted;
    private DB.Player.Hero _saveData;
    private DB.System.Hero _baseData;

    private void OnEnable()
    {
        this.AddListener(EventID.ON_HIGHLIGHT_AURA, SwitchHighlight);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_HIGHLIGHT_AURA, SwitchHighlight);
    }

    public void Init(DB.Player.Hero data, Action<int, DB.Player.Hero> slotHighlighted)
    {
        _saveData = data;
        _baseData = _saveData?.GetHeroWithID();
        hero.SetActive(_saveData != null);
        heroInfo.SetActive(_saveData != null);
        name = (_baseData != null ? _baseData.Name : Constants.EMPTY_MARK);

        _slotHighlighted = slotHighlighted;
        Refresh();
    }

    private void Refresh()
    {
        if (_saveData == null) return;
        
        txtLevel.text = _saveData.GetLevel().ToString();
        sldHp.value = _saveData.curHp / _baseData.Stats.health;
        sldEnergy.value = _saveData.energy / 100;
    }

    private void SwitchHighlight(object condition)
    {
        bool active = false;
        
        if (_baseData != null)
        {
            switch (condition)
            {
                case Race r:
                    active = (_baseData.Race == r);
                    break;
                case Element e:
                    active = (_baseData.Element == e);
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