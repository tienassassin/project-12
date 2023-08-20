using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class HeroInBattle : Hero, IAttacker, IDefender
{
    [TitleGroup("IN-GAME STATS")]
    [SerializeField] protected Stats stats;
    [SerializeField] protected float virtualHp;
    [SerializeField] protected float agility;
    [SerializeField] protected float rage;
    
    [TitleGroup("DEBUFF STATUS")]
    [ShowInInspector] protected bool IsStun;
    [ShowInInspector] protected bool IsSilent;
    [ShowInInspector] protected bool IsBleeding;

    [ShowInInspector] private bool _isUltimateReady;
    
    #region Public properties

    public DamageType DamageType => BaseData.DamageType;
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
            else
            {
                // todo: disable ultimate skill
            }
        }
    }

    #endregion

    #region Callbacks
    
    public Action<float,float,float> hpUpdated;
    public Action<float> angerUpdated;
    
    #endregion

    public override void Init(DB.Player.Hero saveData)
    {
        base.Init(saveData);
        
        //todo: add equipment stats to overall stats

        stats = BaseData.Stats;
         
        virtualHp = 0;
        agility = 0;
        rage = 0;

        IsStun = false;
        IsSilent = false;
        IsBleeding = false;
    }

    private void Start()
    {
        UpdateHp();
        UpdateAnger();
    }
    
    #region Consume Energy/Agility/...
    
    protected bool HasFullEnergy()
    {
        if (Energy < 100) return false;
        Energy -= 100;
        return true;
    }
    
    protected bool HasFullAgility()
    {
        if (agility < 100) return false;
        agility -= 100;
        return true;
    }

    #endregion

    #region Update UI

    protected void UpdateHp()
    {
        hpUpdated?.Invoke(Hp, virtualHp, stats.health);
    }

    protected void UpdateAnger()
    {
        angerUpdated?.Invoke(rage);
    }

    #endregion

    public virtual void Attack(HeroInBattle target) { }

    public virtual void DealDamage(IDefender target, float dmgAmount, DamageType dmgType) { }

    public virtual void TakeDamage(float dmgAmount, DamageType dmgType, float penetration) { }

    public void RegenHp(float hpAmount, bool allowOverflow = false)
    {
        float expectedHp = Hp + hpAmount;
        if (expectedHp > stats.health && allowOverflow)
        {
            float overflowAmount = expectedHp - stats.health;
            virtualHp += overflowAmount;
        }

        Hp = Mathf.Min(expectedHp, stats.health);
    }

    protected virtual void Die()
    {
        EditorLog.Message($"{name} dead");
    }
}
