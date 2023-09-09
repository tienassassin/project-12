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
    [SerializeField] private float hp;
    [SerializeField] private float virtualHp;
    [SerializeField] private float energy;
    [SerializeField] private float agility;
    [SerializeField] private float rage;

    [TitleGroup("STATUS:")]
    [SerializeField] private bool isStun;
    [SerializeField] private bool isSilent;
    [SerializeField] private bool isBleeding;
    [SerializeField] private bool isImmortal;
    [SerializeField] private bool isUltimateReady;

    [TitleGroup("OTHERS:")]
    [SerializeField] private SkillTargetType skillTargetType;
    [SerializeField] private SkillTargetType ultimateTargetType;

    private EntityUI _entityUI;
    private EntityReferenceHolder _ref;
    private EntityConfig _config;


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
    public float HpPercentage => hp / Stats.health;
    public float Hp
    {
        get => hp;
        protected set => hp = value;
    }
    public float VirtualHp
    {
        get => virtualHp;
        protected set => virtualHp = value;
    }
    public float Energy
    {
        get => energy;
        protected set => energy = value;
    }
    public float Agility
    {
        get => agility;
        protected set => agility = value;
    }
    public float Rage
    {
        get => rage;
        protected set => rage = value;
    }
    public bool IsStun
    {
        get => isStun;
        protected set => isStun = value;
    }
    public bool IsSilent
    {
        get => isSilent;
        protected set => isSilent = value;
    }
    public bool IsBleeding
    {
        get => isBleeding;
        protected set => isBleeding = value;
    }
    public bool IsImmortal
    {
        get => isImmortal;
        protected set => isImmortal = value;
    }
    public bool IsUltimateReady
    {
        get => isUltimateReady;
        protected set => isUltimateReady = value;
    }
    public bool CanTakeTurn => !isStun;

    public SkillTargetType SkillTargetType => skillTargetType;
    public SkillTargetType UltimateTargetType => ultimateTargetType;

    #endregion

    private void Awake()
    {
        _entityUI = GetComponent<EntityUI>();
        _ref = GetComponent<EntityReferenceHolder>();
        _config = GetComponent<EntityConfig>();
    }

    public void Init(EntitySaveData entitySaveData)
    {
        side = Side.Ally;
        entityData = entitySaveData.GetEntity();
        level = entitySaveData.GetLevel();
        SetupInfo();

        this.PostEvent(EventID.ON_HEROES_SPAWNED, this);

        Hp = (entitySaveData.currentHp / 100) * Stats.health;
        Energy = entitySaveData.energy;
        SetupStats();
    }

    public void Init(EnemyData enemyData)
    {
        side = Side.Enemy;
        entityData = enemyData.GetEntity();
        level = enemyData.level;
        SetupInfo();

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

        IsStun = false;
        IsSilent = false;
        IsBleeding = false;

        SetupHpSegment();
        UpdateHp();
        UpdateEnergy();
    }

    public Vector3 GetHitPosition()
    {
        return (Side == Side.Ally ? _ref.rightHitPos : _ref.leftHitPos).position;
    }

    #region UI

    protected virtual void SetupHpSegment()
    {
        _entityUI.SetupHpSegment(stats.health);
    }

    protected virtual void UpdateHp(float duration = 1f)
    {
        _entityUI.UpdateHp(Hp, VirtualHp, Stats.health, duration);
    }

    protected virtual void UpdateEnergy(float duration = 1f)
    {
        IsUltimateReady = Energy >= 100;
        this.PostEvent(EventID.ON_ENERGY_UPDATED, Tuple.Create(UniqueID, IsUltimateReady));

        _entityUI.UpdateEnergy(Energy, 100, duration);
    }

    protected void SpawnHpText(HealthImpactType impactType, float amount, int division, float duration)
    {
        if (amount < 1)
        {
            string dmgTxt = (impactType.IsNull() ? "immortal" : "+0");
            var o = ObjectPool.Instance.SpawnObject<HpText>(_ref.hpTextPrefab, _ref.hpTextPos.position);
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
                var o = ObjectPool.Instance.SpawnObject<HpText>(_ref.hpTextPrefab, _ref.hpTextPos.position);
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

        // RegenEnergy(Stats.intelligence);
        //
        // var dmg = new Damage(Stats.damage, DamageType, Stats.accuracy / 100, crit);
        // float dmgDealt = DealDamage(target, dmg);
        //
        // RegenHp(dmgDealt * (Stats.lifeSteal / 100));

        float dmgDealt = 0;
        float pureDmg = Stats.damage * (crit ? Stats.critDamage / 100f : 1f);
        var dmg = new Damage(pureDmg, DamageType, Stats.accuracy / 100, crit);

        if (entityData.info.attackRange == AttackRange.Melee)
        {
            PlayMeleeAnimation(target.GetHitPosition(), Hit, Regen, Finish);
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
            RegenHp(dmgDealt * (Stats.lifeSteal / 100));
        }

        void Finish()
        {
            finished?.Invoke();
        }
    }

    protected virtual void PlayMeleeAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        var origin = transform.position;
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(hitPos, _config.meleeMoveTime))
            .AppendInterval(_config.meleeHitTime)
            .AppendCallback(() => { hitPhase?.Invoke(); })
            .Append(transform.DOMove(origin, _config.meleeReturnTime))
            .AppendCallback(() => { regenPhase?.Invoke(); })
            .AppendInterval(_config.restTime)
            .AppendCallback(() => { finishPhase?.Invoke(); });
    }

    protected virtual void PlayRangedAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        var seq = DOTween.Sequence();
        seq.AppendCallback(() =>
            {
                var o = ObjectPool.Instance.SpawnObject<Projectile>(_ref.projectile, transform.position);
                o.Move(hitPos, _config.rangedMoveTime, () =>
                {
                    hitPhase?.Invoke();
                    regenPhase?.Invoke();
                });
            })
            .AppendInterval(_config.rangedMoveTime + _config.restTime)
            .AppendCallback(() => { finishPhase?.Invoke(); });
    }

    public virtual void UseSkill(IDamageTaker target)
    {
    }

    public virtual void UseUltimate(IDamageTaker target)
    {
        LoseEnergy(100);
    }

    public virtual void RegenHp(float hpAmount, bool allowOverflow = false)
    {
        float expectedHp = Hp + hpAmount;
        if (expectedHp > Stats.health && allowOverflow)
        {
            float overflowAmount = expectedHp - Stats.health;
            VirtualHp += overflowAmount;
        }

        Hp = Mathf.Min(expectedHp, Stats.health);
        UpdateHp();
    }

    [Button]
    public virtual void RegenEnergy(float amount)
    {
        if (Energy >= 100) return;

        Energy += amount;
        UpdateEnergy();
    }

    public virtual void LoseEnergy(float amount)
    {
        Energy = Mathf.Max(Energy - amount, 0);
        UpdateEnergy();
    }

    public virtual float DealDamage(IDamageTaker target, Damage dmg)
    {
        return target.TakeDamage(this, dmg);
    }

    public virtual float TakeDamage(IDamageDealer origin, Damage dmg)
    {
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

        // the displayed damage has no limit,
        // but the actual damage taken cant exceed the current hp
        float actualDmgTaken = Mathf.Min(dmgTaken, Hp + VirtualHp);
        float vhpAffected = Mathf.Min(actualDmgTaken, VirtualHp);
        float hpAffected = actualDmgTaken - vhpAffected;

        VirtualHp -= vhpAffected;
        Hp -= hpAffected;

        SpawnHpText(dmg.GetHealthImpactType(), dmgTaken, dmg.Division, dmg.Duration);
        UpdateHp(dmg.Duration);

        if (Hp < 1)
        {
            Die();
        }

        return actualDmgTaken;
    }

    public virtual float TakeFatalDamage(IDamageDealer origin)
    {
        if (IsImmortal)
        {
            SpawnHpText(HealthImpactType.None, 0, 1, 0);
            return 0;
        }

        float fatalDamage = 999;
        float actualDmgTaken = Hp + VirtualHp;
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
        gameObject.SetActive(false);
        ActionQueue.Instance.RemoveEntity(UniqueID);
        EntityManager.Instance.OnEntityDead(Side);
    }

    #endregion
}

public struct Damage
{
    public float Amount;
    public DamageType Type;
    public float Penetration;
    public bool IsCritical;
    public int Division;
    public float Duration;

    public Damage(float amount, DamageType type, float penetration,
        bool isCritical = false, int division = 1, float duration = 0.5f)
    {
        Amount = amount;
        Type = type;
        Penetration = penetration;
        IsCritical = isCritical;
        Division = division;
        Duration = duration;
    }

    public HealthImpactType GetHealthImpactType()
    {
        return Type switch
        {
            DamageType.Pure when IsCritical => HealthImpactType.CriticalPureDamage,
            DamageType.Pure => HealthImpactType.PureDamage,
            DamageType.Physical when IsCritical => HealthImpactType.CriticalPhysicalDamage,
            DamageType.Physical => HealthImpactType.PhysicalDamage,
            DamageType.Magical when IsCritical => HealthImpactType.CriticalMagicalDamage,
            DamageType.Magical => HealthImpactType.MagicalDamage,
            _ => HealthImpactType.None
        };
    }
}