using UnityEngine;

public class LineUpHeroCard : HeroCard
{
    [SerializeField] private GameObject readyPanel;

    public void UpdateReadyState()
    {
        if (name == Constants.EMPTY_MARK) return;
        bool ready = UserManager.Instance.IsHeroReady(saveData.heroId);
        readyPanel.SetActive(ready);
    }
}