using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private int _fireSpirit;
    private int _maxFireSpirit = 5;

    public SkillTargetType targetType;
    public int id;

    public EntityController selectedEntity;

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

    public void AddSpirit()
    {
        _fireSpirit = Mathf.Clamp(_fireSpirit + 1, 0, _maxFireSpirit);
    }

    public bool HasSpirit()
    {
        if (_fireSpirit < 1) return false;
        _fireSpirit--;
        return true;
    }
}