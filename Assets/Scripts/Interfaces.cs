public interface IAttacker
{
    void DealDamage(IDefender target, float dmgAmount, DamageType dmgType);
}

public interface IDefender
{
    void TakeDamage(float dmgAmount, DamageType dmgType, float penetration);
}