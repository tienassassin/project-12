public abstract class BeastEntity : BattleEntity, IRaceAura
{
    protected float ExtraLifeStealPerStack;
    protected float HpLossPerStack;
    protected float MaxExtraLifeSteal;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                ExtraLifeStealPerStack = 4;
                HpLossPerStack = 0.1f;
                MaxExtraLifeSteal = 20;
                break;

            case 4:
                ExtraLifeStealPerStack = 5;
                HpLossPerStack = 0.1f;
                MaxExtraLifeSteal = 35;
                break;
        }
    }
}