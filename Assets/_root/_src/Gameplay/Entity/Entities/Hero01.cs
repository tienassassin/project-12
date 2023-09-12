using System;
using DG.Tweening;
using UnityEngine;

public class Hero01 : Human
{
    protected override void PlayRangedAnimation(Vector3 hitPos, Action hitPhase, Action regenPhase, Action finishPhase)
    {
        Animator.PlayAnimation(AnimationState.Attack, (t, e) =>
        {
            if (e.Data.Name.Equals("faqi_1"))
            {
                var o = ObjectPool.Instance.SpawnObject<Projectile>(Ref.projectile, GetHitPosition());
                o.Move(hitPos, Config.rangedMoveTime, () =>
                {
                    hitPhase?.Invoke();
                    regenPhase?.Invoke();
                });
            }
        }, t => { DOVirtual.DelayedCall(Config.restTime, () => { finishPhase?.Invoke(); }); });
    }
}