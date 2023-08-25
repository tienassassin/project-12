using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DB.System;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntitySpawner : DuztineBehaviour
{
    [SerializeField, AssetsOnly] private BattleEntity entityPref;

    [SerializeField] private Transform entityContainer;
    [SerializeField] private Transform[] heroPositions;
    [SerializeField] private Transform[] devilPositions;
    
    // fake data
    public List<DB.Player.Hero> heroes;
    public List<DevilData> devils;

    private async void Start()
    {
        await UniTask.WaitUntil(() => DataManager.Ready);
        SpawnHeroes();
        SpawnDevils();
    }

    private void SpawnHeroes()
    {
        int firstIndex = GetFirstPositionIndex(heroes.Count);
        for (int i = 0; i < heroes.Count; i++)
        {
            var newEntity = Instantiate(entityPref, heroPositions[firstIndex + i].position, Quaternion.identity, entityContainer);
            newEntity.Init(heroes[i]);
        }
    }

    private void SpawnDevils()
    {
        int firstIndex = GetFirstPositionIndex(devils.Count);
        for (int i = 0; i < devils.Count; i++)
        {
            var newEntity = Instantiate(entityPref, devilPositions[firstIndex + i].position, Quaternion.identity, entityContainer);
            newEntity.Init(devils[i]);
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