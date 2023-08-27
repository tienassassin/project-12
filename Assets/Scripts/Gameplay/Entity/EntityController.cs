using System;
using UnityEngine;

public class EntityController : DuztineBehaviour
{
    [SerializeField] private GameObject focus; // just test, remove later

    private BattleEntity _entity;
    private Collider2D _collider;

    private bool _isFocused;

    public Action<bool> entitySelected;

    public bool CanTakeTurn => _entity.CanTakeTurn;
    public bool IsAlive => _entity.IsAlive;

    private void Awake()
    {
        _entity = GetComponent<BattleEntity>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        this.AddListener(EventID.ON_TAKE_TURN, OnTakeTurn);
        this.AddListener(EventID.ON_TARGET_FOCUSED, OnFocused);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_TAKE_TURN, OnTakeTurn);
        this.AddListener(EventID.ON_TARGET_FOCUSED, OnFocused);
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
                    _isFocused = _entity.Faction == Faction.Hero && _entity.ID != id;
                    break;
                case SkillTargetType.AllyOrSelf:
                    _isFocused = _entity.Faction == Faction.Hero;
                    break;
                case SkillTargetType.Enemy:
                    _isFocused = _entity.Faction == Faction.Devil;
                    break;
                case SkillTargetType.EnemyOrSelf:
                    _isFocused = _entity.Faction == Faction.Devil || _entity.ID == id;
                    break;
                case SkillTargetType.ExceptSelf:
                    _isFocused = _entity.Faction == Faction.Devil ||
                                 (_entity.Faction == Faction.Hero && _entity.ID != id);
                    break;
                case SkillTargetType.All:
                    _isFocused = true;
                    break;
            }
        }

        _collider.enabled = _isFocused;
        focus.SetActive(_isFocused);
    }

    private void EndTurn()
    {
        ActionQueue.Instance.EndTurn();
    }

    private void OnMouseUpAsButton()
    {
        BattleManager.Instance.SelectEntity(this);
    }
}