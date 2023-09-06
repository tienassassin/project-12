using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    private Dictionary<string, RecyclableObject> _prefabPool = new();
    private Dictionary<string, Queue<RecyclableObject>> _instantiatedObjects = new();
    private Dictionary<string, Transform> _categories = new();

    private bool HasPrefabInPool(string prefabName)
    {
        return _prefabPool.ContainsKey(prefabName);
    }

    private void AddToPool(RecyclableObject newObj)
    {
        _prefabPool.Add(newObj.name, newObj);
    }

    public RecyclableObject SpawnObject(RecyclableObject prefab, Transform parent = null)
    {
        if (!HasPrefabInPool(prefab.name))
        {
            AddToPool(prefab);
        }

        var obj = GetNewObject(prefab.name);

        if (obj)
        {
            if (parent) obj.transform.SetParent(parent);
            else obj.transform.SetParent(GetCategory(obj.Category));
            obj.gameObject.SetActive(true);
            obj.transform.localPosition = Vector3.zero;
            obj.OnSpawn();
        }

        return obj;
    }

    public RecyclableObject SpawnObject(RecyclableObject prefab, Vector3 position)
    {
        if (!HasPrefabInPool(prefab.name))
        {
            AddToPool(prefab);
        }

        var obj = GetNewObject(prefab.name);

        if (obj)
        {
            obj.transform.SetParent(GetCategory(obj.Category));
            obj.gameObject.SetActive(true);
            obj.transform.position = position;
            obj.OnSpawn();
        }

        return obj;
    }

    public RecyclableObject SpawnObject(RecyclableObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!HasPrefabInPool(prefab.name))
        {
            AddToPool(prefab);
        }

        var obj = GetNewObject(prefab.name);

        if (obj)
        {
            obj.transform.SetParent(GetCategory(obj.Category));
            obj.gameObject.SetActive(true);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.OnSpawn();
        }

        return obj;
    }

    public T SpawnObject<T>(RecyclableObject prefab, Transform parent = null) where T : RecyclableObject
    {
        return SpawnObject(prefab, parent) as T;
    }

    public T SpawnObject<T>(RecyclableObject prefab, Vector3 position) where T : RecyclableObject
    {
        return SpawnObject(prefab, position) as T;
    }

    public T SpawnObject<T>(RecyclableObject prefab, Vector3 position, Quaternion rotation) where T : RecyclableObject
    {
        return SpawnObject(prefab, position, rotation) as T;
    }

    private RecyclableObject GetNewObject(string prefabName)
    {
        if (_instantiatedObjects.ContainsKey(prefabName))
        {
            if (_instantiatedObjects[prefabName].Count > 0)
            {
                var obj = _instantiatedObjects[prefabName].Dequeue();
                return obj;
            }
        }

        var prefab = GetPrefab(prefabName);

        if (prefab)
        {
            var obj = Instantiate(prefab);
            obj.originalName = prefabName;
            obj.name = prefabName;
            return obj;
        }

        return null;
    }

    private Transform GetCategory(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            categoryName = "Common";
        }

        if (!_categories.ContainsKey(categoryName))
        {
            var obj = new GameObject(categoryName);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            _categories.Add(categoryName, obj.transform);
        }

        return _categories[categoryName];
    }

    public void DestroyObject(RecyclableObject obj)
    {
        var objName = obj.originalName;
        if (!_instantiatedObjects.ContainsKey(objName))
        {
            _instantiatedObjects[objName] = new Queue<RecyclableObject>();
        }

        _instantiatedObjects[objName].Enqueue(obj);
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(GetCategory(obj.Category));
    }

    private RecyclableObject GetPrefab(string prefabName)
    {
        return _prefabPool.TryGetValue(prefabName, out var value) ? value : null;
    }
}