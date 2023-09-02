using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Mecha : BattleEntity, IRaceAura
{
    public new bool CanTakeTurn => !IsStun && !isHibernating;

    [TitleGroup("MECHA AURA:")]
    [SerializeField] protected bool hasAura;
    [SerializeField] protected bool hasResurrected;
    [SerializeField] protected bool isHibernating;
    [SerializeField] protected float hibernationHpThreshold;
    [SerializeField] protected float extraDamageTaken;
    [SerializeField] protected float resurrectionHpThreshold;

    public void ActiveRaceAura(int rank)
    {
        switch (rank)
        {
            case 3:
                hasAura = true;
                hibernationHpThreshold = 0.5f;
                extraDamageTaken = 1;
                resurrectionHpThreshold = 0.3f;
                break;

            case 4:
                hasAura = true;
                hibernationHpThreshold = 1;
                extraDamageTaken = 1;
                resurrectionHpThreshold = 0.5f;
                break;

            default:
                hasAura = false;
                break;
        }
    }

    public override float TakeDamage(IDamageDealer origin, Damage dmg)
    {
        if (!hasAura)
        {
            return base.TakeDamage(origin, dmg);
        }

        if (IsImmortal)
        {
            SpawnHpText(HealthImpactType.None, 0, 1, 0);
            return 0;
        }

        float dmgReduction = 0;
        switch (dmg.Type)
        {
            case DamageType.Physical:
                dmgReduction = Stats.armor * (1 - dmg.Penetration);
                break;
            case DamageType.Magical:
                dmgReduction = Stats.resistance * (1 - dmg.Penetration);
                break;
            case DamageType.Pure:
                dmgReduction = 0;
                break;
        }

        float dmgTaken = Mathf.Max(1, dmg.Amount - dmgReduction);
        if (isHibernating)
        {
            dmgTaken *= 2;
        }

        SpawnHpText(dmg.GetHealthImpactType(), dmgTaken, dmg.Division, dmg.Duration);

        // the displayed damage has no limit,
        // but the actual damage taken cant exceed the current hp
        float actualDmgTaken = Mathf.Min(dmgTaken, Hp + VirtualHp);
        float vhpAffected = Mathf.Min(actualDmgTaken, VirtualHp);
        float hpAffected = actualDmgTaken - vhpAffected;

        VirtualHp -= vhpAffected;
        Hp -= hpAffected;

        if (Hp < 1)
        {
            Die();
        }

        return actualDmgTaken;
    }

    public override void Die()
    {
        if (!hasAura)
        {
            base.Die();
            return;
        }

        if (!hasResurrected)
        {
            EditorLog.Message($"[Mecha] {name} activates hibernation!");
            hasResurrected = true;
            isHibernating = true;
            RegenHp(Stats.health * hibernationHpThreshold);
        }
        else
        {
            base.Die();
        }
    }
}