using System;
using UnityEngine;

public class EntityController : DuztineBehaviour
{
    private BattleEntity _entity;
    private EntityUI _entityUI;
    private Collider2D _collider;

    private bool _isFocused;

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
        this.RemoveListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    private void OnTakeTurn(object id)
    {
        if (_entity.UniqueID != (int)id)
        {
            _entityUI.SwitchHighlight(false);
            return;
        }

        EditorLog.Message(name + "'s turn!");
        _entityUI.SwitchHighlight(true);
        BattleManager.Instance.UpdateCurrentEntity(this);

        if (!_entity.CanTakeTurn)
        {
            Invoke(nameof(AutoEndTurn), 1f);
        }

        // todo: review later
        if (_entity.Faction != Faction.Hero)
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
        var target = EntitySpawner.Instance.GetRandomEntity(Faction.Hero);
        EditorLog.Message($"{name} attacked {target.name}");
        Entity.Attack(target.Entity);
        AutoEndTurn();
    }

    private void OnMouseUpAsButton()
    {
        BattleManager.Instance.SelectEntity(this);
    }
}