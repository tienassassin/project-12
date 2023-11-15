using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityLibrary", menuName = "AssetLibrary/EntityLibrary")]
public class EntityLibrary : ScriptableObject
{
    public List<EntityAsset> entityAssets;

    [Button]
    public void CheckDuplicate()
    {
        bool hasDuplicate = false;
        for (int i = 0; i < entityAssets.Count; i++)
        {
            for (int j = i + 1; j < entityAssets.Count; j++)
            {
                if (entityAssets[i].battleSkeleton.name == entityAssets[j].battleSkeleton.name)
                {
                    DebugLog.Message($"Duplicate {entityAssets[i].id} and {entityAssets[j].id}");
                    hasDuplicate = true;
                }
            }
        }

        if (!hasDuplicate) DebugLog.Message("No duplicate");
    }

    [Button]
    public void SortByID()
    {
        entityAssets.Sort((e1, e2) => Compare(e1.id, e2.id));

        int Compare(string a, string b)
        {
            int intA = int.Parse(a);
            int intB = int.Parse(b);
            if (intA > intB) return 1;
            if (intA < intB) return -1;
            return 0;
        }
    }

    [Button]
    public void SortBySkeleton()
    {
        entityAssets.Sort((e1, e2) => Compare(e1.battleSkeleton.name, e2.battleSkeleton.name));

        int Compare(string a, string b)
        {
            int intA = int.Parse(a);
            int intB = int.Parse(b);
            if (intA > intB) return 1;
            if (intA < intB) return -1;
            return 0;
        }
    }

    [Button]
    public void GenerateEmptyAssets(int end)
    {
        while (entityAssets.Count - 1 < end)
        {
            entityAssets.Add(new EntityAsset { id = $"{entityAssets.Count:D2}" });
        }
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