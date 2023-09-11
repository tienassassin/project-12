using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class EntityAnimator : DuztineBehaviour
{
    [SerializeField] private SkeletonAnimation skeleton;
    [SpineAnimation] [SerializeField] private string animIdle;
    [SpineAnimation] [SerializeField] private string animAttack;
    [SpineAnimation] [SerializeField] private string animHit;
    [SpineAnimation] [SerializeField] private string animSkill;
    [SpineAnimation] [SerializeField] private string animUltimate;
    [SpineAnimation] [SerializeField] private string animDie;

    public void Flip()
    {
        skeleton.gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    [Button]
    private void PlayAnimation(string animName, bool loop, Action finish = null)
    {
        var te = skeleton.AnimationState.SetAnimation(0, animName, loop);
        te.Complete += _ => { finish?.Invoke(); };
    }
}