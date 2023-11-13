using UnityEngine;

public class EntityController : AssassinBehaviour
{
    [SerializeField] private bool automatic;

    private BattleEntity _entity;
    private EntityUI _entityUI;
    private EntityAutomation _entityAuto;
    private Collider2D _collider;

    private bool _isFocused;

    public BattleEntity Entity => _entity;

    private void Awake()
    {
        _entity = GetComponent<BattleEntity>();
        _entityUI = GetComponent<EntityUI>();
        _entityAuto = GetComponent<EntityAutomation>();
        _collider = GetComponent<Collider2D>();
        
        this.AddListener(EventID.ON_TURN_TAKEN, OnTakeTurn);
        this.AddListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.ON_TURN_TAKEN, OnTakeTurn);
        this.RemoveListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    public void SwitchAutomation(bool active)
    {
        automatic = active;
    }

    private void OnTakeTurn(object id)
    {
        if (_entity.UniqueID != (int)id)
        {
            _entityUI.SwitchHighlight(false);
            return;
        }

        DebugLog.Message("=====> " + name + "'s turn!");
        _entityUI.SwitchHighlight(true);
        BattleManager.Instance.CurrentEntity = this;

        if (!_entity.CanTakeTurn)
        {
            Invoke(nameof(AutoEndTurn), 1f);
        }

        // todo: review later
        if (automatic)
        {
            Invoke(nameof(AutoAction), 1f);
        }
    }

    private void OnFocused(object data)
    {
        if (data == null)
        {
            _isFocused = false;
        }
        else
        {
            var targetType = (SkillTargetType)data;
            var sender = BattleManager.Instance.CurrentEntity.Entity;
            switch (targetType)
            {
                case SkillTargetType.Ally:
                    _isFocused = _entity.Side == sender.Side && _entity.UniqueID != sender.UniqueID;
                    break;

                case SkillTargetType.AllyOrSelf:
                    _isFocused = _entity.Side == sender.Side;
                    break;

                case SkillTargetType.Enemy:
                    _isFocused = _entity.Side != sender.Side;
                    break;

                case SkillTargetType.EnemyOrSelf:
                    _isFocused = _entity.Side != sender.Side || _entity.UniqueID == sender.UniqueID;
                    break;

                case SkillTargetType.ExceptSelf:
                    _isFocused = _entity.UniqueID != sender.UniqueID;
                    break;

                case SkillTargetType.All:
                    _isFocused = true;
                    break;
            }
        }

        _collider.enabled = _isFocused;
        _entityUI.SwitchFocus(_isFocused);
    }

    public void SwitchActionPanel(bool selected)
    {
        _entityUI.SwitchActionPanel(selected);
    }

    private void AutoEndTurn()
    {
        ActionQueue.Instance.EndTurn();
    }

    private void AutoAction()
    {
        var target = _entityAuto.GetTarget();
        DebugLog.Message($"{name} attacked {target.name}");
        Entity.Attack(target.Entity, AutoEndTurn);
    }

    private void OnMouseUpAsButton()
    {
        BattleManager.Instance.SelectEntity(this);
    }
}