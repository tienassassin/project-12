using System.Collections.Generic;
using UnityEngine;

public class BattleUI : BaseUI
{
    [SerializeField] private BattleTurn turnPref;
    [SerializeField] private Transform turnContainer;

    private List<BattleTurn> _turns;
    private float _turnOffset0 = -75;
    private float _turnOffset1 = -60;
    private float _curTurnScale = 1;
    private float _normalTurnScale = 0.8f;

    public static void Show()
    {
        BattleUIManager.Instance.ShowUI(nameof(BattleUI));
    }

    public static void Hide()
    {
        BattleUIManager.Instance.HideUI(nameof(BattleUI));
    }

    protected override void Awake()
    {
        base.Awake();

        _turns = new List<BattleTurn>();
        foreach (Transform children in turnContainer)
        {
            var o = children.gameObject;
            o.SetActive(false);
            _turns.Add(o.GetComponent<BattleTurn>());
        }
    }

    private void OnEnable()
    {
        this.AddListener(EventID.ON_ACTION_QUEUE_CHANGED, Refresh);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_ACTION_QUEUE_CHANGED, Refresh);
    }

    public void Refresh(object data)
    {
        var queue = (List<Turn>)data;
        while (turnContainer.childCount < queue.Count)
        {
            var o = Instantiate(turnPref, turnContainer);
            _turns.Add(o);
        }

        for (int i = 0; i < _turns.Count; i++)
        {
            var turn = _turns[i];
            if (i >= queue.Count)
            {
                turn.gameObject.SetActive(false);
                turn.name = Constants.EMPTY_MARK;
                continue;
            }

            turn.gameObject.SetActive(true);
            turn.Init(queue[i]);

            bool isCurTurn = (i == 0);
            float offset = isCurTurn ? 0 : (_turnOffset0 + _turnOffset1 * (i - 1));
            turn.transform.localPosition = new Vector3(0, offset, 0);
            turn.transform.localScale = Vector3.one * (isCurTurn ? _curTurnScale : _normalTurnScale);
        }
    }
}