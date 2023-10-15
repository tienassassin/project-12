using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityLibrary", menuName = "AssetLibrary/EntityLibrary")]
public class EntityLibrary : ScriptableObject
{
    public List<EntityAsset> entityAssets;
}

[Serializable]
public struct EntityAsset
{
    public string id;

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