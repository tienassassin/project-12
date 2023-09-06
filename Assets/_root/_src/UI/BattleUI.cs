using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : BaseUI
{
    [TitleGroup("Turn queue:")]
    [SerializeField] private BattleTurn turnPref;
    [SerializeField] private Transform turnContainer;

    [TitleGroup("Action menu:")]
    [SerializeField] private GameObject actionMenu;
    [SerializeField] private Image imgSkill;

    [TitleGroup("Fire spirit:")]
    [SerializeField] private Image imgMainFireSpirit;
    [SerializeField] private Image imgSubFireSpirit;
    [SerializeField] private float fireSpiritStep = 1.75f;

    [TitleGroup("Ultimate:")]
    private int _ultimateIndex;
    [SerializeField] private BattleHeroUltimate[] imgUltimateList;

    private List<BattleTurn> _turns;
    private float _turnOffset0 = -75;
    private float _turnOffset1 = -60;
    private float _curTurnScale = 1;
    private float _normalTurnScale = 0.8f;
    private Color _colorSkillAvailable = Color.white;
    private Color _colorSkillUnavailable = new(1, 1, 1, 0.2f);

    public static void Show()
    {
        BattleUIManager.Instance.ShowUI(nameof(BattleUI));
    }

    public static void Hide()
    {
        BattleUIManager.Instance.HideUI(nameof(BattleUI));
    }

    protected override void Awake()
    {
        base.Awake();

        ApplyDefaultLayout();

        _turns = new List<BattleTurn>();
        foreach (Transform children in turnContainer)
        {
            var o = children.gameObject;
            o.SetActive(false);
            _turns.Add(o.GetComponent<BattleTurn>());
        }
    }

    private void OnEnable()
    {
        this.AddListener(EventID.ON_ACTION_QUEUE_CHANGED, RefreshQueue);
        this.AddListener(EventID.ON_CURRENT_ENTITY_UPDATED, UpdateCurrentEntity);
        this.AddListener(EventID.ON_FIRE_SPIRIT_UPDATED, UpdateFireSpirit);
        this.AddListener(EventID.ON_FIRE_SPIRIT_PREVIEWED, PreviewFireSpirit);
        this.AddListener(EventID.ON_HEROES_SPAWNED, SetupUltimateSkills);
    }

    private void OnDisable()
    {
        this.RemoveListener(EventID.ON_ACTION_QUEUE_CHANGED, RefreshQueue);
        this.RemoveListener(EventID.ON_CURRENT_ENTITY_UPDATED, UpdateCurrentEntity);
        this.RemoveListener(EventID.ON_FIRE_SPIRIT_UPDATED, UpdateFireSpirit);
        this.RemoveListener(EventID.ON_FIRE_SPIRIT_PREVIEWED, PreviewFireSpirit);
        this.RemoveListener(EventID.ON_HEROES_SPAWNED, SetupUltimateSkills);
    }

    private void ApplyDefaultLayout()
    {
        actionMenu.SetActive(false);

        foreach (var img in imgUltimateList)
        {
            img.gameObject.SetActive(false);
        }
    }

    private void SetupUltimateSkills(object data)
    {
        var entity = (BattleEntity)data;

        var ultimateSkill = imgUltimateList[_ultimateIndex];
        var skillIcon = Common.GetSkillIcon(entity.EntityID, 2);
        var heroIcon = Common.GetSkillIcon("00", 0); //todo: review later
        ultimateSkill.gameObject.SetActive(true);
        ultimateSkill.Init(entity.UniqueID, skillIcon, heroIcon);
        _ultimateIndex++;
    }

    private void RefreshQueue(object data)
    {
        var queue = (List<Turn>)data;
        while (turnContainer.childCount < queue.Count)
        {
            var o = Instantiate(turnPref, turnContainer);
            _turns.Add(o);
        }

        for (int i = 0; i < _turns.Count; i++)
        {
            var turn = _turns[i];
            if (i >= queue.Count)
            {
                turn.gameObject.SetActive(false);
                turn.name = Constants.EMPTY_MARK;
                continue;
            }

            turn.gameObject.SetActive(true);
            turn.Init(queue[i]);

            bool isCurTurn = (i == 0);
            float offset = isCurTurn ? 0 : (_turnOffset0 + _turnOffset1 * (i - 1));
            turn.transform.localPosition = new Vector3(0, offset, 0);
            turn.transform.localScale = Vector3.one * (isCurTurn ? _curTurnScale : _normalTurnScale);
        }
    }

    private void UpdateCurrentEntity(object data)
    {
        var entity = (EntityController)data;

        if (entity.Entity.Side == Side.Ally)
        {
            actionMenu.SetActive(true);
            imgSkill.sprite = Common.GetSkillIcon(entity.Entity.EntityID, 1);
            imgSkill.color = BattleManager.Instance.HasFireSpirit() ? _colorSkillAvailable : _colorSkillUnavailable;

            EditorLog.Message("auto select attack");
            SelectAttack();
        }
        else
        {
            actionMenu.SetActive(false);
        }
    }

    private void UpdateFireSpirit(object data)
    {
        var (cur, max) = (Tuple<int, int>)data;
        imgMainFireSpirit.fillAmount = 1 - (max - cur) * fireSpiritStep;
        imgSubFireSpirit.fillAmount = 1 - (max - cur) * fireSpiritStep;
    }

    private void PreviewFireSpirit(object data)
    {
        // int diff = (int)data;
        // if (diff > 0)
        // {
        //     imgSubFireSpirit.fillAmount += fireSpiritStep * diff;
        // }
        // else
        // {
        //     imgMainFireSpirit.fillAmount += fireSpiritStep * diff;
        // }
    }

    public void SelectAttack()
    {
        BattleManager.Instance.RequestAttack();
    }

    public void SelectSpell()
    {
        BattleManager.Instance.RequestUseSkill();
    }

    public void SelectSkill()
    {
        if (!BattleManager.Instance.HasFireSpirit()) return;
        BattleManager.Instance.RequestUseUltimate();
    }

    public void SelectDismiss()
    {
        ActionQueue.Instance.EndTurn();
    }
}