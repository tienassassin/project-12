using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour, IAttacker, IDefender
{
    [SerializeField] protected CharacterData charData;

    [SerializeField]
    private List<Equipment> equipmentList = new(); 
    
    [ShowInInspector] private Stats stats;

    #region Public properties

    public float Speed => stats.speed;

    #endregion

    private DamageType GetDamageType()
    {
        switch (charData.element)
        {
            case Elements.Fire:
            case Elements.Ice:
            case Elements.Shadow:
                return DamageType.Magical;
            
            case Elements.Wind:
            case Elements.Thunder:
            case Elements.Cosmos:
                return DamageType.Physical;
        }

        return DamageType.Magical;
    }

    [ShowInInspector] protected float curHP;
    [ShowInInspector] protected float energy;
    [ShowInInspector] protected float agility;
    [ShowInInspector] protected float virtualHP;
    [ShowInInspector] protected float endurance;

    protected bool canUseSkill;

    private void Awake()
    {
        stats = charData.stats;
        equipmentList.ForEach(e=>
        {
            stats += e.GetEquipmentStats(charData.race);
        });
        
        curHP = stats.health;
        energy = 0;
        agility = 0;
        virtualHP = 0;
        endurance = 100;
        canUseSkill = false;
        
        OnRegistration();
    }

    protected virtual void OnRegistration(){}

    protected void ResetEnergy()
    {
        energy = 0;
    }
    

    protected void ResetAgility()
    {
        agility = 0;
    }

    protected void ResetEndurance()
    {
        endurance = 100;
    }
    

    protected void Attack(Character target)
    {
        bool isCrit = Utils.GetRandomResult(stats.critRate);
        DamageType dmgType = GetDamageType();
        float dmg = (dmgType == DamageType.Physical) ? stats.pDmg : stats.mDmg;
        if (isCrit)
        {
            dmg *= (stats.critDmg / 100f);
        }
        
        DealDamage(target, dmg, dmgType);
        DealElementalDamage(target, stats.eDmg, charData.element);
    }

    public void DealDamage(IDefender target, float dmgAmount, DamageType dmgType)
    {
        float penetration = 0;
        switch (dmgType)
        {
            case DamageType.Physical:
                penetration = stats.armorPenetration;
                break;
            case DamageType.Magical:
                penetration = stats.mRPenetration;
                break;
            case DamageType.Pure:
                penetration = 0;
                break;
        }

        energy += stats.energyRegen;
        if (energy >= 100)
        {
            ResetEnergy();
        }

        agility += stats.speed;
        if (agility >= 100)
        {
            ResetAgility();
        }
        
        target.TakeDamage(dmgAmount,dmgType,penetration);
    }

    public void DealElementalDamage(IDefender target, float dmgAmount, Elements element)
    {
        target.TakeElementalDamage(dmgAmount, element);
    }

    public void TakeDamage(float dmgAmount, DamageType dmgType, float penetration)
    {
        float defense = 0;
        switch (dmgType)
        {
            case DamageType.Physical:
                defense = stats.armor;
                break;
            case DamageType.Magical:
                defense = stats.magicResistance;
                break;
            case DamageType.Pure:
                defense = 0;
                break;
        }

        float dmgReduction = defense * (1 - penetration/100f);
        float takenDmg = Mathf.Max(1, dmgAmount - dmgReduction);

        float dmgLoss = Mathf.Min(takenDmg, virtualHP);
        takenDmg -= dmgLoss;
        virtualHP -= dmgLoss;
        
        curHP -= takenDmg;

        if (curHP <= 0)
        {
            Die();
        }
    }

    public void TakeElementalDamage(float dmgAmount, Elements element)
    {
        if (!charData.weakness.Contains(element) && element != Elements.Cosmos) return;

        endurance -= dmgAmount;
        if (endurance <= 0)
        {
            ExposeWeakness();
            ResetEndurance();
        }
    }

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

    private void Die()
    {
        Debug.Log($"{name} dead");
    }

    private void ExposeWeakness()
    {
        Debug.Log($"{name} exposed weakness");
    }
}
