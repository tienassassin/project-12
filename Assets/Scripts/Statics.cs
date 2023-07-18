using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

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
}

public static class StaticData
{
    private static Dictionary<Element, string> elementDescDict = new()
    {
        [Element.Fire] = "Hỏa",
        [Element.Ice] = "Băng",
        [Element.Wind] = "Phong",
        [Element.Thunder] = "Lôi",
    };

    public static string GetElementDescription(Element e)
    {
        if (elementDescDict.ContainsKey(e)) return elementDescDict[e];
        EditorLog.Error($"Element {e} is not defined!!!");
        return "";
    }
}