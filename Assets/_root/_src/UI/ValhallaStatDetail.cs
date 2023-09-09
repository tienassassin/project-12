using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ValhallaStatDetail : DuztineBehaviour, IPointerEnterHandler, IPointerExitHandler
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
                txtStatValue.text = Common.GetIntString(overallStats.health);
                baseValue = Common.GetIntString(baseStats.health);
                diff0Value = " (+" + Common.GetIntString(diff0.health) + ")";
                diff1Value = " (+" + Common.GetIntString(diff1.health) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "damage":
                txtStatValue.text = Common.GetIntString(overallStats.damage);
                baseValue = Common.GetIntString(baseStats.damage);
                diff0Value = " (+" + Common.GetIntString(diff0.damage) + ")";
                diff1Value = " (+" + Common.GetIntString(diff1.damage) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "armor":
                txtStatValue.text = Common.GetIntString(overallStats.armor);
                baseValue = Common.GetIntString(baseStats.armor);
                diff0Value = " (+" + Common.GetIntString(diff0.armor) + ")";
                diff1Value = " (+" + Common.GetIntString(diff1.armor) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "resistance":
                txtStatValue.text = Common.GetIntString(overallStats.resistance);
                baseValue = Common.GetIntString(baseStats.resistance);
                diff0Value = " (+" + Common.GetIntString(diff0.resistance) + ")";
                diff1Value = " (+" + Common.GetIntString(diff1.resistance) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "intelligence":
                txtStatValue.text = Common.GetFloatString(overallStats.intelligence, 1);
                baseValue = Common.GetFloatString(baseStats.intelligence, 1);
                diff0Value = Common.GetFloatString(diff0.intelligence, 1);
                diff1Value = " (+" + Common.GetFloatString(diff1.intelligence, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(Common.GetFloatString(DataManager.Instance.GetStatInfo(key).limit, 1));
                break;
            case "speed":
                txtStatValue.text = Common.GetFloatString(overallStats.speed, 1);
                baseValue = Common.GetFloatString(baseStats.speed, 1);
                diff0Value = Common.GetFloatString(diff0.speed, 1);
                diff1Value = " (+" + Common.GetFloatString(diff1.speed, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(Common.GetFloatString(DataManager.Instance.GetStatInfo(key).limit, 1));
                break;
            case "luck":
                txtStatValue.text = Common.GetIntString(overallStats.luck);
                baseValue = Common.GetIntString(baseStats.luck);
                diff0Value = Common.GetIntString(diff0.luck);
                diff1Value = " (+" + Common.GetIntString(diff1.luck) + ")";
                valueList.Add(txtStatValue.text);
                break;
            case "crit damage":
                txtStatValue.text = Common.GetIntString(overallStats.critDamage);
                baseValue = Common.GetIntString(baseStats.critDamage);
                diff0Value = Common.GetIntString(diff0.critDamage);
                diff1Value = " (+" + Common.GetIntString(diff1.critDamage) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "life steal":
                txtStatValue.text = Common.GetFloatString(overallStats.lifeSteal, 1);
                baseValue = Common.GetFloatString(baseStats.lifeSteal, 1);
                diff0Value = Common.GetFloatString(diff0.lifeSteal, 1);
                diff1Value = " (+" + Common.GetFloatString(diff1.lifeSteal, 1) + ")";
                valueList.Add(txtStatValue.text);
                valueList.Add(dmgTypeName);
                colorList.Add(dmgTypeColorHex);
                break;
            case "accuracy":
                txtStatValue.text = Common.GetFloatString(overallStats.accuracy, 1);
                baseValue = Common.GetFloatString(baseStats.accuracy, 1);
                diff0Value = Common.GetFloatString(diff0.accuracy, 1);
                diff1Value = " (+" + Common.GetFloatString(diff1.accuracy, 1) + ")";
                valueList.Add(txtStatValue.text);
                break;
        }

        if (!diff0Value.Contains("+")) diff0Value = "";

        txtStatDetailValue.text = $"{baseValue}" +
                                  $"<color={_hexColorDiff0}>{diff0Value}</color>" +
                                  $"<color={_hexColorDiff1}>{diff1Value}</color>";

        string rawDesc = DataManager.Instance.GetStatInfo(key).description;
        for (int i = 0; i < valueList.Count; i++)
        {
            rawDesc = rawDesc.Replace($"#value{i}", $"{valueList[i]}");
        }

        for (int i = 0; i < colorList.Count; i++)
        {
            rawDesc = rawDesc.Replace($"#color{i}", $"{colorList[i]}");
        }

        txtDescription.text = rawDesc;
        txtTitle.text = Common.GetTitleCaseString(DataManager.Instance.GetStatInfo(key).name);
    }

    private IEnumerator ShowDetailPanel()
    {
        yield return new WaitForSecondsRealtime(delayShowPanel);
        detailPanel.SetActive(true);
    }
}