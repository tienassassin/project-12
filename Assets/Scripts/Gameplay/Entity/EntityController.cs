public class EntityController : DuztineBehaviour
{
    private int _id;
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

    public void SetID(int id)
    {
        _id = id;
    }

    private void OnTakeTurn(object id)
    {
        if (_id != (int)id) return;
        if (!IsAlive) return;

        EditorLog.Message(name + "'s turn!");
    }
}