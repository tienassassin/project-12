public class EntityController : DuztineBehaviour
{
    private BattleEntity _entity;

    public bool CanTakeTurn => _entity.CanTakeTurn;
    public bool IsAlive => _entity.IsAlive;

    private void Awake()
    {
        _entity = GetComponent<BattleEntity>();
    }

    private void OnEnable()
    {
        this.AddListener(EventID.ON_TAKE_TURN, OnTakeTurn);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_TAKE_TURN, OnTakeTurn);
    }

    private void OnTakeTurn(object id)
    {
        if (_entity.ID != (int)id) return;
        if (!IsAlive) return;

        if (!CanTakeTurn)
        {
            Invoke(nameof(EndTurn), 1f);
        }

        EditorLog.Message(name + "'s turn!");
    }

    private void EndTurn()
    {
        ActionQueue.Instance.EndTurn();
    }
}