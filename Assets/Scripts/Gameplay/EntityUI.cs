using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI : DuztineBehaviour
{
    [TitleGroup("Colors:")]
    [SerializeField] private Color colorPositive;
    [SerializeField] private Color colorNegative;

    [TitleGroup("HP:")]
    [SerializeField] private Image imgMainHp;
    [SerializeField] private Image imgSubHp;
    [SerializeField] private Image imgMainVHp;
    [SerializeField] private Image imgSubVHp;

    [TitleGroup("Energy:")]
    [SerializeField] private Image imgMainEnergy;
    [SerializeField] private Image imgSubEnergy;

    [TitleGroup("Effect:")]
    [SerializeField] private Transform effectContainer;

    [TitleGroup("MARKS:")]
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject focus;

    private BattleEntity _entity;

    private float _fillDelay = 0.5f;
    private Ease _ease = Ease.InCubic;
    private Sequence _hpSeq;
    private Sequence _vHpSeq;
    private Sequence _energySeq;

    private void Awake()
    {
        ResetAll();

        _entity = GetComponent<BattleEntity>();
        _entity.hpUpdated += UpdateHp;
        _entity.energyUpdated += UpdateEnergy;
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

    private void ResetAll()
    {
        imgMainHp.fillAmount = 0;
        imgSubHp.fillAmount = 0;
        imgMainVHp.fillAmount = 0;
        imgSubVHp.fillAmount = 0;
        imgMainEnergy.fillAmount = 0;
        imgSubEnergy.fillAmount = 0;
    }

    public void OnTakeTurn(object id)
    {
        highlight.SetActive(_entity.ID == (int)id);
    }

    public void OnFocused(object data)
    {
        if (data == null)
        {
            focus.SetActive(false);
            return;
        }

        var (targetType, id) = (Tuple<SkillTargetType, int>)data;
        bool isFocused = false;

        switch (targetType)
        {
            case SkillTargetType.Ally:
                isFocused = _entity.Faction == Faction.Hero && _entity.ID != id;
                break;
            case SkillTargetType.AllyOrSelf:
                isFocused = _entity.Faction == Faction.Hero;
                break;
            case SkillTargetType.Enemy:
                isFocused = _entity.Faction == Faction.Devil;
                break;
            case SkillTargetType.EnemyOrSelf:
                isFocused = _entity.Faction == Faction.Devil || _entity.ID == id;
                break;
            case SkillTargetType.ExceptSelf:
                isFocused = _entity.Faction == Faction.Devil || (_entity.Faction == Faction.Hero && _entity.ID != id);
                break;
            case SkillTargetType.All:
                isFocused = true;
                break;
        }

        focus.SetActive(isFocused);
    }

    private void UpdateHp(float hp, float virtualHp, float maxHp, float duration)
    {
        float lastHpPct = imgMainHp.fillAmount;
        float lastVHpPct = imgMainVHp.fillAmount;
        float hpPct = hp / maxHp;
        float vHpPct = virtualHp / maxHp;

        if (hpPct > lastHpPct)
        {
            // healing
            imgSubHp.color = colorPositive;

            _hpSeq?.Kill();
            _hpSeq = DOTween.Sequence();
            _hpSeq.AppendCallback(() => { imgSubHp.fillAmount = hpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgMainHp.DOFillAmount(hpPct, duration));
        }
        else if (hpPct < lastHpPct)
        {
            // dmg
            imgSubHp.color = colorNegative;

            _hpSeq?.Kill();
            _hpSeq = DOTween.Sequence();
            _hpSeq.AppendCallback(() => { imgMainHp.fillAmount = hpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgSubHp.DOFillAmount(hpPct, duration));
        }

        if (vHpPct > lastVHpPct)
        {
            // shield
            imgSubVHp.color = colorPositive;

            _vHpSeq?.Kill();
            _vHpSeq = DOTween.Sequence();
            _vHpSeq.AppendCallback(() => { imgSubVHp.fillAmount = vHpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgMainVHp.DOFillAmount(vHpPct, duration));
        }
        else if (vHpPct < lastVHpPct)
        {
            // dmg
            imgSubVHp.color = colorNegative;

            _vHpSeq?.Kill();
            _vHpSeq = DOTween.Sequence();
            _vHpSeq.AppendCallback(() => { imgMainVHp.fillAmount = vHpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgSubVHp.DOFillAmount(vHpPct, duration));
        }
    }

    private void UpdateEnergy(float energy, float maxEnergy, float duration)
    {
        float lastEnergyPct = imgMainEnergy.fillAmount;
        float energyPct = energy / maxEnergy;

        if (energyPct > lastEnergyPct)
        {
            _energySeq?.Kill();
            _energySeq = DOTween.Sequence();
            _energySeq.AppendCallback(() => { imgSubEnergy.fillAmount = energyPct; })
                .Append(imgMainEnergy.DOFillAmount(energyPct, duration));
        }
        else if (energyPct < lastEnergyPct)
        {
            _energySeq?.Kill();
            _energySeq = DOTween.Sequence();
            _energySeq.AppendCallback(() => { imgMainEnergy.fillAmount = energyPct; })
                .Append(imgSubEnergy.DOFillAmount(energyPct, duration));
        }
    }

    private void UpdateEffects()
    {
    }
}