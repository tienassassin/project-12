using Sirenix.OdinInspector;
using UnityEngine;

public class EntityReferenceHolder : AssassinBehaviour
{
    [TitleGroup("Hp Impact:")]
    public Transform hpTextPos;
    public HpText hpTextPrefab;

    [TitleGroup("Hit positions:")]
    public Transform hitPos;
    public Transform rootPos;

    [TitleGroup("Basic attack vfx:")]
    public Projectile projectile;
}