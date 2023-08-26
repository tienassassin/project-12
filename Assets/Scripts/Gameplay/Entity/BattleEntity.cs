using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BattleEntity : DuztineBehaviour, IDamageDealer, IDamageTaker
{
    [TitleGroup("BASE DATA:")]
    [ShowInInspector] private Faction _faction;
    protected Entity BaseData;
    [ShowInInspector] protected List<Equipment> EqmList = new();

    [TitleGroup("IN-GAME STATS:")]
    [ShowInInspector] private Stats _stats;
    [ShowInInspector] private float _hp;
    [ShowInInspector] private float _virtualHp;
    [ShowInInspector] private float _energy;
    [ShowInInspector] private float _agility;
    [ShowInInspector] private float _rage;

    [TitleGroup("STATUS:")]
    [ShowInInspector] private bool _isStun;
    [ShowInInspector] private bool _isSilent;
    [ShowInInspector] private bool _isBleeding;
    [ShowInInspector] private bool _isImmortal;
    [ShowInInspector] private bool _isUltimateReady;

    #region Events

    // hp, vHp, maxHp, duration
    public Action<float, float, float, float> hpUpdated;
    // energy, maxEnergy, duration
    public Action<float, float, float> energyUpdated;
    public Action<float> rageUpdated;

    #endregion


    #region Public properties

    public Faction Faction => _faction;
    public Stats Stats
    {
        get => _stats;
        protected set => _stats = value;
    }
    public DamageType DamageType => BaseData.damageType;
    public Element Element => BaseData.element;
    public Race Race => BaseData.race;
    public bool IsAlive => _hp > 0;
    public float HpPercentage => _hp / Stats.health;
    public float Hp
    {
        get => _hp;
        protected set => _hp = value;
    }
    public float VirtualHp
    {
        get => _virtualHp;
        protected set => _virtualHp = value;
    }
    public float Energy
    {
        get => _energy;
        protected set => _energy = value;
    }
    public float Agility
    {
        get => _agility;
        protected set => _agility = value;
    }
    public float Rage
    {
        get => _rage;
        protected set => _rage = value;
    }
    public bool IsStun
    {
        get => _isStun;
        protected set => _isStun = value;
    }
    public bool IsSilent
    {
        get => _isSilent;
        protected set => _isSilent = value;
    }
    public bool IsBleeding
    {
        get => _isBleeding;
        protected set => _isBleeding = value;
    }
    public bool IsImmortal
    {
        get => _isImmortal;
        protected set => _isImmortal = value;
    }
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
    public bool CanTakeTurn => !_isStun;

    #endregion


    public void Init(HeroData heroData)
    {
        _faction = Faction.Hero;
        BaseData = heroData.GetHero();
        SetupInfo();

        Hp = (heroData.curHp / 100) * Stats.health;
        Energy = heroData.energy;
        SetupStats();
    }

    public void Init(DevilData devilData)
    {
        _faction = Faction.Devil;
        BaseData = devilData.GetDevil();
        SetupInfo();

        Hp = Stats.health;
        Energy = 0;
        SetupStats();
    }

    private void SetupInfo()
    {
        Stats = BaseData.stats;
        name = BaseData.name;
    }

    private void SetupStats()
    {
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

    protected virtual void UpdateHp(float duration = 1f)
    {
        hpUpdated?.Invoke(Hp, VirtualHp, Stats.health, duration);
    }

    protected virtual void UpdateEnergy(float duration = 1f)
    {
        energyUpdated?.Invoke(Energy, 100, duration);
    }

    protected virtual void UpdateRage()
    {
        rageUpdated?.Invoke(Rage);
    }

    protected void SpawnHpText(bool isHealing, float amount, int division, float duration)
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
        IsUltimateReady = Energy >= 100;

        var dmg = new Damage(Stats.damage, DamageType, Stats.accuracy / 100, crit);
        float dmgDealt = DealDamage(target, dmg);

        RegenHp(dmgDealt * (Stats.lifeSteal / 100));
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

        UpdateHp();
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

        // the displayed damage has no limit,
        // but the actual damage taken cant exceed the current hp
        float actualDmgTaken = Mathf.Min(dmgTaken, Hp + VirtualHp);
        float vhpAffected = Mathf.Min(actualDmgTaken, VirtualHp);
        float hpAffected = actualDmgTaken - vhpAffected;

        VirtualHp -= vhpAffected;
        Hp -= hpAffected;

        SpawnHpText(false, dmgTaken, dmg.Division, dmg.Duration);
        UpdateHp(dmg.Duration);

        if (Hp < 1)
        {
            Die();
        }

        return actualDmgTaken;
    }

    public virtual float TakeFatalDamage(IDamageDealer origin)
    {
        if (IsImmortal)
        {
            SpawnHpText(false, 0, 1, 0);
            return 0;
        }

        float fatalDamage = 999;
        float actualDmgTaken = Hp + VirtualHp;
        while (fatalDamage < actualDmgTaken)
        {
            fatalDamage = fatalDamage * 10 + 9;
        }

        VirtualHp = 0;
        Hp = 0;

        SpawnHpText(true, fatalDamage, 1, 0);
        UpdateHp();
        Die();

        return actualDmgTaken;
    }

    public virtual void Die()
    {
        EditorLog.Message($"{name} dead");
    }

    #endregion
}

public enum Faction
{
    Hero,
    Devil
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