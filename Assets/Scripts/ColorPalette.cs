using Sirenix.OdinInspector;
using UnityEngine;

public class ColorPalette : Singleton<ColorPalette>
{
    [ColorPalette(PaletteName = "Rarity")]
    public Color[] rarityColors;

    [ColorPalette(PaletteName = "Tier")]
    public Color[] tierColors;

    [ColorPalette(PaletteName = "Element")]
    public Color[] elementColors;

    public Color GetRarityColor(Rarity r)
    {
        return rarityColors[(int)r];
    }
    
    public string GetRarityColorHex(Rarity r)
    {
        return ColorUtility.ToHtmlStringRGBA(rarityColors[(int)r]);
    }
    
    public Color GetTierColor(Tier t)
    {
        return tierColors[(int)t];
    }
    
    public string GetTierColorHex(Tier t)
    {
        return ColorUtility.ToHtmlStringRGBA(tierColors[(int)t]);
    }
    
    public Color GetElementColor(Element e)
    {
        return elementColors[(int)e];
    }
    
    public string GetElementColorHex(Element e)
    {
        return ColorUtility.ToHtmlStringRGBA(elementColors[(int)e]);
    }
}