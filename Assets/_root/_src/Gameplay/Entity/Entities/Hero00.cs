using System;
using DG.Tweening;
using UnityEngine;

public class Hero00 : Human
{
    protected override void PlayMeleeAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        var origin = transform.position;
        transform.DOMove(hitPos, entityConfig.meleeMoveTime).OnComplete(() =>
        {
            entityAnim.PlayAnimation(AnimationState.Attack, (t, e) =>
            {
                if (e.Data.Name.Equals("faqi_1"))
                {
                    hitPhase?.Invoke();
                }
            }, t =>
            {
                transform.DOMove(origin, entityConfig.meleeReturnTime).OnComplete(() =>
                {
                    regenPhase?.Invoke();
                    DOVirtual.DelayedCall(entityConfig.restTime, () => { finishPhase?.Invoke(); });
                });
            });
        });
    }
}