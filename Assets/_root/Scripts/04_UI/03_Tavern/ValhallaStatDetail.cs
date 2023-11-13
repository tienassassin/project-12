using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValhallaStatDetail : AssassinBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string key;
    [SerializeField] private TMP_Text txtTitle;
    [SerializeField] private TMP_Text txtDescription;
    [SerializeField] private TMP_Text txtStatValue;
    [SerializeField] private TMP_Text txtStatDetailValue;
    [SerializeField] private Color colorDiff0;
    [SerializeField] private Color colorDiff1;
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private float delayShowPanel = 0.5f;
    private Coroutine _coroutine;

    private string _hexColorDiff0;
    private string _hexColorDiff1;

    private void Awake()
    {
        detailPanel.SetActive(false);
        _hexColorDiff0 = "#" + ColorUtility.ToHtmlStringRGBA(colorDiff0);
        _hexColorDiff1 = "#" + ColorUtility.ToHtmlStringRGBA(colorDiff1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _coroutine = StartCoroutine(ShowDetailPanel());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        detailPanel.SetActive(false);
    }

    public void Init(Stats baseStats, Stats nonEqmStats, Stats overallStats, DamageType dmgType)
    {
        var diff0 = nonEqmStats - baseStats;
        var diff1 = overallStats - nonEqmStats;
        string baseValue = "";
        string diff0Value = "";
        string diff1Value = "";
        List<string> valueList = new();
        List<string> colorList = new();
        string dmgTypeName = dmgType != DamageType.Physical ? "ma thuật" : "vật lý";
        string dmgTypeColorHex = dmgType != DamageType.Physical ? "#854DFF" : "#FFAA33";

        switch (key)
        {
            case "health":
                txtStatValue.text = Utils.GetIntString(overallStats.health);
                baseValue = Utils.GetIntString(baseStats.health);
                diff0Value = " (+" + Utils.GetIntString(diff0.health) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.health) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "damage":
                txtStatValue.text = Utils.GetIntString(overallStats.damage);
                baseValue = Utils.GetIntString(baseStats.damage);
                diff0Value = " (+" + Utils.GetIntString(diff0.damage) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.damage) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "armor":
                txtStatValue.text = Utils.GetIntString(overallStats.armor);
                baseValue = Utils.GetIntString(baseStats.armor);
                diff0Value = " (+" + Utils.GetIntString(diff0.armor) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.armor) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "resistance":
                txtStatValue.text = Utils.GetIntString(overallStats.resistance);
                baseValue = Utils.GetIntString(baseStats.resistance);
                diff0Value = " (+" + Utils.GetIntString(diff0.resistance) + ")";
                diff1Value = " (+" + Utils.GetIntString(diff1.resistance) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "intelligence":
                txtStatValue.text = Utils.GetFloatString(overallStats.intelligence, 1);
                baseValue = Utils.GetFloatString(baseStats.intelligence, 1);
                diff0Value = Utils.GetFloatString(diff0.intelligence, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.intelligence, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(Utils.GetFloatString(GameDatabase.Instance.GetStatInfo(key).limit, 1));
                break;
            case "speed":
                txtStatValue.text = Utils.GetFloatString(overallStats.speed, 1);
                baseValue = Utils.GetFloatString(baseStats.speed, 1);
                diff0Value = Utils.GetFloatString(diff0.speed, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.speed, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(Utils.GetFloatString(GameDatabase.Instance.GetStatInfo(key).limit, 1));
                break;
            case "luck":
                txtStatValue.text = Utils.GetIntString(overallStats.luck);
                baseValue = Utils.GetIntString(baseStats.luck);
                diff0Value = Utils.GetIntString(diff0.luck);
                diff1Value = " (+" + Utils.GetIntString(diff1.luck) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "crit damage":
                txtStatValue.text = Utils.GetIntString(overallStats.critDamage);
                baseValue = Utils.GetIntString(baseStats.critDamage);
                diff0Value = Utils.GetIntString(diff0.critDamage);
                diff1Value = " (+" + Utils.GetIntString(diff1.critDamage) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "life steal":
                txtStatValue.text = Utils.GetFloatString(overallStats.lifeSteal, 1);
                baseValue = Utils.GetFloatString(baseStats.lifeSteal, 1);
                diff0Value = Utils.GetFloatString(diff0.lifeSteal, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.lifeSteal, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "accuracy":
                txtStatValue.text = Utils.GetFloatString(overallStats.accuracy, 1);
                baseValue = Utils.GetFloatString(baseStats.accuracy, 1);
                diff0Value = Utils.GetFloatString(diff0.accuracy, 1);
                diff1Value = " (+" + Utils.GetFloatString(diff1.accuracy, 1) + ")";
                valueList.Add(txtStatValue.text);
                break;
        }

        if (!diff0Value.Contains("+")) diff0Value = "";

        txtStatDetailValue.text = $"{baseValue}" +
                                  $"<color={_hexColorDiff0}>{diff0Value}</color>" +
                                  $"<color={_hexColorDiff1}>{diff1Value}</color>";

        var rawDesc = GameDatabase.Instance.GetStatInfo(key).description;
        for (int i = 0; i < valueList.Count; i++)
        {
            rawDesc = rawDesc.Replace($"#value{i}", $"{valueList[i]}");
        }

        for (int i = 0; i < colorList.Count; i++)
        {
            rawDesc = rawDesc.Replace($"#color{i}", $"{colorList[i]}");
        }

        txtDescription.text = rawDesc;
        txtTitle.text = Utils.GetTitleCaseString(GameDatabase.Instance.GetStatInfo(key).name);
    }

    private IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSecondsRealtime(delayShowPanel);
        detailPanel.SetActive(true);
    }
}