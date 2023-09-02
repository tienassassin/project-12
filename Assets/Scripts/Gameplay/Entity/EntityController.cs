using System;
using UnityEngine;

public class EntityController : DuztineBehaviour
{
    private BattleEntity _entity;
    private EntityUI _entityUI;
    private Collider2D _collider;

    private bool _isFocused;

    public Action<bool> entitySelected;

    public BattleEntity Entity => _entity;

    private void Awake()
    {
        _entity = GetComponent<BattleEntity>();
        _entityUI = GetComponent<EntityUI>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        this.AddListener(EventID.ON_TURN_TAKEN, OnTakeTurn);
        this.AddListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_TURN_TAKEN, OnTakeTurn);
        this.AddListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    private void OnTakeTurn(object id)
    {
        if (_entity.UniqueID != (int)id || !_entity.IsAlive)
        {
            _entityUI.SwitchHighlight(false);
            return;
        }

        if (!_entity.CanTakeTurn)
        {
            Invoke(nameof(EndTurn), 1f);
        }

        EditorLog.Message(name + "'s turn!");
        _entityUI.SwitchHighlight(true);
        BattleManager.Instance.UpdateCurrentEntity(this);
    }

    private void OnFocused(object data)
    {
        if (data == null)
        {
            _isFocused = false;
        }
        else
        {
            var (targetType, id) = (Tuple<SkillTargetType, int>)data;
            switch (targetType)
            {
                case SkillTargetType.Ally:
                    _isFocused = _entity.Faction == Faction.Hero && _entity.UniqueID != id;
                    break;
                case SkillTargetType.AllyOrSelf:
                    _isFocused = _entity.Faction == Faction.Hero;
                    break;
                case SkillTargetType.Enemy:
                    _isFocused = _entity.Faction == Faction.Devil;
                    break;
                case SkillTargetType.EnemyOrSelf:
                    _isFocused = _entity.Faction == Faction.Devil || _entity.UniqueID == id;
                    break;
                case SkillTargetType.ExceptSelf:
                    _isFocused = _entity.Faction == Faction.Devil ||
                                 (_entity.Faction == Faction.Hero && _entity.UniqueID != id);
                    break;
                case SkillTargetType.All:
                    _isFocused = true;
                    break;
            }
        }

        _collider.enabled = _isFocused;
        _entityUI.SwitchFocus(_isFocused);
    }

    public void Action()
    {
    }

    public void EndTurn()
    {
        ActionQueue.Instance.EndTurn();
    }

    private void OnMouseUpAsButton()
    {
        BattleManager.Instance.SelectEntity(this);
    }
}