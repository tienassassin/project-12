using UnityEngine;
using UnityEngine.UI;

public class BattleExtraTurn : AssassinBehaviour
{
    [SerializeField] private Image imgBackground;
    [SerializeField] private Image imgBanner;
    [SerializeField] private Sprite sprAlly;
    [SerializeField] private Sprite sprEnemy;
    [SerializeField] private Color colorAlly;
    [SerializeField] private Color colorEnemy;

    public void Init(TurnInfo turnInfo)
    {
        name = turnInfo.name;
        imgBackground.sprite = (turnInfo.side == Side.Ally ? sprAlly : sprEnemy);
        imgBackground.color = (turnInfo.side == Side.Ally ? colorAlly : colorEnemy);
        imgBanner.sprite = turnInfo.img;
    }
}