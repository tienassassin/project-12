using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Beast : BattleEntity, IRaceAura
{
    [TitleGroup("BEAST AURA:")]
    [SerializeField] protected bool hasAura;
    [SerializeField] protected float extraLifeStealPerStack;
    [SerializeField] protected float hpLossPerStack;
    [SerializeField] protected int maxStack;
    [SerializeField] protected int hungryStack;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                hasAura = true;
                extraLifeStealPerStack = 4;
                hpLossPerStack = 0.1f;
                maxStack = 5;
                break;

            case 4:
                hasAura = true;
                extraLifeStealPerStack = 5;
                hpLossPerStack = 0.1f;
                maxStack = 7;
                break;

            default:
                hasAura = false;
                break;
        }
    }

    [Button]
    protected void UpdateHungryStack()
    {
        int lastStack = hungryStack;
        hungryStack = Mathf.Min((int)((1 - HpPercentage + 0.001f) / hpLossPerStack), maxStack);
        Stats += new Stats { lifeSteal = (hungryStack - lastStack) * extraLifeStealPerStack };
    }
}