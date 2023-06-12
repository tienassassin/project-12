using System;
using UnityEngine;

public enum Element
{
    Fire,
    Ice,
    Wind,
    Thunder,
}

public enum Faction
{
    Legion,
    Dynasty,
    Herald,
    Cosmos,
}

public enum DamageType
{
    Physical,
    Magical,
    Pure,
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
            armor = st1.armor + st2.armor,
            magicResistance = st1.magicResistance + st2.magicResistance,
            energyRegen = ClampRate(st1.energyRegen + st2.energyRegen),
            
            speed = ClampRate(st1.speed + st2.speed),
            critRate = ClampRate(st1.critRate + st2.critRate),
            critDmg = st1.critDmg + st2.critDmg,
            
            lifeSteal = st1.lifeSteal + st2.lifeSteal,
            armorPenetration = ClampRate(st1.armorPenetration + st2.armorPenetration, 80),
            mRPenetration = ClampRate(st1.mRPenetration + st2.mRPenetration, 80),
        };
    }
    
    public static Stats operator *(Stats st1, float rate)
    {
        return new Stats
        {
            health = st1.health * rate,
            pDmg = st1.pDmg * rate,
            mDmg = st1.mDmg * rate,
            armor = st1.armor * rate,
            magicResistance = st1.magicResistance * rate,
            energyRegen = ClampRate(st1.energyRegen * rate),
            
            speed = ClampRate(st1.speed * rate),
            critRate = ClampRate(st1.critRate * rate),
            critDmg = st1.critDmg * rate,
            
            lifeSteal = st1.lifeSteal * rate,
            armorPenetration = (ClampRate(st1.armorPenetration * rate, 80)),
            mRPenetration = ClampRate(st1.mRPenetration * rate, 80),
        };
    }

    private static float ClampRate(float value, float limit = 100)
    {
        return Mathf.Min(value, limit);
    }
}