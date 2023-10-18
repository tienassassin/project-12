using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLibrary", menuName = "AssetLibrary/ItemLibrary")]
public class ItemLibrary : ScriptableObject
{
    [TableList] public List<ItemAsset> itemAssets;
}

[Serializable]
public struct ItemAsset
{
    public string id;
    [PreviewField(ObjectFieldAlignment.Center)]
    public Sprite icon;
}