using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    [SerializeField] private List<Turn> _queue;
    private Turn _curTurn;

    private int _currentPhase;
    public int CurrentPhase => _currentPhase;

    public void InitQueue(List<BattleEntity> entities)
    {
        var sortedEntities = new List<BattleEntity>();
        entities.ForEach(x => sortedEntities.Add(x));
        sortedEntities.Sort((e1, e2) => CompareOrder(e2, e1));

        sortedEntities.ForEach(x => { _queue.Add(new Turn(x.ID, x.name, false)); });

        NextTurn();

        int CompareOrder(BattleEntity e1, BattleEntity e2)
        {
            var s1 = e1.Stats;
            var s2 = e2.Stats;
            int speedComparision = s1.speed.CompareTo(s2.speed);
            if (speedComparision != 0) return speedComparision;
            int luckComparision = s1.luck.CompareTo(s2.luck);
            if (luckComparision != 0) return luckComparision;
            int dmgComparision = s2.damage.CompareTo(s1.damage);
            return dmgComparision;
        }
    }

    private void NextTurn()
    {
        _curTurn = _queue[0];
        this.PostEvent(EventID.ON_TAKE_TURN, _curTurn.info.id);
        this.PostEvent(EventID.ON_ACTION_QUEUE_CHANGED, _queue);
    }

    public void AddExtraTurn(TurnInfo extraTurn)
    {
        _curTurn.AddExtraTurn(extraTurn);
    }

    [Button]
    public void EndTurn()
    {
        var lastTurn = _queue[0];
        _queue.RemoveAt(0);

        for (int i = 0; i < lastTurn.extraTurns.Count; i++)
        {
            var extraTurn = lastTurn.extraTurns[i];
            _queue.Insert(i, new Turn(extraTurn.id, extraTurn.name, true));
        }

        lastTurn.extraTurns.Clear();
        if (!lastTurn.isExtra) _queue.Add(lastTurn);

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