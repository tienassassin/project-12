using Sirenix.OdinInspector;

public abstract class Human : BattleEntity, IRaceAura
{
    [TitleGroup("HUMAN AURA:")]
    [ShowInInspector] protected float InstantKillThreshold;
    [ShowInInspector] protected float InstantKillMaxDamage;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                InstantKillThreshold = 0.03f;
                InstantKillMaxDamage = 2;
                break;

            case 4:
                InstantKillThreshold = 0.05f;
                InstantKillMaxDamage = 3;
                break;
        }
    }

    public override float DealDamage(IDamageTaker target, Damage dmg)
    {
        float dmgDealt = base.DealDamage(target, dmg);
        if (target is BattleEntity entity)
        {
            if (entity.HpPercentage - 0.001f <= InstantKillThreshold &&
                entity.Hp <= Stats.damage * InstantKillMaxDamage)
            {
                target.TakeFatalDamage(this);
            }
        }

        return dmgDealt;
    }
}