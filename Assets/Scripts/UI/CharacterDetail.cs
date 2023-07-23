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
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }
}