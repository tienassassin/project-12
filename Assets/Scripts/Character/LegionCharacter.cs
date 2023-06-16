using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LegionCharacter : Character
{
    public string target;
    
    [Button]
    public void TestAttack()
    {
        Attack(GameObject.Find(target).GetComponent<Character>());
    }

    [Button]
    public void TestRegen(int amount, bool overflow)
    {
        RegenHP(amount, overflow);
    }

    public override void Attack(Character target)
    {
        bool isCrit = Utils.GetRandomResult(anger);
        float dmg = (DamageType == DamageType.Physical) ? stats.pDmg : stats.mDmg;
        if (isCrit)
        {
            dmg *= (stats.critDmg / 100f);
            anger = Mathf.Max(anger - 100, 0);
        }
        else
        {
            anger += stats.critRate;
        }
        
        UpdateAnger();
        DealDamage(target, dmg, DamageType);
    }

    public override void DealDamage(IDefender target, float dmgAmount, DamageType dmgType)
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
        if (HasFullEnergy())
        {
            IsUltimateReady = true;
        }
        
        target.TakeDamage(dmgAmount,dmgType,penetration);
    }

    public override void TakeDamage(float dmgAmount, DamageType dmgType, float penetration)
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

        curHP = Mathf.Clamp(curHP - takenDmg, 0, stats.health);
        UpdateHp();

        if (curHP < 0.01)
        {
            Die();
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}
