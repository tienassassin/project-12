using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class CharacterInBattle : Character, IAttacker, IDefender
{
    [TitleGroup("BASE DATA:")]

    [TitleGroup("IN-GAME STATS")]
    [SerializeField] protected Stats stats;
    [SerializeField] protected float virtualHP;
    [SerializeField] protected float agility;
    [SerializeField] protected float rage;
    
    [TitleGroup("DEBUFF STATUS")]
    [SerializeField] protected bool hasStun;
    [SerializeField] protected bool hasSilent;
    [SerializeField] protected bool hasBleeding;

    [SerializeField] private bool isUltimateReady = false;
    
    #region Public properties

    public Element Element => baseChr.element;
    public Race Race => baseChr.race;
    public DamageType DmgType => baseChr.damageType;
    public bool IsAlive => curHP > 0;
    public bool CanTakeTurn => !hasStun;
    public bool CanUseSkill => !hasSilent;
    public bool HasHeal => !hasBleeding;
    
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
            else
            {
                // todo: disable ultimate skill
            }
        }
    }

    #endregion

    #region Callbacks
    
    public Action<float,float,float> OnUpdateHp;
    public Action<float> OnUpdateAnger;
    
    #endregion

    public override void Init(CharacterSaveData saveData)
    {
        base.Init(saveData);
        
        //todo: add equipment stats to overall stats

        stats = baseChr.stats;
         
        virtualHP = 0;
        agility = 0;
        rage = 0;

        hasStun = false;
        hasSilent = false;
        hasBleeding = false;
    }

    private void Start()
    {
        UpdateHp();
        UpdateAnger();
    }
    
    #region Consume Energy/Agility/...
    
    protected bool HasFullEnergy()
    {
        if (energy < 100) return false;
        energy -= 100;
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
        OnUpdateHp?.Invoke(curHP, virtualHP, stats.health);
    }

    protected void UpdateAnger()
    {
        OnUpdateAnger?.Invoke(rage);
    }

    #endregion

    public virtual void Attack(CharacterInBattle target) { }

    public virtual void DealDamage(IDefender target, float dmgAmount, DamageType dmgType) { }

    public virtual void TakeDamage(float dmgAmount, DamageType dmgType, float penetration) { }

    public void RegenHP(float hpAmount, bool allowOverflow = false)
    {
        float expectedHP = curHP + hpAmount;
        if (expectedHP > stats.health && allowOverflow)
        {
            float overflowAmount = expectedHP - stats.health;
            virtualHP += overflowAmount;
        }

        curHP = Mathf.Min(expectedHP, stats.health);
    }

    protected virtual void Die()
    {
        EditorLog.Message($"{name} dead");
    }
}
