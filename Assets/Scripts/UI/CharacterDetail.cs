using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDetail : DuztineBehaviour
{
    [SerializeField] private GameObject[] tabs;

    [SerializeField] private TextMeshProUGUI chrName;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private Slider exp;
    [SerializeField] private Image race;
    [SerializeField] private Image damageType;

    [SerializeField] private TextMeshProUGUI healthTxt;
    [SerializeField] private TextMeshProUGUI damageTxt;
    [SerializeField] private TextMeshProUGUI armorTxt;
    [SerializeField] private TextMeshProUGUI resistanceTxt;
    [SerializeField] private TextMeshProUGUI intelligenceTxt;
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI luckTxt;
    [SerializeField] private TextMeshProUGUI critDamageTxt;
    [SerializeField] private TextMeshProUGUI lifeStealTxt;
    [SerializeField] private TextMeshProUGUI accuracyTxt;

    [SerializeField] private StatDetail[] statDetails;

    private BaseCharacter baseChr;
    private int level;
    private int curExp;
    private int nextExp;

    private Stats baseStats;
    private Stats nonEqmStats;
    private Stats overallStats;

    private void OnEnable()
    {
        SwitchTab(0);
    }

    public void Init(CharacterSaveData saveData)
    {
        baseChr = Database.Instance.GetCharacterWithID(saveData.chrId);
        chrName.text = baseChr.name;
        level = saveData.GetLevel();
        (curExp, nextExp) = saveData.GetExp();
        
        levelTxt.text = level.ToString();
        bool levelMaxed = level >= Database.Instance.GetLevelMax();
        expTxt.text = levelMaxed ? "MAX" : $"EXP: {curExp} / {nextExp} ({curExp * 100 / nextExp}%)";
        exp.value = (float)curExp / nextExp;
        
        LoadStatsTab();
    }

    private void LoadStatsTab()
    {
        baseStats = baseChr.stats;
        nonEqmStats = baseStats.GetStatsByLevel(level, baseChr.GetCharacterGrowth());
        overallStats = nonEqmStats;

        foreach (var statDetail in statDetails)
        {
            statDetail.Init(baseStats, nonEqmStats, overallStats, baseChr.damageType);
        }
        
        var stats = baseChr.stats;
        healthTxt.text = Utils.GetIntString(overallStats.health);
        damageTxt.text = Utils.GetIntString(overallStats.damage);
        armorTxt.text = Utils.GetIntString(overallStats.armor);
        resistanceTxt.text = Utils.GetIntString(overallStats.resistance);
        intelligenceTxt.text = Utils.GetFloatString(overallStats.intelligence, 1);
        speedTxt.text = Utils.GetFloatString(overallStats.speed, 1);
        luckTxt.text = Utils.GetIntString(overallStats.luck);
        critDamageTxt.text = Utils.GetIntString(overallStats.critDamage);
        lifeStealTxt.text = Utils.GetFloatString(overallStats.lifeSteal, 1);
        accuracyTxt.text = Utils.GetFloatString(overallStats.accuracy, 1);
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}