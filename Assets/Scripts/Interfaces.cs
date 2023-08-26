public interface IDamageDealer
{
    float DealDamage(IDamageTaker target, Damage dmg);
}

public interface IDamageTaker
{
    float TakeDamage(IDamageDealer origin, Damage dmg);
}

public interface IRaceAura
{
    void ActiveRaceAura(int rank);
}

public interface IElementAura
{
    void ActiveElementAura(int rank);
}