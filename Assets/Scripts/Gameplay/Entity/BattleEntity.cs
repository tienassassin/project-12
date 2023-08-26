using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BattleEntity : DuztineBehaviour, IDamageDealer, IDamageTaker
{
    [TitleGroup("BASE DATA:")]
    protected Entity BaseData;
    [ShowInInspector] protected List<Equipment> EqmList = new();

    [TitleGroup("IN-GAME STATS:")]
    [ShowInInspector] protected Stats Stats;
    [ShowInInspector] protected float Hp;
    [ShowInInspector] protected float VirtualHp;
    [ShowInInspector] protected float Energy;
    [ShowInInspector] protected float Agility;
    [ShowInInspector] protected float Rage;

    [TitleGroup("STATUS:")]
    [ShowInInspector] protected bool IsStun;
    [ShowInInspector] protected bool IsSilent;
    [ShowInInspector] protected bool IsBleeding;
    [ShowInInspector] protected bool IsImmortal;
    [ShowInInspector] private bool _isUltimateReady;

    #region Events

    public Action<float, float, float> hpUpdated;
    public Action<float, float> energyUpdated;
    public Action<float> rageUpdated;

    #endregion


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
        UpdateEnergy();
        UpdateRage();
    }

    #region UI

    protected virtual void UpdateHp()
    {
        hpUpdated?.Invoke(Hp, VirtualHp, Stats.health);
    }

    protected virtual void UpdateEnergy()
    {
        energyUpdated?.Invoke(Energy, 100);
    }

    protected virtual void UpdateRage()
    {
        rageUpdated?.Invoke(Rage);
    }

    private void SpawnHpText(bool isHealing, float amount, int division, float duration)
    {
        if (amount < 1)
        {
            string dmgTxt = isHealing ? "+0" : "immortal";
            EditorLog.Message(name + dmgTxt);
        }
        else
        {
            if (division < 1) division = 1;
            int amountPerHit = (int)(amount / division);
            float interval = duration / division;

            for (int i = 0; i < division; i++)
            {
                string dmgTxt = (isHealing ? "+" : "-") + amountPerHit;
                EditorLog.Message(name + dmgTxt);
            }
        }
    }

    #endregion


    #region Resources consumption

    protected virtual bool HasFullEnergy()
    {
        if (Energy < 100) return false;
        Energy -= 100;
        return true;
    }

    protected virtual bool HasFullAgility()
    {
        if (Agility < 100) return false;
        Agility -= 100;
        return true;
    }

    #endregion


    #region Actions

    public virtual void Attack(IDamageTaker target)
    {
        Rage += Stats.luck;
        bool crit = Utils.GetRandomResult(Rage);
        if (crit)
        {
            Rage = Mathf.Max(0, Rage - 100);
        }

        if (Energy < 100) Energy += Stats.intelligence;
        var dmg = new Damage(Stats.damage, DamageType, Stats.accuracy / 100, crit);
        float actualDmgDealt = DealDamage(target, dmg);

        RegenHp(actualDmgDealt * (Stats.lifeSteal / 100));
    }

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

    public virtual float DealDamage(IDamageTaker target, Damage dmg)
    {
        return target.TakeDamage(this, dmg);
    }

    public virtual float TakeDamage(IDamageDealer origin, Damage dmg)
    {
        if (IsImmortal)
        {
            SpawnHpText(false, 0, 1, 0);
            return 0;
        }

        float dmgReduction = 0;
        switch (dmg.Type)
        {
            case DamageType.Physical:
                dmgReduction = Stats.armor * (1 - dmg.Penetration);
                break;
            case DamageType.Magical:
                dmgReduction = Stats.resistance * (1 - dmg.Penetration);
                break;
            case DamageType.Pure:
                dmgReduction = 0;
                break;
        }

        float dmgTaken = Mathf.Max(1, dmg.Amount - dmgReduction);
        SpawnHpText(false, dmgTaken, dmg.Division, dmg.Duration);

        // the displayed damage has no limit,
        // but the actual damage taken cant exceed the current hp
        float actualDmgTaken = Mathf.Min(dmgTaken, Hp + VirtualHp);
        float vhpAffected = Mathf.Min(actualDmgTaken, VirtualHp);
        float hpAffected = actualDmgTaken - vhpAffected;

        VirtualHp -= vhpAffected;
        Hp -= hpAffected;

        if (Hp < 1)
        {
            Die();
        }

        return actualDmgTaken;
    }

    public virtual void Die()
    {
        EditorLog.Message($"{name} dead");
    }

    #endregion
}

public struct Damage
{
    public float Amount;
    public DamageType Type;
    public float Penetration;
    public bool IsCritical;
    public int Division;
    public float Duration;

    public Damage(float amount, DamageType type, float penetration,
        bool isCritical = false, int division = 1, float duration = 0.5f)
    {
        Amount = amount;
        Type = type;
        Penetration = penetration;
        IsCritical = isCritical;
        Division = division;
        Duration = duration;
    }
}