using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarLibrary", menuName = "AssetLibrary/AvatarLibrary")]
public class AvatarLibrary : ScriptableObject
{
    [TableList] public List<AvatarAsset> avatarAssets;
    [TableList] public List<AvatarFrameAsset> avatarFrameAssets;
}

[Serializable]
public struct AvatarAsset
{
    public string id;
    [PreviewField(ObjectFieldAlignment.Center)]
    public Sprite avatar;
}

[Serializable]
public struct AvatarFrameAsset
{
    public string id;
    [PreviewField(ObjectFieldAlignment.Center)]
    public Sprite avatarFrame;
}