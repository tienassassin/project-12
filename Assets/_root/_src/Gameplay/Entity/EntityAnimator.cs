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

    private void Start()
    {
        PlayAnimation(AnimationState.Idle);
    }

    public void Flip()
    {
        skeleton.gameObject.transform.localScale = new Vector3(-1, 1, 1);
    }

    [Button]
    private void PlayAnimation(string animName, bool loop,
        Spine.AnimationState.TrackEntryEventDelegate @event = null,
        Spine.AnimationState.TrackEntryDelegate finish = null)
    {
        var te = skeleton.AnimationState.SetAnimation(0, animName, loop);
        te.Event += @event;
        te.Complete += finish;
    }

    public void PlayAnimation(AnimationState state,
        Spine.AnimationState.TrackEntryEventDelegate @event = null,
        Spine.AnimationState.TrackEntryDelegate finish = null)
    {
        switch (state)
        {
            case AnimationState.Idle:
                PlayAnimation(animIdle, true, @event, finish);
                break;

            case AnimationState.Attack:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animAttack, false, @event, finish);
                break;

            case AnimationState.Hit:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animHit, false, @event, finish);
                break;

            case AnimationState.Skill:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animSkill, false, @event, finish);
                break;

            case AnimationState.Ultimate:
                finish += _ => PlayAnimation(AnimationState.Idle);
                PlayAnimation(animUltimate, false, @event, finish);
                break;

            case AnimationState.Die:
                finish += _ => gameObject.SetActive(false);
                PlayAnimation(animDie, false, @event, finish);
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