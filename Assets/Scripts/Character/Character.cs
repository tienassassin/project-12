using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour, IAttacker, IDefender
{
    [TitleGroup("BASE DATA:")]
    [SerializeField] protected CharacterData charData;
    
    [TitleGroup("DATA")]
    [ShowInInspector] protected Element element;
    [ShowInInspector] protected Faction faction;
    [ShowInInspector] protected Stats stats;

    [TitleGroup("IN-GAME STATS")]
    [ShowInInspector] protected float curHP;
    [ShowInInspector] protected float energy;
    [ShowInInspector] protected float agility;
    [ShowInInspector] protected float virtualHP;

    protected bool canUseSkill;

    #region Public properties

    public float Speed => stats.speed;

    #endregion

    private DamageType GetDamageType()
    {
        switch (element)
        {
            case Element.Fire:
            case Element.Thunder:
                return DamageType.Magical;
            
            case Element.Wind:
            case Element.Ice:
                return DamageType.Physical;
        }

        return 0;
    }
    
    private void Awake()
    {
        name = charData.baseData.characterName;
        
        element = charData.baseData.element;
        faction = charData.baseData.faction;
        stats = charData.baseData.stats;
        
        charData.equipmentList.ForEach(e=>
        {
            stats += e.GetFullStats(faction);
        });
        
        curHP = stats.health;
        energy = 0;
        agility = 0;
        virtualHP = 0;
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

    private string json;

    [Button]
    public void Export()
    {
        json = JsonUtility.ToJson(charData);
    }

    [Button]
    public void Import()
    {
        charData = JsonUtility.FromJson<CharacterData>(json);
    }
}
