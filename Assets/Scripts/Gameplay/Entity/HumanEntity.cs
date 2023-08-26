public abstract class HumanEntity : BattleEntity, IRaceAura
{
    protected float InstantKillThreshold;
    protected float InstantKillMaxDamage;

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
}