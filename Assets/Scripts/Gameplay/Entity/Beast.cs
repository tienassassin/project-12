using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Beast : BattleEntity, IRaceAura
{
    [TitleGroup("BEAST AURA:")]
    [ShowInInspector] protected float ExtraLifeStealPerStack;
    [ShowInInspector] protected float HpLossPerStack;
    [ShowInInspector] protected int MaxStack;
    [ShowInInspector] protected int HungryStack;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                ExtraLifeStealPerStack = 4;
                HpLossPerStack = 0.1f;
                MaxStack = 5;
                break;

            case 4:
                ExtraLifeStealPerStack = 5;
                HpLossPerStack = 0.1f;
                MaxStack = 7;
                break;
        }
    }

    [Button]
    protected void UpdateHungryStack()
    {
        int lastStack = HungryStack;
        HungryStack = Mathf.Min((int)((1 - HpPercentage + 0.001f) / HpLossPerStack), MaxStack);
        Stats += new Stats { lifeSteal = (HungryStack - lastStack) * ExtraLifeStealPerStack };
    }
}