using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Human : BattleEntity, IRaceAura
{
    [TitleGroup("HUMAN AURA:")]
    [SerializeField] protected float instantKillThreshold;
    [SerializeField] protected float instantKillMaxDamage;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                instantKillThreshold = 0.03f;
                instantKillMaxDamage = 2;
                break;

            case 4:
                instantKillThreshold = 0.05f;
                instantKillMaxDamage = 3;
                break;
        }
    }

    public override float DealDamage(IDamageTaker target, Damage dmg)
    {
        float dmgDealt = base.DealDamage(target, dmg);
        if (target is BattleEntity entity)
        {
            if (entity.HpPercentage - 0.001f <= instantKillThreshold &&
                entity.Hp <= Stats.damage * instantKillMaxDamage)
            {
                target.TakeFatalDamage(this);
            }
        }

        return dmgDealt;
    }
}