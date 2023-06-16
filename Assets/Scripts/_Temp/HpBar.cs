using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Image hpBar;
    [SerializeField] private Image angerBar;

    private void OnEnable()
    {
        character.OnUpdateHp += UpdateHpBar;
        character.OnUpdateAnger += UpdateAngerBar;
    }

    private void OnDisable()
    {
        character.OnUpdateHp -= UpdateHpBar;
        character.OnUpdateAnger -= UpdateAngerBar;
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
