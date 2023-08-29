using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntitySpawner : DuztineBehaviour
{
    [SerializeField] private EntityPrefabList entityPrefList;

    [SerializeField] private Transform entityContainer;
    [SerializeField] private Transform[] heroPositions;
    [SerializeField] private Transform[] devilPositions;

    // fake data
    public List<HeroData> heroes = new();
    public List<DevilData> devils = new();
    private List<BattleEntity> _entities = new();

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
            var pref = entityPrefList.GetHeroPrefab(heroes[i].heroId);
            var newEntity = Instantiate(pref, heroPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Init(heroes[i]);
            _entities.Add(newEntity);
        }
    }

    private void SpawnDevils()
    {
        int firstIndex = GetFirstPositionIndex(devils.Count);
        for (int i = 0; i < devils.Count; i++)
        {
            var pref = entityPrefList.GetDevilPrefab(devils[i].devilId);
            var newEntity = Instantiate(pref, devilPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Init(devils[i]);
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
}