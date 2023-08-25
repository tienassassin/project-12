using UnityEngine;
using UnityEngine.UI;

public class HeroAvatar : DuztineBehaviour
{
    [SerializeField] private Image imgAvatar;
    [SerializeField] private Image imgHp;
    [SerializeField] private Image imgEnergy;
    private Hero _baseData;

    private HeroData _saveData;

    public void Init(HeroData data)
    {
        _saveData = data;
        _baseData = _saveData?.GetHeroWithID();
        name = (_baseData != null ? _baseData.name : Constants.EMPTY_MARK);

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

        imgHp.fillAmount = (_saveData.curHp / _baseData.stats.health) / 2;
        imgEnergy.fillAmount = (_saveData.energy / 100) / 2;
    }

    public void OnClickAvatar()
    {
        if (name == Constants.EMPTY_MARK) return;

        LineUpUI.Show();
    }
}