using System;
using UnityEngine;

public enum Elements
{
    Fire,
    Ice,
    Wind,
    Thunder,
    Cosmos,
    Shadow,
}

public enum Races
{
    Human,
    Beast,
    Mecha,
}

public enum DamageType
{
    Physical,
    Magical,
    Pure
}

public enum EquipmentType
{
    Headgear,
    Garment,
    Footwear,
    
    LArm,
    RArm,
    
    Relic,
}

[Serializable]
public struct Stats
{
    public float health;
    public float pDmg;
    public float mDmg;
    public float eDmg;
    public float armor;
    public float magicResistance;
    public float energyRegen;

    public float speed;
    public float critRate;
    public float critDmg;

    public float lifeSteal;
    public float armorPenetration;
    public float mRPenetration;

    public static Stats operator +(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health + st2.health,
            pDmg = st1.pDmg + st2.pDmg,
            mDmg = st1.mDmg + st2.mDmg,
            eDmg = ClampRate(st1.eDmg + st2.eDmg),
            armor = st1.armor + st2.armor,
            magicResistance = st1.magicResistance + st2.magicResistance,
            energyRegen = ClampRate(st1.energyRegen + st2.energyRegen),
            
            speed = ClampRate(st1.speed + st2.speed),
            critRate = ClampRate(st1.critRate + st2.critRate),
            critDmg = st1.critDmg + st2.critDmg,
            
            lifeSteal = st1.lifeSteal + st2.lifeSteal,
            armorPenetration = ClampRate(st1.armorPenetration + st2.armorPenetration),
            mRPenetration = ClampRate(st1.mRPenetration + st2.mRPenetration),
        };
    }
    
    public static Stats operator *(Stats st1, float rate)
    {
        return new Stats
        {
            health = st1.health * rate,
            pDmg = st1.pDmg * rate,
            mDmg = st1.mDmg * rate,
            eDmg = ClampRate(st1.eDmg * rate),
            armor = st1.armor * rate,
            magicResistance = st1.magicResistance * rate,
            energyRegen = ClampRate(st1.energyRegen * rate),
            
            speed = ClampRate(st1.speed * rate),
            critRate = ClampRate(st1.critRate * rate),
            critDmg = st1.critDmg * rate,
            
            lifeSteal = st1.lifeSteal * rate,
            armorPenetration = (ClampRate(st1.armorPenetration * rate)),
            mRPenetration = ClampRate(st1.mRPenetration * rate),
        };
    }

    private static float ClampRate(float value, float limit = 100)
    {
        return Mathf.Min(value, limit);
    }
}