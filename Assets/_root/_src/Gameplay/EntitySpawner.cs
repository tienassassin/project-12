using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntitySpawner : Singleton<EntitySpawner>
{
    [SerializeField] private EntityPrefabList entityPrefList;

    [SerializeField] private Transform entityContainer;
    [SerializeField] private Transform[] heroPositions;
    [SerializeField] private Transform[] devilPositions;

    // fake data
    public List<EntitySaveData> heroes = new();
    public List<EnemyData> devils = new();
    private List<EntityController> _entities = new();

    private async void Start()
    {
        await UniTask.WaitUntil(() => DataManager.Ready);
        SpawnHeroes();
        SpawnDevils();
        ActionQueue.Instance.InitQueue(_entities);
    }

    private void SpawnHeroes()
    {
        int firstIndex = GetFirstPositionIndex(heroes.Count);
        for (int i = 0; i < heroes.Count; i++)
        {
            var pref = entityPrefList.GetEntityPrefab(heroes[i].entityId);
            var newEntity = Instantiate(pref, heroPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Entity.Init(heroes[i]);
            _entities.Add(newEntity);
        }
    }

    private void SpawnDevils()
    {
        int firstIndex = GetFirstPositionIndex(devils.Count);
        for (int i = 0; i < devils.Count; i++)
        {
            var pref = entityPrefList.GetEntityPrefab(devils[i].entityId);
            var newEntity = Instantiate(pref, devilPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Entity.Init(devils[i]);
            _entities.Add(newEntity);
        }
    }

    private int GetFirstPositionIndex(int quantity)
    {
        // The first member of a X-member team will be in Y-th position
        //                   0   1   2   3
        // X = 1 -> Y = 1   [ ] [x] [ ] [ ]
        // X = 2 -> Y = 1   [ ] [x] [x] [ ]
        // X = 3 -> Y = 0   [x] [x] [x] [ ]
        // X = 4 -> Y = 0   [x] [x] [x] [x]
        return quantity <= 2 ? 1 : 0;
    }

    [Button]
    public List<EntityController> GetAllEntities(Faction faction)
    {
        return _entities.Where(x => x.Entity.Faction == faction && x.Entity.IsAlive).ToList();
    }

    public EntityController GetRandomEntity(Faction faction)
    {
        var entities = _entities.Where(x => x.Entity.Faction == faction && x.Entity.IsAlive).ToList();
        if (entities.Count < 1) return null;
        return entities[Random.Range(0, entities.Count)];
    }

    [Button]
    public List<EntityController> GetAdjacentEntities(EntityController entity, bool includeMain = true)
    {
        var result = new List<EntityController>();
        if (includeMain) result.Add(entity);
        var indexOfMain = _entities.FindIndex(x => x == entity);
        if (indexOfMain >= 0)
        {
            int leftIndex = indexOfMain - 1;
            if (leftIndex >= 0)
            {
                var leftEntity = _entities[leftIndex];
                if (leftEntity != null
                    && leftEntity.Entity.IsAlive
                    && leftEntity.Entity.Faction == entity.Entity.Faction)
                {
                    result.Add(leftEntity);
                }
            }

            int rightIndex = indexOfMain + 1;
            if (rightIndex < _entities.Count)
            {
                var rightEntity = _entities[rightIndex];
                if (rightEntity != null
                    && rightEntity.Entity.IsAlive
                    && rightEntity.Entity.Faction == entity.Entity.Faction)
                {
                    result.Add(rightEntity);
                }
            }
        }

        return result;
    }
}