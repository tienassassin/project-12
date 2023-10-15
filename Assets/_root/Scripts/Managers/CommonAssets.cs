using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class CommonAssets : Singleton<CommonAssets>
{
    [SerializeField] private List<EntityAsset> entityAssets;
    [SerializeField] [TableList] private List<ItemAsset> itemAssets;
    [SerializeField] private AvatarLibrary avatarLibrary;

    public EntityAsset GetEntityAsset(string id)
    {
        return entityAssets.Find(x => x.id.Equals(id));
    }

    public ItemAsset GetItemAsset(string id)
    {
        return itemAssets.Find(x => x.id.Equals(id));
    }

    public AvatarAsset GetAvatar(string id)
    {
        return avatarLibrary.avatarAssets.Find(x => x.id.Equals(id));
    }

    public AvatarFrameAsset GetAvatarFrame(string id)
    {
        return avatarLibrary.avatarFrameAssets.Find(x => x.id.Equals(id));
    }
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

[Serializable]
public struct ItemAsset
{
    public string id;
    [PreviewField(ObjectFieldAlignment.Center)]
    public Sprite icon;
}