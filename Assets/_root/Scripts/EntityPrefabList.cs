using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrefabList", menuName = "EntityPrefabList")]
public class EntityPrefabList : ScriptableObject
{
    [TableList]
    public List<EntityPrefab> entityPrefabs = new();

    public EntityController GetEntityPrefab(string id)
    {
        return entityPrefabs.Find(x => x.prefabID.Equals(id)).entity;
    }
}

[Serializable]
public struct EntityPrefab
{
    [TableColumnWidth(100, false)]
    public string prefabID;
    public EntityController entity;
}