using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTurn : DuztineBehaviour
{
    [SerializeField] private BattleExtraTurn turnPref;
    [SerializeField] private Transform turnContainer;
    [SerializeField] private Image imgBackground;
    [SerializeField] private Image imgBanner;
    [SerializeField] private Sprite sprAlly;
    [SerializeField] private Sprite sprEnemy;
    [SerializeField] private Color colorAlly;
    [SerializeField] private Color colorEnemy;

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
        name = turnInfo.name;
        imgBackground.sprite = (turnInfo.side == Side.Ally ? sprAlly : sprEnemy);
        imgBackground.color = (turnInfo.side == Side.Ally ? colorAlly : colorEnemy);
        imgBanner.sprite = turnInfo.img;
        
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
            turn.Init(extraTurns[i]);
        }
    }
}