using Sirenix.OdinInspector;
using UnityEngine;

public class EntityConfig : DuztineBehaviour
{
    [TitleGroup("Basic attack timing:")]
    [InfoBox("0: All / 1: Melee Config / 2: Ranged Config")]
    [SerializeField] [Range(0, 2)] private int viewMode;
    [ShowIf("@viewMode == 0 || viewMode == 1")]
    [Tooltip("The amount of time the entity moves from its current location to the target's location")]
    public float meleeMoveTime;
    [ShowIf("@viewMode == 0 || viewMode == 1")]
    [Tooltip("The amount of time the entity makes a melee attack")]
    public float meleeHitTime;
    [ShowIf("@viewMode == 0 || viewMode == 1")]
    [Tooltip("The amount of time the entity returns to its original position")]
    public float meleeReturnTime;
    [ShowIf("@viewMode == 0 || viewMode == 2")]
    [Tooltip("The amount of time the ranged attack moves")]
    public float rangedMoveTime;
    [Tooltip("Waiting time until end of turn")]
    public float restTime;
}