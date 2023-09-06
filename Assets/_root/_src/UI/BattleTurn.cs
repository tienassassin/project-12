using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleTurn : DuztineBehaviour
{
    [SerializeField] private TMP_Text txtName;
    [SerializeField] private BattleExtraTurn turnPref;
    [SerializeField] private Transform turnContainer;

    private List<BattleExtraTurn> _turns;

    private void Awake()
    {
        _turns = new List<BattleExtraTurn>();
        foreach (Transform children in turnContainer)
        {
            var o = children.gameObject;
            o.SetActive(false);
            _turns.Add(o.GetComponent<BattleExtraTurn>());
        }
    }

    public void Init(Turn data)
    {
        var turnInfo = data.info;
        txtName.text = turnInfo.name;
        name = turnInfo.name;

        var extraTurns = data.extraTurns;
        while (turnContainer.childCount < extraTurns.Count)
        {
            var o = Instantiate(turnPref, turnContainer);
            _turns.Add(o);
        }

        for (int i = 0; i < _turns.Count; i++)
        {
            var turn = _turns[i];
            if (i >= extraTurns.Count)
            {
                turn.gameObject.SetActive(false);
                turn.name = Constants.EMPTY_MARK;
                continue;
            }

            turn.gameObject.SetActive(true);
            turn.Init(extraTurns[i].name);
        }
    }
}