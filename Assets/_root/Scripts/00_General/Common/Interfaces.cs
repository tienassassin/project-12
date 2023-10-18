using UnityEngine;

public interface IDamageDealer
{
    int DealDamage(IDamageTaker target, Damage dmg);
}

public interface IDamageTaker
{
    Vector3 GetRootPosition();
    Vector3 GetHitPosition();
    int TakeDamage(IDamageDealer origin, Damage dmg);
    int TakeFatalDamage(IDamageDealer origin);
}

public interface IRaceAura
{
    void ActiveRaceAura(int rank);
}

public interface IElementAura
{
    void ActiveElementAura(int rank);
}