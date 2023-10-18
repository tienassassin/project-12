using System;
using DG.Tweening;
using UnityEngine;

public class Projectile : RecyclableObject
{
    public void Move(Vector3 destination, float time, Action finish)
    {
        var seq = DOTween.Sequence();
        seq.Append(transform.DOMove(destination, time).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                finish?.Invoke();
                Recycle();
            });
    }
}