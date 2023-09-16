using System;
using DG.Tweening;
using UnityEngine;

public class Entity03 : Mecha
{
    protected override void PlayRangedAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        entityAnim.PlayAnimation(AnimationState.Attack, (t, e) =>
        {
            if (e.Data.Name.Equals("faqi_1"))
            {
                var o = ObjectPool.Instance.SpawnObject<Projectile>(entityRef.projectile, GetHitPosition());
                o.Move(hitPos, entityConfig.rangedMoveTime, () =>
                {
                    hitPhase?.Invoke();
                    regenPhase?.Invoke();
                });
            }
        }, t => { DOVirtual.DelayedCall(entityConfig.restTime, () => { finishPhase?.Invoke(); }); });
    }
}