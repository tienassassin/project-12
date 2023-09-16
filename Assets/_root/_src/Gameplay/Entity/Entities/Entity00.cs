using System;
using DG.Tweening;
using UnityEngine;

public class Entity00 : Human
{
    protected override void PlayMeleeAnimation(Vector3 rootPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        rootPos += new Vector3(Side == Side.Ally ? -4 : 4, 0, 0);
        
        var origin = transform.position;
        transform.DOMove(rootPos, entityConfig.meleeMoveTime).OnComplete(() =>
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