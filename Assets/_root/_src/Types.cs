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

public enum Side
{
    Ally,
    Enemy
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