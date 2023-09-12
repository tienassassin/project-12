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
    private void PlayAnimation(string animName, bool loop,
        Spine.AnimationState.TrackEntryDelegate finish = null,
        Spine.AnimationState.TrackEntryEventDelegate @event = null)
    {
        var te = skeleton.AnimationState.SetAnimation(0, animName, loop);
        te.Complete += finish;
        te.Event += @event;
    }

    public void PlayAnimation(AnimationState state,
        Spine.AnimationState.TrackEntryDelegate finish = null,
        Spine.AnimationState.TrackEntryEventDelegate @event = null)
    {
        switch (state)
        {
            case AnimationState.Idle:
                PlayAnimation(animIdle, true, finish, @event);
                break;

            case AnimationState.Attack:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animAttack, false, finish, @event);
                break;

            case AnimationState.Hit:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animHit, false, finish, @event);
                break;

            case AnimationState.Skill:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animSkill, false, finish, @event);
                break;

            case AnimationState.Ultimate:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animUltimate, false, finish, @event);
                break;

            case AnimationState.Die:
                finish += _ => gameObject.SetActive(false);
                PlayAnimation(animDie, false, finish, @event);
                break;
        }
    }
}

public enum AnimationState
{
    Idle,
    Attack,
    Hit,
    Skill,
    Ultimate,
    Die
}