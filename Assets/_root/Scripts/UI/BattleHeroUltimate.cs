using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleHeroUltimate : DuztineBehaviour
{
    [SerializeField] private Image imgSkill;
    [SerializeField] private Image imgHero;

    private int _id = -1;
    private bool _isAvailable;
    private Color _colorAvailable = Color.white;
    private Color _colorUnavailable = new(1, 1, 1, 0.2f);

    private void Awake()
    {
        this.AddListener(EventID.ON_ENERGY_UPDATED, OnEnergyUpdated);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_ENERGY_UPDATED, OnEnergyUpdated);
    }

    public void Init(int id, Sprite skill, Sprite hero)
    {
        _id = id;
        imgSkill.sprite = skill;

        SwitchAvailability(false);
    }

    private void OnEnergyUpdated(object data)
    {
        var (id, available) = (Tuple<int, bool>)data;
        if (id != _id) return;

        SwitchAvailability(available);
    }

    private void SwitchAvailability(bool available)
    {
        _isAvailable = available;
        imgSkill.color = available ? _colorAvailable : _colorUnavailable;
    }

    private void UseUltimateSkill()
    {
        if (!_isAvailable) return;

        // todo: review later
    }
}