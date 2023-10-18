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
public struct EntityPrice
{
    public int gold;
    public int diamond;
    public int shard;
}



public static partial class DataExtensions
{
    public static float GetEntityGrowth(this EntityInfo @this)
    {
        return GameDatabase.Instance.GetGrowth(@this.realm);
    }
}