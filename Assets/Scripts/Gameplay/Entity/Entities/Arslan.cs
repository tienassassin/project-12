using Sirenix.OdinInspector;

public class Arslan : HumanEntity
{
    [Button]
    public override float TakeDamage(IDamageDealer origin, Damage dmg)
    {
        return base.TakeDamage(origin, dmg);
    }
}