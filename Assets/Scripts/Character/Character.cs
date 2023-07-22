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
        baseChr = Database.Instance.GetCharacterWithID(saveData.chrId);
        name = baseChr.name;
        level = saveData.GetLevel();
        curHP = saveData.curHp;
        energy = saveData.energy;
    }
}

[Serializable]
public struct CharacterSaveData
{
    public string chrId;
    public int totalExp;
    public float curHp;
    public float energy;
    public List<EquipmentSaveData> eqmList;
}

[Serializable]
public struct EquipmentSaveData
{
    public string eqmId;
    public int level;
}
