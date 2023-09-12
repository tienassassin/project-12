using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class HpText : RecyclableObject
{
    [SerializeField] private TMP_Text txtAmount;
    [SerializeField] private int fontSize0;
    [SerializeField] private int fontSize1;
    [SerializeField] [TableList] private List<HealthImpactColor> colors;

    public void Init(HealthImpactType type, string content)
    {
        txtAmount.text = content;
        txtAmount.color = colors.Find(x => x.type == type).color;
        txtAmount.fontStyle = type.IsCriticalHit() ? FontStyles.Bold : FontStyles.Normal;
        txtAmount.fontSize = type.IsCriticalHit() ? fontSize1 : fontSize0;
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        float duration = 2f;
        var newPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f);
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(newPos, duration))
            .Insert(1f, txtAmount.DOFade(0f, duration - 1f))
            .AppendCallback(Recycle);
    }
}

[Serializable]
public struct HealthImpactColor
{
    public HealthImpactType type;
    public Color color;
}