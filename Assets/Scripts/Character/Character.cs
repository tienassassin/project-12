using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : MonoBehaviour, IAttacker, IDefender
{
    [TitleGroup("BASE DATA:")]
    [SerializeField] protected CharacterData charData;
    
    [TitleGroup("DATA")]
    [ShowInInspector] protected Element element;
    [ShowInInspector] protected Faction faction;
    [ShowInInspector] protected Stats stats;

    [TitleGroup("IN-GAME STATS")]
    [ShowInInspector] protected float curHP;
    [ShowInInspector] protected float virtualHP;
    [ShowInInspector] protected float energy;
    [ShowInInspector] protected float agility;
    [ShowInInspector] protected float anger;
    
    [TitleGroup("DEBUFF STATUS")]
    [ShowInInspector] protected bool hasStun;
    [ShowInInspector] protected bool hasSilent;
    [ShowInInspector] protected bool hasBleeding;

    [ShowInInspector] private bool isUltimateReady = false;
    
    #region Public properties

    public Stats Stats => stats;

    public bool IsAlive => curHP > 0;
    public bool CanTakeTurn => !hasStun;
    public bool CanUseSkill => !hasSilent;
    public bool HasHeal => !hasBleeding;
    
    public DamageType DamageType
    {
        get
        {
            switch (element)
            {
                case Element.Fire:
                case Element.Thunder:
                    return DamageType.Magical;

                case Element.Wind:
                case Element.Ice:
                    return DamageType.Physical;
            }

            return 0;
        }
    }

    public bool IsUltimateReady
    {
        get => isUltimateReady;
        protected set
        {
            isUltimateReady = value;
            if (value)
            {
                // todo: enable ultimate skill
            }
            else
            {
                // todo: disable ultimate skill
            }
        }
    }

    #endregion

    #region Callbacks
    
    public Action<float,float,float> OnUpdateHp;
    public Action<float> OnUpdateAnger;
    
    #endregion
    
    private void Awake()
    {
        name = charData.baseData.characterName;
        
        Initialize();
        OnRegistration();
    }

    private void Initialize()
    {
        element = charData.baseData.element;
        faction = charData.baseData.faction;
        stats = charData.baseData.stats;
        
        charData.equipmentList.ForEach(e=>
        {
            stats += e.GetFullStats(faction);
        });
        
        curHP = stats.health;
        virtualHP = 0;
        energy = 0;
        
        agility = 0;
        anger = 0;

        hasStun = false;
        hasSilent = false;
        hasBleeding = false;
    }

    private void Start()
    {
        OnStart();
        UpdateHp();
        UpdateAnger();
    }

    protected virtual void OnRegistration() { }

    protected virtual void OnStart() { }

    #region Consume Energy/Agility/...
    
    protected bool HasFullEnergy()
    {
        if (energy < 100) return false;
        energy -= 100;
        return true;
    }
    
    protected bool HasFullAgility()
    {
        if (agility < 100) return false;
        agility -= 100;
        return true;
    }

    #endregion

    #region Update UI

    protected void UpdateHp()
    {
        OnUpdateHp?.Invoke(curHP, virtualHP, stats.health);
    }

    protected void UpdateAnger()
    {
        OnUpdateAnger?.Invoke(anger);
    }

    #endregion

    public virtual void Attack(Character target) { }

    public virtual void DealDamage(IDefender target, float dmgAmount, DamageType dmgType) { }

    public virtual void TakeDamage(float dmgAmount, DamageType dmgType, float penetration) { }

    public void RegenHP(float hpAmount, bool allowOverflow = false)
    {
        float expectedHP = curHP + hpAmount;
        if (expectedHP > stats.health && allowOverflow)
        {
            float overflowAmount = expectedHP - stats.health;
            virtualHP += overflowAmount;
        }

        curHP = Mathf.Min(expectedHP, stats.health);
    }

    protected virtual void Die()
    {
        Debug.Log($"{name} dead");
    }

    private string json;

    [Button]
    public void Export()
    {
        json = JsonUtility.ToJson(charData);
    }

    [Button]
    public void Import()
    {
        charData = JsonUtility.FromJson<CharacterData>(json);
    }
}
