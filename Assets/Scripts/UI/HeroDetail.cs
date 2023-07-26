using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetail : DuztineBehaviour
{
    [SerializeField] private GameObject[] tabs;

    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private Image elementIcon;
    [SerializeField] private TMP_Text levelTxt;
    [SerializeField] private TMP_Text expTxt;
    [SerializeField] private Slider exp;
    [SerializeField] private Image race;
    [SerializeField] private Image damageType;
    [SerializeField] private TMP_Text aliasTxt;
    [SerializeField] private TMP_Text storyTxt;

    [SerializeField] private StatDetail[] statDetails;

    private BaseHero baseHero;
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

    public void Init(HeroSaveData saveData)
    {
        baseHero = saveData.GetHeroWithID();
        nameTxt.text = baseHero.name;
        level = saveData.GetLevel();
        (curExp, nextExp) = saveData.GetExp();
        
        levelTxt.text = level.ToString();
        bool levelMaxed = level >= Database.Instance.GetLevelMax();
        expTxt.text = levelMaxed ? "MAX" : $"EXP: {curExp} / {nextExp} ({curExp * 100 / nextExp}%)";
        exp.value = (float)curExp / nextExp;
        
        LoadStatsTab();
        LoadStoryTab();
    }

    private void LoadStatsTab()
    {
        baseStats = baseHero.stats;
        nonEqmStats = baseStats.GetStatsByLevel(level, baseHero.GetHeroGrowth());
        overallStats = nonEqmStats;

        foreach (var statDetail in statDetails)
        {
            statDetail.Init(baseStats, nonEqmStats, overallStats, baseHero.damageType);
        }
    }

    private void LoadStoryTab()
    {
        aliasTxt.text = $"[ {baseHero.name} - {baseHero.GetHeroAlias()} ]";
        storyTxt.text = baseHero.GetHeroStory();
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }
}