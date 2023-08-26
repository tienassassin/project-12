using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Mecha : BattleEntity, IRaceAura
{
    public bool CanTakeTurn => !IsStun && !IsHibernating;

    [TitleGroup("MECHA AURA:")]
    [ShowInInspector] protected bool HasResurrected;
    [ShowInInspector] protected bool IsHibernating;
    [ShowInInspector] protected float HibernationHpThreshold;
    [ShowInInspector] protected float ExtraDamageTaken;
    [ShowInInspector] protected float ResurrectionHpThreshold;

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

    public override float TakeDamage(IDamageDealer origin, Damage dmg)
    {
        if (IsImmortal)
        {
            SpawnHpText(false, 0, 1, 0);
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
        if (IsHibernating)
        {
            dmgTaken *= 2;
        }

        SpawnHpText(false, dmgTaken, dmg.Division, dmg.Duration);

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
        if (!HasResurrected)
        {
            EditorLog.Message($"[Mecha] {name} activates hibernation!");
            HasResurrected = true;
            IsHibernating = true;
            RegenHp(Stats.health * HibernationHpThreshold);
        }
        else
        {
            base.Die();
        }
    }
}