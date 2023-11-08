using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : Singleton<BattleManager>
{
    [SerializeField] private BattleState state;

    [SerializeField] private int fireSpirit;
    [SerializeField] private int maxFireSpirit = 5;

    [SerializeField] private EntityController currentEntity;
    [SerializeField] private EntityController selectedEntity;

    private Action<EntityController> _targetConfirmed;

    public EntityController CurrentEntity
    {
        get => currentEntity;
        set
        {
            currentEntity = value;
            this.PostEvent(EventID.ON_CURRENT_ENTITY_UPDATED, currentEntity);
        }
    }
    public BattleState State
    {
        get => state;
        set
        {
            state = value;
            switch (state)
            {
                case BattleState.Victory:
                    DebugLog.Message("<color=yellow>BATTLE END: VICTORY</color>");
                    // todo: review later
                    break;

                case BattleState.Loss:
                    DebugLog.Message("<color=yellow>BATTLE END: LOSS</color>");
                    // todo: review later
                    break;
            }
        }
    }

    private void Start()
    {
        state = BattleState.Preparing;
        UpdateFireSpirit(1);
        StartBattle();
    }

    private async void StartBattle()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(3));
        DebugLog.Message("<color=yellow>BATTLE START!</color>");
        state = BattleState.Playing;
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
            DebugLog.Message("Selected " + entity.name);
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

    public void UnfocusAll()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED);
    }

    public void RequestAttack()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED, SkillTargetType.Enemy);
        _targetConfirmed = target =>
        {
            DebugLog.Message($"{CurrentEntity.name} attacked {target.name}");
            CurrentEntity.Entity.Attack(target.Entity, () =>
            {
                _targetConfirmed = null;
                ActionQueue.Instance.EndTurn();
            });
        };
    }

    public void RequestUseSkill()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED, CurrentEntity.Entity.SkillTargetType);
        _targetConfirmed = target =>
        {
            DebugLog.Message($"{CurrentEntity.name} used skill on {target.name}");
            // todo: review later
            CurrentEntity.Entity.UseSkill(target.Entity);
        };
    }

    public void RequestUseUltimate()
    {
        SelectEntity(null);
        this.PostEvent(EventID.ON_TARGET_FOCUSED, CurrentEntity.Entity.UltimateTargetType);
        _targetConfirmed = target =>
        {
            DebugLog.Message($"{CurrentEntity.name} used ultimate on {target.name}");
            // todo: review later
            CurrentEntity.Entity.UseUltimate(target.Entity);
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