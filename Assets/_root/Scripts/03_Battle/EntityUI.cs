using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI : AssassinBehaviour
{
    [TitleGroup("Colors:")]
    [SerializeField] private Color colorPositive;
    [SerializeField] private Color colorNegative;

    [TitleGroup("HP:")]
    [SerializeField] private Image imgMainHp;
    [SerializeField] private Image imgSubHp;
    [SerializeField] private Image imgMainVHp;
    [SerializeField] private Image imgSubVHp;
    [SerializeField] private float hpAmountPerSegment = 100;
    [SerializeField] private GameObject segmentLinePref;
    [SerializeField] private Transform segmentLineContainer;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [TitleGroup("Energy:")]
    [SerializeField] private Image imgMainEnergy;
    [SerializeField] private Image imgSubEnergy;

    [TitleGroup("Effect:")]
    [SerializeField] private Transform effectContainer;

    [TitleGroup("Marks:")]
    [SerializeField] private GameObject highlight;
    [SerializeField] private GameObject focus;
    [SerializeField] private GameObject actionMenu;

    private EntityController _controller;

    private float _fillDelay = 0.5f;
    private Sequence _hpSeq;
    private Sequence _vHpSeq;
    private Sequence _energySeq;

    private void Awake()
    {
        ApplyDefaultLayout();
        ResetAll();

        _controller = GetComponent<EntityController>();
    }

    private void ApplyDefaultLayout()
    {
        highlight.SetActive(false);
        focus.SetActive(false);
        actionMenu.SetActive(false);
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

    public void SwitchHighlight(bool active)
    {
        highlight.SetActive(active);
    }

    public void SwitchFocus(bool active)
    {
        focus.SetActive(active);
    }

    public void SetupHpSegment(float maxHp)
    {
        int lineCount = (int)(maxHp / hpAmountPerSegment);
        if (lineCount < 1) return;
        Vector3 lengthPerHp = (rightLimit.position - leftLimit.position) / maxHp;
        for (int i = 0; i < lineCount; i++)
        {
            var o = Instantiate(segmentLinePref, segmentLineContainer);
            o.SetActive(true);
            o.transform.position = leftLimit.position + lengthPerHp * hpAmountPerSegment * (i + 1);
        }
    }

    public void UpdateHp(float hp, float virtualHp, float maxHp, float duration)
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
                .Append(imgMainHp.DOFillAmount(hpPct, duration))
                .Append(imgSubHp.DOColor(Color.clear, 0.25f));
        }
        else if (hpPct < lastHpPct)
        {
            // dmg
            imgSubHp.color = colorNegative;

            _hpSeq?.Kill();
            _hpSeq = DOTween.Sequence();
            _hpSeq.AppendCallback(() => { imgMainHp.fillAmount = hpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgSubHp.DOFillAmount(hpPct, duration))
                .Append(imgSubHp.DOColor(Color.clear, 0.25f));
        }

        if (vHpPct > lastVHpPct)
        {
            // shield
            // imgSubVHp.color = colorPositive;

            _vHpSeq?.Kill();
            _vHpSeq = DOTween.Sequence();
            _vHpSeq.AppendCallback(() => { imgSubVHp.fillAmount = vHpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgMainVHp.DOFillAmount(vHpPct, duration));
        }
        else if (vHpPct < lastVHpPct)
        {
            // dmg
            // imgSubVHp.color = colorNegative;

            _vHpSeq?.Kill();
            _vHpSeq = DOTween.Sequence();
            _vHpSeq.AppendCallback(() => { imgMainVHp.fillAmount = vHpPct; })
                .AppendInterval(_fillDelay)
                .Append(imgSubVHp.DOFillAmount(vHpPct, duration));
        }
    }

    public void UpdateEnergy(float energy, float maxEnergy, float duration)
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

    public void UpdateEffects()
    {
    }

    public void SwitchActionPanel(bool active)
    {
        actionMenu.SetActive(active);
    }

    public void ConfirmAction()
    {
        // EditorLog.Message("Action confirmed!");
        BattleManager.Instance.ConfirmAction();
    }

    public void CancelAction()
    {
        // EditorLog.Message("Action cancelled!");
        BattleManager.Instance.SelectEntity(null);
    }
}