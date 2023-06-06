public interface IAttacker
{
    void DealDamage(IDefender target, float dmgAmount, DamageType dmgType);
    void DealElementalDamage(IDefender target, float dmgAmount, Elements element);
}

public interface IDefender
{
    void TakeDamage(float dmgAmount, DamageType dmgType, float penetration);
    void TakeElementalDamage(float dmgAmount, Elements element);
}