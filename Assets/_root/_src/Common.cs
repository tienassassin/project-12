using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Common
{
    public static bool GetRandomResult(float rate)
    {
        return Random.value <= rate / 100f;
    }

    public static int GetRandomResult(List<float> list)
    {
        float rand = Random.value;
        float sum = 0;
        for (int i = 0; i < list.Count; i++)
        {
            sum += list[i] / 100f;
            if (rand <= sum) return i;
        }

        return 0;
    }

    public static T Parse<T>(string value)
        where T : struct, IComparable, IConvertible, IFormattable
    {
        if (typeof(T) == typeof(float))
        {
            var numStyle = NumberStyles.Float;
            var culture = new CultureInfo("en-US");
            float.TryParse(value, numStyle, culture, out var result);
            return (T)(object)result;
        }

        if (typeof(T) == typeof(int))
        {
            int.TryParse(value, out var result);
            return (T)(object)result;
        }

        return default;
    }

    public static string GetIntString(float num)
    {
        return ((int)num).ToString();
    }

    public static string GetFloatString(float num, int digit)
    {
        var culture = new CultureInfo("en-US");
        return Math.Round(num, digit).ToString(culture);
    }

    public static string GetNormalizedString(string raw)
    {
        string pattern = @"^\d+_([a-zA-Z]+)$";

        Match match = Regex.Match(raw, pattern);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        EditorLog.Error($"{raw} not match pattern {pattern}");
        return raw;
    }

    public static string GetTitleCaseString(string raw)
    {
        return Regex.Replace(raw, @"\b\w", match => match.Value.ToUpper());
    }

    private static string _skillIconPath = "Icons/Skill";

    public static Sprite GetSkillIcon(string heroId, int skillId)
    {
        return Resources.Load<Sprite>($"{_skillIconPath}/{heroId}/{skillId}");
    }
}

public static class RectTransformExtensions
{
    public static void SetRect(this RectTransform rt, float left, float right, float top, float bottom)
    {
        rt.SetLeft(left);
        rt.SetRight(right);
        rt.SetTop(top);
        rt.SetBottom(bottom);
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}