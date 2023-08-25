using UnityEngine;
using UnityEngine.UI;

public class HeroAvatar : HeroCard
{
    [SerializeField] private Image imgAvatar;
    [SerializeField] private Image imgHp;
    [SerializeField] private Image imgEnergy;

    public override void Init(HeroData data)
    {
        base.Init(data);
        name = (BaseData != null ? BaseData.name : Constants.EMPTY_MARK);

        Refresh();
    }

    private void Refresh()
    {
        if (name == Constants.EMPTY_MARK)
        {
            imgHp.fillAmount = 0;
            imgEnergy.fillAmount = 0;
            return;
        }

        imgHp.fillAmount = (SaveData.curHp / BaseData.stats.health) / 2;
        imgEnergy.fillAmount = (SaveData.energy / 100) / 2;
    }

    public void OnClickAvatar()
    {
        if (name == Constants.EMPTY_MARK) return;

        LineUpUI.Show();
    }
}