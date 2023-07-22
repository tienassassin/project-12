using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatDetail : DuztineBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string key;
    [SerializeField] private TextMeshProUGUI statValueTxt;
    [SerializeField] private TextMeshProUGUI statDetailValueTxt;
    [SerializeField] private Color diff0Color;
    [SerializeField] private Color diff1Color;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private float delayShowPanel = 0.5f;
    private string diff0ColorHex;
    private string diff1ColorHex;
    private Coroutine c;

    private void Awake()
    {
        diff0ColorHex = ColorUtility.ToHtmlStringRGBA(diff0Color);
        diff1ColorHex = ColorUtility.ToHtmlStringRGBA(diff1Color);
    }

    public void Init(Stats baseStats, Stats nonEqmStats, Stats overallStats)
    {
        var diff0 = nonEqmStats - baseStats;
        var diff1 = overallStats - nonEqmStats;
        string baseValue = "";
        string diff0Value = "";
        string diff1Value = "";

        switch (key)
        {
            case "health":
                statValueTxt.text = Utils.GetIntString(overallStats.health);
                baseValue = Utils.GetIntString(baseStats.health);
                diff0Value = " (+" + Utils.GetIntString(diff0.health) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.health) + ")";
                break;
            case "damage":
                statValueTxt.text = Utils.GetIntString(overallStats.damage);
                baseValue = Utils.GetIntString(baseStats.damage);
                diff0Value = " (+" + Utils.GetIntString(diff0.damage) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.damage) + ")";
                break;
            case "armor":
                statValueTxt.text = Utils.GetIntString(overallStats.armor);
                baseValue = Utils.GetIntString(baseStats.armor);
                diff0Value = " (+" + Utils.GetIntString(diff0.armor) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.armor) + ")";
                break;
            case "resistance":
                statValueTxt.text = Utils.GetIntString(overallStats.resistance);
                baseValue = Utils.GetIntString(baseStats.resistance);
                diff0Value = " (+" + Utils.GetIntString(diff0.resistance) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.resistance) + ")";
                break;
            case "intelligence":
                statValueTxt.text = Utils.GetFloatString(overallStats.intelligence, 1);
                baseValue = Utils.GetFloatString(baseStats.intelligence, 1);
                diff0Value = Utils.GetFloatString(diff0.intelligence, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.intelligence, 1) + ")";
                break;
            case "speed":
                statValueTxt.text = Utils.GetFloatString(overallStats.speed, 1);
                baseValue = Utils.GetFloatString(baseStats.speed, 1);
                diff0Value = Utils.GetFloatString(diff0.speed, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.speed, 1) + ")";
                break;
            case "luck":
                statValueTxt.text = Utils.GetIntString(overallStats.luck);
                baseValue = Utils.GetIntString(baseStats.luck);
                diff0Value = Utils.GetIntString(diff0.luck);
                diff1Value = " (+" + Utils.GetIntString(diff1.luck) + ")";
                break;
            case "crit damage":
                statValueTxt.text = Utils.GetIntString(overallStats.critDamage);
                baseValue = Utils.GetIntString(baseStats.critDamage);
                diff0Value = Utils.GetIntString(diff0.critDamage);
                diff1Value = " (+" + Utils.GetIntString(diff1.critDamage) + ")";
                break;
            case "life steal":
                statValueTxt.text = Utils.GetFloatString(overallStats.lifeSteal, 1);
                baseValue = Utils.GetFloatString(baseStats.lifeSteal, 1);
                diff0Value = Utils.GetFloatString(diff0.lifeSteal, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.lifeSteal, 1) + ")";
                break;
            case "accuracy":
                statValueTxt.text = Utils.GetFloatString(overallStats.accuracy, 1);
                baseValue = Utils.GetFloatString(baseStats.accuracy, 1);
                diff0Value = Utils.GetFloatString(diff0.accuracy, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.accuracy, 1) + ")";
                break;
        }

        if (!diff0Value.Contains("+")) diff0Value = "";

        statDetailValueTxt.text = $"{baseValue}" +
                            $"<color=#{diff0ColorHex}>{diff0Value}</color>" +
                            $"<color=#{diff1ColorHex}>{diff1Value}</color>";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        c = StartCoroutine(ShowDetailPanel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (c != null) StopCoroutine(c);
        detailPanel.SetActive(false);
    }

    IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSecondsRealtime(delayShowPanel);
        detailPanel.SetActive(true);
    }
}