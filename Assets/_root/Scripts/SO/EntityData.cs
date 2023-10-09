using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "DB/EntityData")]
public class EntityData : ScriptableObject
{
    [InfoBox("Some entities cannot be unlocked. (campaign enemies...)")]
    public bool canUnlock;
    
    [InfoBox("Basic information of entity")]
    [FoldoutGroup("Info")]
    [HideLabel]
    public EntityInfo info;
    
    [InfoBox("Assets of entity")]
    [FoldoutGroup("Asset")]
    [HideLabel]
    public EntityAsset asset;

    [InfoBox("Prices of entity")]
    [FoldoutGroup("Price")]
    [HideLabel]
    public EntityPrice price;
}

[Serializable]
public struct EntityInfo
{
    public string id;
    public string name;
    public string alias;
    public string story;
    public Tier tier;
    public Role role;
    public Realm realm;
    public DamageType damageType;
    public AttackRange attackRange;

    [FoldoutGroup("Stats")]
    [HideLabel]
    public Stats stats;
}

[Serializable]
public struct EntityAsset
{
    [PreviewField(ObjectFieldAlignment.Left)]
    public Sprite avatar;

    public Sprite splashArt;
    public Sprite banner;
    public SkeletonDataAsset lobbySkeleton;
    public SkeletonDataAsset battleSkeleton;

    [FoldoutGroup("Skill")]
    [HorizontalGroup("Skill/SkillIcon")]
    [PreviewField(ObjectFieldAlignment.Left)]
    [LabelText("Passive:")]
    public Sprite passiveIcon;

    [FoldoutGroup("Skill")]
    [HorizontalGroup("Skill/SkillIcon")]
    [PreviewField(ObjectFieldAlignment.Left)]
    [LabelText("Skill:")]
    public Sprite skillIcon;

    [FoldoutGroup("Skill")]
    [HorizontalGroup("Skill/SkillIcon")]
    [PreviewField(ObjectFieldAlignment.Left)]
    [LabelText("Ultimate:")]
    public Sprite ultimateIcon;
}

[Serializable]
public struct EntityPrice
{
    public int gold;
    public int diamond;
    public int shard;
}

[Serializable]
public struct Stats
{
    public int health;
    public int damage;
    public int armor;
    public int resistance;

    public int intelligence;
    public int speed;
    public int luck;
    public int critDamage;

    public int lifeSteal;
    public int accuracy;

    public Stats GetStatsByLevel(int level, float growth)
    {
        return new Stats
        {
            health = (int)(health * Mathf.Pow(1 + growth, level - 1)),
            damage = (int)(damage * Mathf.Pow(1 + growth, level - 1)),
            armor = (int)(armor * Mathf.Pow(1 + growth, level - 1)),
            resistance = (int)(resistance * Mathf.Pow(1 + growth, level - 1)),

            intelligence = intelligence,
            speed = speed,
            luck = luck,
            critDamage = critDamage,

            lifeSteal = lifeSteal,
            accuracy = accuracy
        };
    }

    public static Stats operator +(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health + st2.health,
            damage = st1.damage + st2.damage,
            armor = st1.armor + st2.armor,
            resistance = st1.resistance + st2.resistance,

            intelligence = Clamp(st1.intelligence + st2.intelligence, 100),
            speed = Clamp(st1.speed + st2.speed, 100),
            luck = Clamp(st1.luck + st2.luck, 100),
            critDamage = st1.critDamage + st2.critDamage,

            lifeSteal = st1.lifeSteal + st2.lifeSteal,
            accuracy = Clamp(st1.accuracy + st2.accuracy, 80)
        };
    }

    public static Stats operator -(Stats st1, Stats st2)
    {
        return new Stats
        {
            health = st1.health - st2.health,
            damage = st1.damage - st2.damage,
            armor = st1.armor - st2.armor,
            resistance = st1.resistance - st2.resistance,

            intelligence = st1.intelligence - st2.intelligence,
            speed = st1.speed - st2.speed,
            luck = st1.luck - st2.luck,
            critDamage = st1.critDamage - st2.critDamage,

            lifeSteal = st1.lifeSteal - st2.lifeSteal,
            accuracy = st1.accuracy - st2.accuracy
        };
    }

    public static Stats operator *(Stats st1, float rate)
    {
        return new Stats
        {
            health = (int)(st1.health * rate),
            damage = (int)(st1.damage * rate),
            armor = (int)(st1.armor * rate),
            resistance = (int)(st1.resistance * rate),

            intelligence = Clamp((int)(st1.intelligence * rate), 100),
            speed = Clamp((int)(st1.speed * rate), 100),
            luck = Clamp((int)(st1.luck * rate), 100),
            critDamage = (int)(st1.critDamage * rate),

            lifeSteal = (int)(st1.lifeSteal * rate),
            accuracy = Clamp((int)(st1.accuracy * rate), 80)
        };
    }

    private static int Clamp(int value, int limit)
    {
        return Mathf.Min(value, limit);
    }
}

public static partial class DataExtensions
{
    public static float GetEntityGrowth(this EntityInfo @this)
    {
        return GameDatabase.Instance.GetGrowth(@this.realm);
    }
}