using UnityEngine;

public class LineUpHeroCard : HeroCard
{
    [SerializeField] private GameObject readyPanel;

    public void ChangeState(bool ready)
    {
        readyPanel.SetActive(ready);
    }
}