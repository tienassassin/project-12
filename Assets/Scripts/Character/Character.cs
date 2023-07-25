using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Character : DuztineBehaviour
{
    public int Level => level;
    public Tier Tier => baseChr.tier;
    public Element Element => baseChr.element;
    public Race Race => baseChr.race;
    
    [Title("BASE DATA")]
    protected CharacterSaveData saveData;
    protected BaseCharacter baseChr;
    protected int level = 1;
    protected float curHP;
    protected float energy;
    protected List<Equipment> eqmList = new();

    public virtual void Init(CharacterSaveData data)
    {
        saveData = data;
        baseChr = saveData.GetCharacterWithID();
        name = baseChr.name;
        level = saveData.GetLevel();
        curHP = saveData.curHp;
        energy = saveData.energy;
    }
}
