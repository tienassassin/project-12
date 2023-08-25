using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private BattleEntity battleEntity;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image angerBar;

    private void OnEnable()
    {
        battleEntity.hpUpdated += UpdateHpBar;
        battleEntity.rageUpdated += UpdateAngerBar;
    }

    private void OnDisable()
    {
        battleEntity.hpUpdated -= UpdateHpBar;
        battleEntity.rageUpdated -= UpdateAngerBar;
    }

    private void UpdateHpBar(float curHp, float virtualHp, float maxHp)
    {
        var totalHp = curHp / virtualHp;
        hpBar.fillAmount = totalHp / Mathf.Max(totalHp, maxHp);
    }

    private void UpdateAngerBar(float anger)
    {
        angerBar.fillAmount = anger / 100;
    }
}
