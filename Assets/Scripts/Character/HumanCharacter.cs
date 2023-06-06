using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HumanCharacter : Character
{
    public string target;
    
    [Button]
    public void TestAttack()
    {
        Attack(GameObject.Find(target).GetComponent<Character>());
    }

    [Button]
    public void TestRegen(int amount, bool overflow)
    {
        RegenHP(amount, overflow);
    }
}
