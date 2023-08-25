using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BattleEntity : DuztineBehaviour, IAttacker, IDefender
{
    public Action<float, float, float> hpUpdated;
    public Action<float> rageUpdated;
    [ShowInInspector] private bool _isUltimateReady;
    [ShowInInspector] protected float Agility;
    [TitleGroup("BASE DATA")]
    protected Entity BaseData;
    [ShowInInspector] protected float Energy;
    protected List<Equipment> EqmList = new();
    [ShowInInspector] protected float Hp;
    [ShowInInspector] protected bool IsBleeding;
    [ShowInInspector] protected bool IsSilent;

    [TitleGroup("DEBUFF STATUS")]
    [ShowInInspector] protected bool IsStun;
    [ShowInInspector] protected float Rage;

    [TitleGroup("IN-GAME STATS")]
    [ShowInInspector] protected Stats Stats;
    [ShowInInspector] protected float VirtualHp;

    public abstract void DealDamage(IDefender target, float dmgAmount, DamageType dmgType);

    public abstract void TakeDamage(float dmgAmount, DamageType dmgType, float penetration);

    public void Init(HeroData heroData)
    {
        BaseData = heroData.GetHero();
        Setup();

        Hp = (heroData.curHp / 100) * Stats.health;
        Energy = heroData.energy;
    }

    public void Init(DevilData devilData)
    {
        BaseData = devilData.GetDevil();
        Setup();

        Hp = Stats.health;
        Energy = 0;
    }

    private void Setup()
    {
        Stats = BaseData.stats;
        name = BaseData.name;

        VirtualHp = 0;
        Agility = 0;
        Rage = 0;

        IsStun = false;
        IsSilent = false;
        IsBleeding = false;

        UpdateHp();
        UpdateRage();
    }

    public abstract void Attack(BattleEntity target);

    public virtual void RegenHp(float hpAmount, bool allowOverflow = false)
    {
        float expectedHp = Hp + hpAmount;
        if (expectedHp > Stats.health && allowOverflow)
        {
            float overflowAmount = expectedHp - Stats.health;
            VirtualHp += overflowAmount;
        }

        Hp = Mathf.Min(expectedHp, Stats.health);
    }

    protected virtual void Die()
    {
        EditorLog.Message($"{name} dead");
    }

    #region Public properties

    public DamageType DamageType => BaseData.damageType;
    public Element Element => BaseData.element;
    public Race Race => BaseData.race;
    public bool IsAlive => Hp > 0;
    public bool CanTakeTurn => !IsStun;
    public bool CanUseSkill => !IsSilent;
    public bool CanHeal => !IsBleeding;

    public bool IsUltimateReady
    {
        get => _isUltimateReady;
        protected set
        {
            _isUltimateReady = value;
            if (value)
            {
                // todo: enable ultimate skill
            }
            // todo: disable ultimate skill
        }
    }

    #endregion

    #region Consume Energy/Agility/...

    protected bool HasFullEnergy()
    {
        if (Energy < 100) return false;
        Energy -= 100;
        return true;
    }

    protected bool HasFullAgility()
    {
        if (Agility < 100) return false;
        Agility -= 100;
        return true;
    }

    #endregion

    #region Update UI

    protected virtual void UpdateHp()
    {
        hpUpdated?.Invoke(Hp, VirtualHp, Stats.health);
    }

    protected virtual void UpdateRage()
    {
        rageUpdated?.Invoke(Rage);
    }

    #endregion
}