using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class BattleEntity : DuztineBehaviour, IDamageDealer, IDamageTaker
{
    private static int _autogeneratedId;

    [TitleGroup("BASE DATA:")]
    [SerializeField] private int uniqueID;
    [SerializeField] protected EntityData entityData;
    [SerializeField] private int level;
    [SerializeField] private Side side;
    // [SerializeField] protected List<Equipment> eqmList = new();

    [TitleGroup("IN-GAME STATS:")]
    [SerializeField] private Stats stats;

    [SerializeField] private int hp;
    [SerializeField] private int virtualHp;
    [SerializeField] private int energy;
    [SerializeField] private int agility;
    [SerializeField] private int rage;

    [TitleGroup("STATUS:")] [SerializeField]
    private int stun;

    [SerializeField] private int silent;
    [SerializeField] private int bleeding;
    [SerializeField] private int immortal;

    [TitleGroup("OTHERS:")]
    [SerializeField] private SkillTargetType skillTargetType;
    [SerializeField] private SkillTargetType ultimateTargetType;

    protected EntityUI entityUI;
    protected EntityReferenceHolder entityRef;
    protected EntityConfig entityConfig;
    protected EntityAnimator entityAnim;


    #region Public properties

    /// <summary>
    ///     This ID is <b>UNIQUE</b>.
    ///     Used to distinguish entities in the same battle from each other.
    /// </summary>
    public int UniqueID => uniqueID;
    /// <summary>
    ///     This ID is not unique.
    ///     2 entities can have the same EntityID if they have the same baseData.
    ///     Used to retrieve data belonging to that entity (images, sounds...)
    /// </summary>
    public string EntityID => entityData.info.id;
    public Side Side => side;
    public Stats Stats
    {
        get => stats;
        protected set => stats = value;
    }
    public DamageType DamageType => entityData.info.damageType;
    public Element Element => entityData.info.element;
    public Race Race => entityData.info.race;
    public bool IsAlive => hp > 0;
    public float HpPercentage => (float)hp / Stats.health;

    public int Hp
    {
        get => hp;
        protected set => hp = value;
    }

    public int VirtualHp
    {
        get => virtualHp;
        protected set => virtualHp = value;
    }

    public int Energy
    {
        get => energy;
        protected set => energy = value;
    }

    public int Agility
    {
        get => agility;
        protected set => agility = value;
    }

    public int Rage
    {
        get => rage;
        protected set => rage = value;
    }

    public bool IsStun => stun > 0;
    public bool IsSilent => silent > 0;
    public bool IsBleeding => bleeding > 0;
    public bool IsImmortal => immortal > 0;
    public bool IsUltimateReady => Energy >= 100;
    public bool CanTakeTurn => !IsStun;

    public SkillTargetType SkillTargetType => skillTargetType;
    public SkillTargetType UltimateTargetType => ultimateTargetType;

    #endregion

    private void Awake()
    {
        entityUI = GetComponent<EntityUI>();
        entityRef = GetComponent<EntityReferenceHolder>();
        entityConfig = GetComponent<EntityConfig>();
        entityAnim = GetComponent<EntityAnimator>();
    }

    public void Init(EntitySaveData entitySaveData)
    {
        side = Side.Ally;
        entityData = entitySaveData.GetEntity();
        level = entitySaveData.GetLevel();
        SetupInfo();

        this.PostEvent(EventID.ON_HEROES_SPAWNED, this);

        Hp = (int)(entitySaveData.currentHp.Percent() * Stats.health);
        Energy = entitySaveData.energy;
        SetupStats();
    }

    public void Init(EnemyData enemyData)
    {
        side = Side.Enemy;
        entityData = enemyData.GetEntity();
        level = enemyData.level;
        SetupInfo();

        entityAnim.Flip();

        Hp = Stats.health;
        Energy = 0;
        SetupStats();
    }

    private void SetupInfo()
    {
        uniqueID = _autogeneratedId;
        _autogeneratedId++;
        Stats = entityData.info.stats; // base stats
        Stats = Stats.GetStatsByLevel(level, entityData.info.GetEntityGrowth()); // level-based level
        // Stats = Stats; // todo: overall stats
        name = entityData.name;
    }

    private void SetupStats()
    {
        VirtualHp = 0;
        Agility = 0;
        Rage = 0;

        stun = 0;
        silent = 0;
        bleeding = 0;
        immortal = 0;

        SetupHpSegment();
        UpdateHp();
        UpdateEnergy();
    }

    public Vector3 GetRootPosition()
    {
        return entityRef.rootPos.position;
    }

    public Vector3 GetHitPosition()
    {
        return entityRef.hitPos.position;
    }

    #region UI

    protected virtual void SetupHpSegment()
    {
        entityUI.SetupHpSegment(stats.health);
    }

    protected virtual void UpdateHp(float duration = 1f)
    {
        entityUI.UpdateHp(Hp, VirtualHp, Stats.health, duration);
    }

    protected virtual void UpdateEnergy(float duration = 1f)
    {
        this.PostEvent(EventID.ON_ENERGY_UPDATED, Tuple.Create(UniqueID, IsUltimateReady));

        entityUI.UpdateEnergy(Energy, 100, duration);
    }

    protected void SpawnHpText(HealthImpactType impactType, int amount, int division, float duration)
    {
        if (amount < 1)
        {
            string dmgTxt = (impactType.IsNull() ? "immortal" : "+0");
            var o = ObjectPool.Instance.SpawnObject<HpText>(entityRef.hpTextPrefab, entityRef.hpTextPos.position);
            o.Init(impactType, dmgTxt);
            EditorLog.Message(name + dmgTxt);
        }
        else
        {
            if (division < 1) division = 1;
            int amountPerHit = (int)(amount / division);
            float interval = duration / division;

            for (int i = 0; i < division; i++)
            {
                string dmgTxt = (impactType.IsHealing() ? "+" : "-") + amountPerHit;
                var o = ObjectPool.Instance.SpawnObject<HpText>(entityRef.hpTextPrefab, entityRef.hpTextPos.position);
                o.Init(impactType, dmgTxt);
                EditorLog.Message(name + dmgTxt);
            }
        }
    }

    #endregion


    #region Resources consumption

    protected virtual bool HasFullEnergy()
    {
        return Energy >= 100;
    }

    protected virtual bool HasFullAgility()
    {
        return Agility >= 100;
    }

    #endregion


    #region Actions

    public virtual void Attack(IDamageTaker target, Action finished)
    {
        Rage += Stats.luck;
        bool crit = Common.GetRandomResult(Rage);
        if (crit)
        {
            Rage = Mathf.Max(0, Rage - 100);
        }

        var dmgDealt = 0;
        var pureDmg = Stats.damage.GetValue(crit ? Stats.critDamage : 100);
        var dmg = new Damage(pureDmg, DamageType, Stats.accuracy.Percent(), crit);

        if (entityData.info.attackRange == AttackRange.Melee)
        {
            PlayMeleeAnimation(target.GetRootPosition(), Hit, Regen, Finish);
        }
        else
        {
            PlayRangedAnimation(target.GetHitPosition(), Hit, Regen, Finish);
        }

        void Hit()
        {
            dmgDealt = DealDamage(target, dmg);
        }

        void Regen()
        {
            RegenEnergy(Stats.intelligence);
            RegenHp((int)(dmgDealt * Stats.lifeSteal.Percent()));
        }

        void Finish()
        {
            finished?.Invoke();
        }
    }

    protected virtual void PlayMeleeAnimation(Vector3 rootPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        var origin = transform.position;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(rootPos, entityConfig.meleeMoveTime))
            .AppendCallback(() => entityAnim.PlayAnimation(AnimationState.Attack))
            .AppendInterval(entityConfig.meleeHitTime)
            .AppendCallback(() => { hitPhase?.Invoke(); })
            .Append(transform.DOMove(origin, entityConfig.meleeReturnTime))
            .AppendCallback(() => { regenPhase?.Invoke(); })
            .AppendInterval(entityConfig.restTime)
            .AppendCallback(() => { finishPhase?.Invoke(); });
    }

    protected virtual void PlayRangedAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() => entityAnim.PlayAnimation(AnimationState.Attack))
            .AppendCallback(() =>
            {
                var o = ObjectPool.Instance.SpawnObject<Projectile>(entityRef.projectile, GetHitPosition());
                o.Move(hitPos, entityConfig.rangedMoveTime, () =>
                {
                    hitPhase?.Invoke();
                    regenPhase?.Invoke();
                });
            })
            .AppendInterval(entityConfig.rangedMoveTime + entityConfig.restTime)
            .AppendCallback(() => { finishPhase?.Invoke(); });
    }

    public virtual void UseSkill(IDamageTaker target)
    {
    }

    public virtual void UseUltimate(IDamageTaker target)
    {
        LoseEnergy(100);
    }

    public virtual void RegenHp(int hpAmount, bool allowOverflow = false)
    {
        var expectedHp = Hp + hpAmount;
        if (expectedHp > Stats.health && allowOverflow)
        {
            var overflowAmount = expectedHp - Stats.health;
            VirtualHp += overflowAmount;
        }

        Hp = Mathf.Min(expectedHp, Stats.health);
        UpdateHp();
    }

    [Button]
    public virtual void RegenEnergy(int amount)
    {
        if (Energy >= 100) return;

        Energy += amount;
        UpdateEnergy();
    }

    public virtual void LoseEnergy(int amount)
    {
        Energy = Mathf.Max(Energy - amount, 0);
        UpdateEnergy();
    }

    public virtual int DealDamage(IDamageTaker target, Damage dmg)
    {
        return target.TakeDamage(this, dmg);
    }

    public virtual int TakeDamage(IDamageDealer origin, Damage dmg)
    {
        if (IsImmortal)
        {
            SpawnHpText(HealthImpactType.None, 0, 1, 0);
            return 0;
        }

        entityAnim.PlayAnimation(AnimationState.Hit);

        var dmgReduction = 0;
        switch (dmg.type)
        {
            case DamageType.Physical:
                dmgReduction = Stats.armor.GetValue(1 - dmg.penetration);
                break;
            case DamageType.Magical:
                dmgReduction = Stats.resistance.GetValue(1 - dmg.penetration);
                break;
            case DamageType.Pure:
                dmgReduction = 0;
                break;
        }

        var dmgTaken = Mathf.Max(1, dmg.amount - dmgReduction);

        // the displayed damage has no limit,
        // but the actual damage taken cant exceed the current hp
        var actualDmgTaken = Mathf.Min(dmgTaken, Hp + VirtualHp);
        var vhpAffected = Mathf.Min(actualDmgTaken, VirtualHp);
        var hpAffected = actualDmgTaken - vhpAffected;

        VirtualHp -= vhpAffected;
        Hp -= hpAffected;

        SpawnHpText(dmg.GetHealthImpactType(), dmgTaken, dmg.division, dmg.duration);
        UpdateHp(dmg.duration);

        if (!IsAlive)
        {
            Die();
        }

        return actualDmgTaken;
    }

    public virtual int TakeFatalDamage(IDamageDealer origin)
    {
        if (IsImmortal)
        {
            SpawnHpText(HealthImpactType.None, 0, 1, 0);
            return 0;
        }

        var fatalDamage = 999;
        var actualDmgTaken = Hp + VirtualHp;
        while (fatalDamage < actualDmgTaken)
        {
            fatalDamage = fatalDamage * 10 + 9;
        }

        VirtualHp = 0;
        Hp = 0;

        SpawnHpText(HealthImpactType.CriticalPureDamage, fatalDamage, 1, 0);
        UpdateHp();
        Die();

        return actualDmgTaken;
    }

    public virtual void Die()
    {
        EditorLog.Message($"<color=red>{name} dead</color>");

        entityAnim.PlayAnimation(AnimationState.Die);
        ActionQueue.Instance.RemoveEntity(UniqueID);
        EntityManager.Instance.OnEntityDead(Side);
    }

    #endregion
}

public struct Damage
{
    public int amount;
    public DamageType type;
    public float penetration;
    public bool isCritical;
    public int division;
    public float duration;

    public Damage(int amount, DamageType type, float penetration,
        bool isCritical = false, int division = 1, float duration = 0.5f)
    {
        this.amount = amount;
        this.type = type;
        this.penetration = penetration;
        this.isCritical = isCritical;
        this.division = division;
        this.duration = duration;
    }

    public HealthImpactType GetHealthImpactType()
    {
        return type switch
        {
            DamageType.Pure when isCritical => HealthImpactType.CriticalPureDamage,
            DamageType.Pure => HealthImpactType.PureDamage,
            DamageType.Physical when isCritical => HealthImpactType.CriticalPhysicalDamage,
            DamageType.Physical => HealthImpactType.PhysicalDamage,
            DamageType.Magical when isCritical => HealthImpactType.CriticalMagicalDamage,
            DamageType.Magical => HealthImpactType.MagicalDamage,
            _ => HealthImpactType.None
        };
    }
}