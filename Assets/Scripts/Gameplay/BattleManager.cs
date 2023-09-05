using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private int fireSpirit;
    [SerializeField] private int maxFireSpirit = 5;

    [SerializeField] private EntityController currentEntity;
    [SerializeField] private EntityController selectedEntity;

    private Action<EntityController> _targetConfirmed;

    private void Start()
    {
        UpdateFireSpirit(1);
    }

    public void SelectEntity(EntityController entity)
    {
        if (selectedEntity == entity) return;

        if (selectedEntity != null)
        {
            selectedEntity.SwitchActionPanel(false);
        }

        selectedEntity = entity;

        if (selectedEntity)
        {
            EditorLog.Message("Selected " + entity.name);
            selectedEntity.SwitchActionPanel(true);
        }
    }

    public bool HasFireSpirit()
    {
        return fireSpirit > 0;
    }

    public void AddFireSpirit()
    {
        UpdateFireSpirit(fireSpirit + 1);
    }

    public void ConsumeFireSpirit()
    {
        UpdateFireSpirit(fireSpirit - 1);
    }

    private void UpdateFireSpirit(int amount)
    {
        fireSpirit = Mathf.Clamp(amount, 0, maxFireSpirit);
        this.PostEvent(EventID.ON_FIRE_SPIRIT_UPDATED, Tuple.Create(fireSpirit, maxFireSpirit));
    }

    public void PreviewFireSpirit(int difference)
    {
        difference = Mathf.Min(difference, maxFireSpirit - fireSpirit);
        this.PostEvent(EventID.ON_FIRE_SPIRIT_PREVIEWED, difference);
    }

    public void UpdateCurrentEntity(EntityController entity)
    {
        currentEntity = entity;
        this.PostEvent(EventID.ON_CURRENT_ENTITY_UPDATED, entity);
    }

    public void UnfocusAll()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED);
    }

    public void RequestAttack()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED, Tuple.Create(SkillTargetType.Enemy, currentEntity.Entity.UniqueID));
        _targetConfirmed = target =>
        {
            EditorLog.Message($"{currentEntity.name} attacked {target.name}");
            currentEntity.Entity.Attack(target.Entity, () =>
            {
                _targetConfirmed = null;
                ActionQueue.Instance.EndTurn();
            });
        };
    }

    public void RequestUseSkill()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED,
            Tuple.Create(currentEntity.Entity.SkillTargetType, currentEntity.Entity.UniqueID));
        _targetConfirmed = target =>
        {
            EditorLog.Message($"{currentEntity.name} used skill on {target.name}");
            // todo: review later
            currentEntity.Entity.UseSkill(target.Entity);
        };
    }

    public void RequestUseUltimate()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED,
            Tuple.Create(currentEntity.Entity.UltimateTargetType, currentEntity.Entity.UniqueID));
        _targetConfirmed = target =>
        {
            EditorLog.Message($"{currentEntity.name} used ultimate on {target.name}");
            // todo: review later
            currentEntity.Entity.UseUltimate(target.Entity);
        };
    }

    public void ConfirmAction()
    {
        if (!selectedEntity) return;

        var target = selectedEntity;
        UnfocusAll();
        _targetConfirmed?.Invoke(target);
    }

    [Button]
    public void Debug_QuickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}