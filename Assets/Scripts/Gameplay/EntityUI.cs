using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EntityUI : DuztineBehaviour
{
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

    private BattleEntity _entity;

    private void Awake()
    {
        ResetAll();

        _entity = GetComponent<BattleEntity>();
        _entity.hpUpdated += UpdateHp;
        _entity.energyUpdated += UpdateEnergy;
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

    private void UpdateHp(float hp, float virtualHp, float maxHp)
    {
        // todo: add smoothness
        imgMainHp.fillAmount = hp / maxHp;
        imgMainVHp.fillAmount = virtualHp / maxHp;
    }

    private void UpdateEnergy(float energy, float maxEnergy)
    {
        // todo: add smoothness
        imgMainEnergy.fillAmount = energy / maxEnergy;
    }

    private void UpdateEffects()
    {
    }
}