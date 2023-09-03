using System;
using Sirenix.OdinInspector;
using UnityEngine;

public enum Element
{
    Fire,
    Ice,
    Wind,
    Thunder
}

public enum Race
{
    Human,
    Beast,
    Mecha
}

public enum Tier
{
    D,
    C,
    B,
    A,
    S,
    X
}

public enum DamageType
{
    Physical,
    Magical,
    Pure
}

public enum AttackRange
{
    Ranged,
    Melee
}

public enum Rarity
{
    Normal,
    Rare,
    Epic,
    Legendary,
    Mythic,
    Relic
}

public enum Slot
{
    Weapon,
    Headgear,
    Garment,
    Jewelry
}

public enum Requirement
{
    None,
    Physical,
    Magical
}

public enum Effect
{
    //buff
    Immortal,

    //debuff
    Stun,
    Silent,
    Bleeding
}

public enum SortType
{
    None,
    Ascending,
    Descending
}

public enum SkillTargetType
{
    Auto, // automatically cast, no need to choose target
    Ally, // target can be an ally
    AllyOrSelf, // target can be an ally or self
    Enemy, // target can be an enemy
    EnemyOrSelf, // target can be an enemy or self
    ExceptSelf, // target can be an ally or an enemy
    All // target can be anyone
}

public enum Faction
{
    Hero,
    Devil
}

public enum HealthImpactType
{
    None,
    Healing,
    PureDamage,
    CriticalPureDamage,
    PhysicalDamage,
    CriticalPhysicalDamage,
    MagicalDamage,
    CriticalMagicalDamage
}

public static class TypeExtensions
{
    public static bool IsNull(this HealthImpactType type)
    {
        return type is HealthImpactType.None;
    }

    public static bool IsHit(this HealthImpactType type)
    {
        return type.IsNormalHit() || type.IsCriticalHit();
    }

    public static bool IsNormalHit(this HealthImpactType type)
    {
        return type is HealthImpactType.PureDamage
            or HealthImpactType.PhysicalDamage
            or HealthImpactType.MagicalDamage;
    }

    public static bool IsCriticalHit(this HealthImpactType type)
    {
        return type is HealthImpactType.CriticalPureDamage
            or HealthImpactType.CriticalPhysicalDamage
            or HealthImpactType.CriticalMagicalDamage;
    }

    public static bool IsHealing(this HealthImpactType type)
    {
        return type is HealthImpactType.Healing;
    }
}

[Serializable]
public struct Stats
{
    [HideInInspector]
    public bool hideZero;

    [ShowIf("@!hideZero || this.health > 0")]
    public float health;
    [ShowIf("@!hideZero || this.damage > 0")]
    public float damage;
    [ShowIf("@!hideZero || this.armor > 0")]
    public float armor;
    [ShowIf("@!hideZero || this.resistance > 0")]
    public float resistance;

    [ShowIf("@!hideZero || this.intelligence > 0")]
    public float intelligence;
    [ShowIf("@!hideZero || this.speed > 0")]
    public float speed;
    [ShowIf("@!hideZero || this.luck > 0")]
    public float luck;
    [ShowIf("@!hideZero || this.critDamage > 0")]
    public float critDamage;

    [ShowIf("@!hideZero || this.lifeSteal > 0")]
    public float lifeSteal;
    [ShowIf("@!hideZero || this.accuracy > 0")]
    public float accuracy;

    public Stats GetStatsByLevel(int level, float growth)
    {
        return new Stats
        {
            health = health * Mathf.Pow(1 + growth, level - 1),
            damage = damage * Mathf.Pow(1 + growth, level - 1),
            armor = armor * Mathf.Pow(1 + growth, level - 1),
            resistance = resistance * Mathf.Pow(1 + growth, level - 1),

            intelligence = intelligence,
            speed = speed,
            luck = luck,
            critDamage = critDamage,

            lifeSteal = lifeSteal,
            accuracy = accuracy
        };
    }

    public static Stats operator +(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health + st2.health,
            damage = st1.damage + st2.damage,
            armor = st1.armor + st2.armor,
            resistance = st1.resistance + st2.resistance,

            intelligence = Clamp(st1.intelligence + st2.intelligence, 100),
            speed = Clamp(st1.speed + st2.speed, 100),
            luck = Clamp(st1.luck + st2.luck, 100),
            critDamage = st1.critDamage + st2.critDamage,

            lifeSteal = st1.lifeSteal + st2.lifeSteal,
            accuracy = Clamp(st1.accuracy + st2.accuracy, 80)
        };
    }

    public static Stats operator -(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health - st2.health,
            damage = st1.damage - st2.damage,
            armor = st1.armor - st2.armor,
            resistance = st1.resistance - st2.resistance,

            intelligence = st1.intelligence - st2.intelligence,
            speed = st1.speed - st2.speed,
            luck = st1.luck - st2.luck,
            critDamage = st1.critDamage - st2.critDamage,

            lifeSteal = st1.lifeSteal - st2.lifeSteal,
            accuracy = st1.accuracy - st2.accuracy
        };
    }

    public static Stats operator *(Stats st1, float rate)
    {
        return new Stats
        {
            health = st1.health * rate,
            damage = st1.damage * rate,
            armor = st1.armor * rate,
            resistance = st1.resistance * rate,

            intelligence = Clamp(st1.intelligence * rate, 100),
            speed = Clamp(st1.speed * rate, 100),
            luck = Clamp(st1.luck * rate, 100),
            critDamage = st1.critDamage * rate,

            lifeSteal = st1.lifeSteal * rate,
            accuracy = Clamp(st1.accuracy * rate, 80)
        };
    }

    private static float Clamp(float value, float limit)
    {
        return Mathf.Min(value, limit);
    }
}