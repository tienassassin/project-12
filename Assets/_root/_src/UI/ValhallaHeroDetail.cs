using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValhallaHeroDetail : DuztineBehaviour
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

    [SerializeField] private ValhallaStatDetail[] statDetails;

    private Entity _info;

    private Stats _baseStats;
    private int _curExp;
    private int _level;
    private int _nextExp;
    private Stats _nonEqmStats;
    private Stats _overallStats;

    private void OnEnable()
    {
        SwitchTab(0);
    }

    public void Init(EntitySaveData saveData)
    {
        _info = saveData.GetEntity();
        txtName.text = _info.name;
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
        _baseStats = _info.stats;
        _nonEqmStats = _baseStats.GetStatsByLevel(_level, _info.GetEntityGrowth());
        _overallStats = _nonEqmStats;

        foreach (var statDetail in statDetails)
        {
            statDetail.Init(_baseStats, _nonEqmStats, _overallStats, _info.damageType);
        }
    }

    private void LoadStoryTab()
    {
        txtAlias.text = $"[ {_info.name} - {_info.GetEntityAlias()} ]";
        txtStory.text = _info.GetEntityStory();
    }

    public void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == index);
        }
    }
}