using System;
using DB.System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroDetail : DuztineBehaviour
{
    [SerializeField] private GameObject[] tabs;

    [SerializeField] private TMP_Text txtName;
    [SerializeField] private Image imgElement;
    [SerializeField] private TMP_Text txtLevel;
    [SerializeField] private TMP_Text txtExp;
    [SerializeField] private Slider sldExp;
    [SerializeField] private Image imgRace;
    [SerializeField] private Image imgDamageType;
    [SerializeField] private TMP_Text txtAlias;
    [SerializeField] private TMP_Text txtStory;

    [SerializeField] private StatDetail[] statDetails;

    private DB.System.Hero _baseData;
    private int _level;
    private int _curExp;
    private int _nextExp;

    private Stats _baseStats;
    private Stats _nonEqmStats;
    private Stats _overallStats;

    private void OnEnable()
    {
        SwitchTab(0);
    }

    public void Init(DB.Player.Hero saveData)
    {
        _baseData = saveData.GetHeroWithID();
        txtName.text = _baseData.Name;
        _level = saveData.GetLevel();
        (_curExp, _nextExp) = saveData.GetExp();
        
        txtLevel.text = _level.ToString();
        bool levelMaxed = _level >= DataManager.Instance.GetLevelMax();
        txtExp.text = levelMaxed ? "MAX" : $"EXP: {_curExp} / {_nextExp} ({_curExp * 100 / _nextExp}%)";
        sldExp.value = (float)_curExp / _nextExp;
        
        LoadStatsTab();
        LoadStoryTab();
    }

    private void LoadStatsTab()
    {
        _baseStats = _baseData.Stats;
        _nonEqmStats = _baseStats.GetStatsByLevel(_level, _baseData.GetHeroGrowth());
        _overallStats = _nonEqmStats;

        foreach (var statDetail in statDetails)
        {
            statDetail.Init(_baseStats, _nonEqmStats, _overallStats, _baseData.DamageType);
        }
    }

    private void LoadStoryTab()
    {
        txtAlias.text = $"[ {_baseData.Name} - {_baseData.GetHeroAlias()} ]";
        txtStory.text = _baseData.GetHeroStory();
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }
}