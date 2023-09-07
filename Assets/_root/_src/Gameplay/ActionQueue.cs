using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    [SerializeField] private List<Turn> queue;
    private Turn _curTurn;

    private int _currentPhase = 0;
    public int CurrentPhase => _currentPhase;

    public async void InitQueue(List<EntityController> entities)
    {
        var sortedEntities = new List<EntityController>();
        entities.ForEach(x => sortedEntities.Add(x));
        sortedEntities.Sort((e1, e2) => CompareOrder(e2, e1));

        sortedEntities.ForEach(x => { queue.Add(new Turn(x.Entity.UniqueID, x.name, false)); });

        await UniTask.WaitUntil(() => BattleManager.Instance.State == BattleState.Playing);

        NextTurn();

        int CompareOrder(EntityController e1, EntityController e2)
        {
            var s1 = e1.Entity.Stats;
            var s2 = e2.Entity.Stats;
            int speedComparision = s1.speed.CompareTo(s2.speed);
            if (speedComparision != 0) return speedComparision;
            int luckComparision = s1.luck.CompareTo(s2.luck);
            if (luckComparision != 0) return luckComparision;
            int dmgComparision = s2.damage.CompareTo(s1.damage);
            return dmgComparision;
        }
    }

    public void RemoveEntity(int id)
    {
        // var turns = queue.FindAll(x => x.info.id == id);
        // turns.ForEach(x => queue.Remove(x));
        queue.RemoveAll(x => x.info.id == id);
    }

    private void NextTurn()
    {
        BattleManager.Instance.UnfocusAll();

        _curTurn = queue[0];
        this.PostEvent(EventID.ON_TURN_TAKEN, _curTurn.info.id);
        this.PostEvent(EventID.ON_ACTION_QUEUE_CHANGED, queue);
    }

    public void AddExtraTurn(TurnInfo extraTurn)
    {
        _curTurn.AddExtraTurn(extraTurn);
    }

    [Button]
    public void EndTurn()
    {
        if (BattleManager.Instance.State != BattleState.Playing) return;

        var lastTurn = queue[0];
        queue.RemoveAt(0);

        for (int i = 0; i < lastTurn.extraTurns.Count; i++)
        {
            var extraTurn = lastTurn.extraTurns[i];
            queue.Insert(i, new Turn(extraTurn.id, extraTurn.name, true));
        }

        lastTurn.extraTurns.Clear();
        if (!lastTurn.isExtra) queue.Add(lastTurn);

        NextTurn();
    }
}

[Serializable]
public class Turn
{
    public TurnInfo info;
    public bool isExtra;
    public List<TurnInfo> extraTurns;

    public Turn(int id, string name, bool isExtra)
    {
        info = new TurnInfo(id, name);
        this.isExtra = isExtra;
        extraTurns = new List<TurnInfo>();
    }

    public void AddExtraTurn(TurnInfo extraTurn)
    {
        extraTurns.Add(extraTurn);
    }
}

[Serializable]
public class TurnInfo
{
    public int id;
    public string name;

    public TurnInfo(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}