using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private int _fireSpirit;
    private int _maxFireSpirit = 5;

    public SkillTargetType targetType;
    public int id;

    public EntityController currentEntity;
    public EntityController selectedEntity;

    private void Start()
    {
        UpdateFireSpirit(0);
    }

    [Button]
    public void Test()
    {
        selectedEntity = null;
        this.PostEvent(EventID.ON_TARGET_FOCUSED, Tuple.Create(targetType, id));
    }

    [Button]
    public void TestUnfocus()
    {
        selectedEntity = null;
        this.PostEvent(EventID.ON_TARGET_FOCUSED);
    }

    public void SelectEntity(EntityController entity)
    {
        if (selectedEntity == entity) return;

        if (selectedEntity != null)
        {
            selectedEntity.entitySelected?.Invoke(false);
        }

        EditorLog.Message("Selected " + entity.name);
        selectedEntity = entity;
        selectedEntity.entitySelected?.Invoke(true);
    }

    public void ClearSelectedEntity()
    {
        selectedEntity = null;
    }

    public bool HasEnoughFireSpirit(int amount)
    {
        return _fireSpirit >= amount;
    }

    [Button]
    public void AddFireSpirit()
    {
        UpdateFireSpirit(_fireSpirit + 1);
    }

    [Button]
    public void ConsumeFireSpirit()
    {
        UpdateFireSpirit(_fireSpirit - 1);
    }

    private void UpdateFireSpirit(int amount)
    {
        _fireSpirit = Mathf.Clamp(amount, 0, _maxFireSpirit);
        this.PostEvent(EventID.ON_FIRE_SPIRIT_UPDATED, Tuple.Create(_fireSpirit, _maxFireSpirit));
    }

    [Button]
    public void PreviewFireSpirit(int difference)
    {
        difference = Mathf.Min(difference, _maxFireSpirit - _fireSpirit);
        this.PostEvent(EventID.ON_FIRE_SPIRIT_PREVIEWED, difference);
    }

    public void UpdateCurrentEntity(EntityController entity)
    {
        currentEntity = entity;
        this.PostEvent(EventID.ON_CURRENT_ENTITY_UPDATED, entity);
    }
}