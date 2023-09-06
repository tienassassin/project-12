using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class EntityAutomation : DuztineBehaviour
{
    [SerializeField] private TargetSelectRate[] targetSelectRates;

    private EntityController _specificTarget;
    private BattleEntity _entity;
    private List<int> _prefixSum;

    private void Awake()
    {
        _entity = GetComponent<BattleEntity>();
    }

    public void SetSpecificTarget(EntityController entity)
    {
        _specificTarget = entity;
    }

    public EntityController GetTarget()
    {
        if (_specificTarget) return _specificTarget;
        int rand = Common.GetRandomResult(targetSelectRates.Select(x => x.rate).ToList());
        var condition = targetSelectRates[rand].condition;
        return EntityManager.Instance.GetEntity(_entity.Side.GetOpposite(), condition);
    }

    private void OnValidate()
    {
        if (targetSelectRates == null) return;

        int capacity = 100;
        for (int i = 0; i < targetSelectRates.Length; i++)
        {
            targetSelectRates[i].rate = Mathf.Clamp(targetSelectRates[i].rate, 0, capacity);
            capacity -= targetSelectRates[i].rate;
        }
    }
}

[Serializable]
public struct TargetSelectRate
{
    public TargetSelectCondition condition;
    [ProgressBar(0, 100)]
    public int rate;
}