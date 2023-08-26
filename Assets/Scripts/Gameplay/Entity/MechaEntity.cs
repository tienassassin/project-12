public abstract class MechaEntity : BattleEntity, IRaceAura
{
    protected bool HasResurrected;
    protected bool IsInHibernation;
    protected float HibernationHpThreshold;
    protected float ExtraDamageTaken;
    protected float ResurrectionHpThreshold;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                HibernationHpThreshold = 0.5f;
                ExtraDamageTaken = 1;
                ResurrectionHpThreshold = 0.3f;
                break;

            case 4:
                HibernationHpThreshold = 1;
                ExtraDamageTaken = 1;
                ResurrectionHpThreshold = 0.5f;
                break;
        }
    }

    public override void Die()
    {
        if (!HasResurrected)
        {
            HasResurrected = true;
            IsInHibernation = true;
        }
        else
        {
            base.Die();
        }
    }
}