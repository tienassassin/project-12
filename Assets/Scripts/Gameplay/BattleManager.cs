using System;
using Sirenix.OdinInspector;

public class BattleManager : Singleton<BattleManager>
{
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
}