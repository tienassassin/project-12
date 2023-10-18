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

    [ColorPalette(PaletteName = "Race")]
    public Color[] raceColors;

    public Color GetRarityColor(Rarity r)
    {
        return rarityColors[(int)r];
    }
    
    public string GetRarityColorHex(Rarity r, bool hasPrefix = true)
    {
        string prefix = (hasPrefix ? "#" : "");
        return prefix + ColorUtility.ToHtmlStringRGBA(rarityColors[(int)r]);
    }
    
    public Color GetTierColor(Tier t)
    {
        return tierColors[(int)t];
    }
    
    public string GetTierColorHex(Tier t, bool hasPrefix = true)
    {
        string prefix = (hasPrefix ? "#" : "");
        return prefix + ColorUtility.ToHtmlStringRGBA(tierColors[(int)t]);
    }

    public Color GetElementColor(Role e)
    {
        return elementColors[(int)e];
    }

    public string GetElementColorHex(Role e, bool hasPrefix = true)
    {
        string prefix = (hasPrefix ? "#" : "");
        return prefix + ColorUtility.ToHtmlStringRGBA(elementColors[(int)e]);
    }

    public Color GetRaceColor(Realm r)
    {
        return raceColors[(int)r];
    }

    public string GetRaceColorHex(Realm r, bool hasPrefix = true)
    {
        string prefix = (hasPrefix ? "#" : "");
        return prefix + ColorUtility.ToHtmlStringRGBA(raceColors[(int)r]);
    }
}