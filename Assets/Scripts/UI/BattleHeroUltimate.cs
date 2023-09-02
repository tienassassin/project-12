using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleHeroUltimate : DuztineBehaviour
{
    [SerializeField] private Image imgSkill;
    [SerializeField] private Image imgHero;

    private int _id = -1;
    private bool _isAvailable;

    private void OnEnable()
    {
        this.AddListener(EventID.ON_ENERGY_UPDATED, OnEnergyUpdated);
    }

    private void OnDisable()
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
        var color = imgSkill.color;
        color.a = available ? 1f : 0.2f;
        imgSkill.color = color;
    }

    private void UseUltimateSkill()
    {
        if (!_isAvailable) return;

        // todo: review later
    }
}