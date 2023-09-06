using Sirenix.OdinInspector;
using UnityEngine;

public class EntityReferenceHolder : DuztineBehaviour
{
    [TitleGroup("Hp Impact:")]
    public Transform hpTextPos;
    public HpText hpTextPrefab;

    [TitleGroup("Hit positions:")]
    public Transform leftHitPos;
    public Transform rightHitPos;

    [TitleGroup("Basic attack vfx:")]
    public Projectile projectile;
}