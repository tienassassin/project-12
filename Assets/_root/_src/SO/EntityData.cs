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
    public EntityInfo info;
    [InfoBox("Assets of entity")]
    public EntityAsset asset;
}

[Serializable]
public struct EntityInfo
{
    public string id;
    public string name;
    public string alias;
    public string story;
    public Tier tier;
    public Element element;
    public Race race;
    public DamageType damageType;
    public AttackRange attackRange;
    public Stats stats;
}

[Serializable]
public struct EntityAsset
{
    public Sprite avatar;
    public Sprite splashArt;
    public Sprite banner;
    public SkeletonDataAsset lobbySkeleton;
    public SkeletonDataAsset battleSkeleton;
}

[Serializable]
public struct Stats
{
    public float health;
    public float damage;
    public float armor;
    public float resistance;

    public float intelligence;
    public float speed;
    public float luck;
    public float critDamage;

    public float lifeSteal;
    public float accuracy;

    public Stats GetStatsByLevel(int level, float growth)
    {
        return new Stats
        {
            health = health * Mathf.Pow(1 + growth, level - 1),
            damage = damage * Mathf.Pow(1 + growth, level - 1),
            armor = armor * Mathf.Pow(1 + growth, level - 1),
            resistance = resistance * Mathf.Pow(1 + growth, level - 1),

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
            health = st1.health * rate,
            damage = st1.damage * rate,
            armor = st1.armor * rate,
            resistance = st1.resistance * rate,

            intelligence = Clamp(st1.intelligence * rate, 100),
            speed = Clamp(st1.speed * rate, 100),
            luck = Clamp(st1.luck * rate, 100),
            critDamage = st1.critDamage * rate,

            lifeSteal = st1.lifeSteal * rate,
            accuracy = Clamp(st1.accuracy * rate, 80)
        };
    }

    private static float Clamp(float value, float limit)
    {
        return Mathf.Min(value, limit);
    }
}

public static partial class DataExtensions
{
    public static float GetEntityGrowth(this EntityInfo @this)
    {
        return DataManager.Instance.GetGrowth(@this.race);
    }
}