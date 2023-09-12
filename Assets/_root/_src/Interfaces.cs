using UnityEngine;

public interface IDamageDealer
{
    float DealDamage(IDamageTaker target, Damage dmg);
}

public interface IDamageTaker
{
    Vector3 GetRootPosition();
    Vector3 GetHitPosition();
    float TakeDamage(IDamageDealer origin, Damage dmg);
    float TakeFatalDamage(IDamageDealer origin);
}

public interface IRaceAura
{
    void ActiveRaceAura(int rank);
}

public interface IElementAura
{
    void ActiveElementAura(int rank);
}