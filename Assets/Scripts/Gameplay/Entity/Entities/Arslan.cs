using Sirenix.OdinInspector;

public class Arslan : Human
{
    [Button]
    public override float TakeDamage(IDamageDealer origin, Damage dmg)
    {
        return base.TakeDamage(origin, dmg);
    }

    [Button]
    public override void RegenHp(float hpAmount, bool allowOverflow = false)
    {
        base.RegenHp(hpAmount, allowOverflow);
    }
}