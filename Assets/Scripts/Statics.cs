using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Get random result (true/false) from 0-100(%) rate
    /// </summary>
    public static bool GetRandomResult(float rate)
    {
        float num = Random.Range(0f,100f);
        return num <= rate;
    }
}

public static class StaticData
{
    private static Dictionary<string, string> statDescDic = new ()
    {
        ["health"] = "Khi máu của giảm về 0, bạn sẽ tử vong.",
        ["pDmg"] = "Sát thương vật lý, gây sát thương sẽ làm giảm Máu của kẻ địch.",
        ["mDmg"] = "Sát thương phép thuật, gây sát thương sẽ làm giảm Máu của kẻ địch.",
        ["armor"] = "Giáp, giảm Sát thương Vật lý phải chịu. Tối đa giảm xuống còn 1 sát thương.",
        ["magicResistance"] = "Kháng phép, giảm Sát thương phép thuật phải chịu. Tối đa giảm xuống còn 1 sát thương.",
        ["energyRegen"] = "(Max: 100) Năng lượng phục hồi sau mỗi đòn đánh cơ bản. Khi đầy Năng lượng (100), có thể dùng kĩ năng tối thượng.",
        
        ["speed"] = "(Max: 100) Tốc độ, quyết định thứ tự hành động. Khi đầy Nhanh nhẹn (100), lập tức có thêm một lượt",
        ["critRate"] = "(Max: 100) Tỉ lệ chí mạng, mỗi điểm tương đương 1% tỉ lệ gây đòn đánh chí mạng",
        ["critDmg"] = "Sát thương chí mạng, mỗi điểm tương đương 1% bội số sát thương của đòn đánh chí mạng",
        
        ["lifeSteal"] = "Hút máu, mỗi điểm tương đương 1% sát thương gây ra bởi đòn đánh cơ bản chuyển hóa thành Máu hồi phục",
        ["armorPenetration"] = "(Max: 80) Xuyên giáp, mỗi điểm tương đương 1% giáp của kẻ địch bị bỏ qua khi gây sát thương vật lý",
        ["mRPenetration"] = "(Max: 80) Xuyên kháng phép, mỗi điểm tương đương 1% kháng phép của kẻ địch bị bỏ qua khi gây sát thương phép thuật",
    };

    public static string GetStatDescription(string stat)
    {
        if (statDescDic.ContainsKey(stat)) return statDescDic[stat];
        Debug.LogError($"Stat {stat} is not defined!!!");
        return "";
    }

    private static Dictionary<Element, string> elementDescDic = new()
    {
        [Element.Fire] = "Hỏa",
        [Element.Ice] = "Băng",
        [Element.Wind] = "Phong",
        [Element.Thunder] = "Lôi",
    };

    public static string GetElementDescription(Element e)
    {
        if (elementDescDic.ContainsKey(e)) return elementDescDic[e];
        Debug.LogError($"Element {e} is not defined!!!");
        return "";
    }

    private static Dictionary<Faction, string> factionDescDic = new()
    {
        [Faction.Legion] = "Quân Đoàn",
        [Faction.Dynasty] = "Vương Triều",
        [Faction.Herald] = "Sứ Giả",
        [Faction.Cosmos] = "Hư không",
    };
}