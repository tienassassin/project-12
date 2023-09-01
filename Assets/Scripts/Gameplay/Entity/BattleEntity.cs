using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BattleEntity : DuztineBehaviour, IDamageDealer, IDamageTaker
{
    private static int _autogeneratedId;

    [TitleGroup("BASE DATA:")]
    [SerializeField] private int id;
    [SerializeField] protected Entity baseData;
    [SerializeField] private int level;
    [SerializeField] private Faction faction;
    [SerializeField] protected List<Equipment> eqmList = new();

    [TitleGroup("IN-GAME STATS:")]
    [SerializeField] private Stats stats;
    [SerializeField] private float hp;
    [SerializeField] private float virtualHp;
    [SerializeField] private float energy;
    [SerializeField] private float agility;
    [SerializeField] private float rage;

    [TitleGroup("STATUS:")]
    [SerializeField] private bool isStun;
    [SerializeField] private bool isSilent;
    [SerializeField] private bool isBleeding;
    [SerializeField] private bool isImmortal;
    [SerializeField] private bool isUltimateReady;

    private EntityUI _entityUI;

    #region Events

    public Action<float> rageUpdated;

    #endregion


    #region Public properties

    public int ID => id;
    public Faction Faction => faction;
    public Stats Stats
    {
        get => stats;
        protected set => stats = value;
    }
    public DamageType DamageType => baseData.damageType;
    public Element Element => baseData.element;
    public Race Race => baseData.race;
    public bool IsAlive => hp > 0;
    public float HpPercentage => hp / Stats.health;
    public float Hp
    {
        get => hp;
        protected set => hp = value;
    }
    public float VirtualHp
    {
        get => virtualHp;
        protected set => virtualHp = value;
    }
    public float Energy
    {
        get => energy;
        protected set => energy = value;
    }
    public float Agility
    {
        get => agility;
        protected set => agility = value;
    }
    public float Rage
    {
        get => rage;
        protected set => rage = value;
    }
    public bool IsStun
    {
        get => isStun;
        protected set => isStun = value;
    }
    public bool IsSilent
    {
        get => isSilent;
        protected set => isSilent = value;
    }
    public bool IsBleeding
    {
        get => isBleeding;
        protected set => isBleeding = value;
    }
    public bool IsImmortal
    {
        get => isImmortal;
        protected set => isImmortal = value;
    }
    public bool IsUltimateReady
    {
        get => isUltimateReady;
        protected set
        {
            isUltimateReady = value;
            if (value)
            {
                // todo: enable ultimate skill
            }
            // todo: disable ultimate skill
        }
    }
    public bool CanTakeTurn => !isStun;

    #endregion

    private void Awake()
    {
        _entityUI = GetComponent<EntityUI>();
    }

    public void Init(HeroData heroData)
    {
        faction = Faction.Hero;
        baseData = heroData.GetHero();
        level = heroData.GetLevel();
        SetupInfo();

        Hp = (heroData.curHp / 100) * Stats.health;
        Energy = heroData.energy;
        SetupStats();
    }

    public void Init(DevilData devilData)
    {
        faction = Faction.Devil;
        baseData = devilData.GetDevil();
        level = devilData.level;
        SetupInfo();

        Hp = Stats.health;
        Energy = 0;
        SetupStats();
    }

    private void SetupInfo()
    {
        id = _autogeneratedId;
        _autogeneratedId++;
        Stats = baseData.stats; // base stats
        Stats = Stats.GetStatsByLevel(level, baseData.GetEntityGrowth()); // level-based level
        // Stats = Stats; // todo: overall stats
        name = baseData.name;
    }

    private void SetupStats()
    {
        VirtualHp = 0;
        Agility = 0;
        Rage = 0;

        IsStun = false;
        IsSilent = false;
        IsBleeding = false;

        SetupHpSegment();
        UpdateHp();
        UpdateEnergy();
        UpdateRage();
    }

    #region UI

    protected virtual void SetupHpSegment()
    {
        _entityUI.SetupHpSegment(stats.health);
    }

    protected virtual void UpdateHp(float duration = 1f)
    {
        _entityUI.UpdateHp(Hp, VirtualHp, Stats.health, duration);
    }

    protected virtual void UpdateEnergy(float duration = 1f)
    {
        _entityUI.UpdateEnergy(Energy, 100, duration);
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

    public virtual void UseSkill(IDamageTaker target)
    {
    }

    public virtual void UseUltimate(IDamageTaker target)
    {
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