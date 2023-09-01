using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrefabList", menuName = "EntityPrefabList")]
public class EntityPrefabList : ScriptableObject
{
    [TableList]
    public List<EntityPrefab> heroPrefs = new();

    [TableList]
    public List<EntityPrefab> devilPrefs = new();

    public EntityController GetHeroPrefab(string id)
    {
        return heroPrefs.Find(x => x.prefabID.Equals(id)).entity;
    }

    public EntityController GetDevilPrefab(string id)
    {
        return devilPrefs.Find(x => x.prefabID.Equals(id)).entity;
    }
}

[Serializable]
public struct EntityPrefab
{
    [TableColumnWidth(100, false)]
    public string prefabID;
    public EntityController entity;
}