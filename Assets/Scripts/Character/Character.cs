using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Character : DuztineBehavior
{
    [Title("BASE DATA")]
    protected BaseCharacter baseChr;
    protected int level = 1;
    protected List<Equipment> eqmList = new();
}
