using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    [SerializeField] private EntityPrefabList entityPrefList;

    [SerializeField] private Transform entityContainer;
    [SerializeField] private Transform[] heroPositions;
    [SerializeField] private Transform[] devilPositions;

    // fake data
    public List<EntitySaveData> allies = new();
    public List<EnemyData> enemies = new();
    private List<EntityController> _entities = new();

    private int _allyCount;
    private int _enemyCount;

    private async void Start()
    {
        await UniTask.WaitUntil(() => DataManager.Ready);
        SpawnAllyTeam();
        SpawnEnemyTeam();
        ActionQueue.Instance.InitQueue(_entities);
    }

    private void SpawnAllyTeam()
    {
        int firstIndex = GetFirstPositionIndex(allies.Count);
        for (int i = 0; i < allies.Count; i++)
        {
            var pref = entityPrefList.GetEntityPrefab(allies[i].entityId);
            var newEntity = Instantiate(pref, heroPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Entity.Init(allies[i]);
            newEntity.name = "(A)" + newEntity.name;
            newEntity.SwitchAutomation(true);
            _entities.Add(newEntity);
            _allyCount++;
        }

        EditorLog.Message("Ally team loaded");
    }

    private void SpawnEnemyTeam()
    {
        int firstIndex = GetFirstPositionIndex(enemies.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            var pref = entityPrefList.GetEntityPrefab(enemies[i].entityId);
            var newEntity = Instantiate(pref, devilPositions[firstIndex + i].position, Quaternion.identity,
                entityContainer);
            newEntity.Entity.Init(enemies[i]);
            newEntity.name = "(E)" + newEntity.name;
            newEntity.SwitchAutomation(true);
            _entities.Add(newEntity);
            _enemyCount++;
        }

        EditorLog.Message("Enemy team loaded");
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
    public List<EntityController> GetAllEntities(Side side)
    {
        return _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
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
                    && leftEntity.Entity.Side == entity.Entity.Side)
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
                    && rightEntity.Entity.Side == entity.Entity.Side)
                {
                    result.Add(rightEntity);
                }
            }
        }

        return result;
    }

    public EntityController GetEntity(Side side, TargetSelectCondition condition)
    {
        switch (condition)
        {
            case TargetSelectCondition.Random:
                return GetRandomEntity(side);

            case TargetSelectCondition.LowestHp:
                return GetLowestHpEntity(side);

            case TargetSelectCondition.LowestHpPercentage:
                return GetLowestHpPercentageEntity(side);

            case TargetSelectCondition.LowestEnergy:
                return GetLowestEnergyEntity(side);

            case TargetSelectCondition.LowestArmor:
                return GetLowestArmorEntity(side);

            case TargetSelectCondition.LowestResistance:
                return GetLowestResistanceEntity(side);

            case TargetSelectCondition.HighestDamage:
                return GetHighestDamageEntity(side);

            case TargetSelectCondition.HighestSpeed:
                return GetHighestSpeedEntity(side);

            case TargetSelectCondition.HighestIntelligence:
                return GetHighestIntelligenceEntity(side);

            default:
                return null;
        }
    }

    #region Get entity based on condition

    private EntityController GetRandomEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        return list[Random.Range(0, list.Count)];
    }

    private EntityController GetLowestHpEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e1.Entity.Hp.CompareTo(e2.Entity.Hp));
        return list[0];
    }

    private EntityController GetLowestHpPercentageEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e1.Entity.HpPercentage.CompareTo(e2.Entity.HpPercentage));
        return list[0];
    }

    private EntityController GetLowestEnergyEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e1.Entity.Energy.CompareTo(e2.Entity.Energy));
        return list[0];
    }

    private EntityController GetLowestArmorEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e1.Entity.Stats.armor.CompareTo(e2.Entity.Stats.armor));
        return list[0];
    }

    private EntityController GetLowestResistanceEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e1.Entity.Stats.resistance.CompareTo(e2.Entity.Stats.resistance));
        return list[0];
    }

    private EntityController GetHighestDamageEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e2.Entity.Stats.damage.CompareTo(e1.Entity.Stats.damage));
        return list[0];
    }

    private EntityController GetHighestSpeedEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e2.Entity.Stats.speed.CompareTo(e1.Entity.Stats.speed));
        return list[0];
    }

    private EntityController GetHighestIntelligenceEntity(Side side)
    {
        var list = _entities.Where(x => x.Entity.Side == side && x.Entity.IsAlive).ToList();
        if (list.Count < 1) return null;
        list.Sort((e1, e2) => e2.Entity.Stats.intelligence.CompareTo(e1.Entity.Stats.intelligence));
        return list[0];
    }

    #endregion

    public void OnEntityDead(Side side)
    {
        if (side == Side.Ally) _allyCount--;
        else _enemyCount--;

        if (_allyCount <= 0)
        {
            BattleManager.Instance.State = BattleState.Loss;
        }
        else if (_enemyCount <= 0)
        {
            BattleManager.Instance.State = BattleState.Victory;
        }
    }
}