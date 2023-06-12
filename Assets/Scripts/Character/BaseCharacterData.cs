using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Character Data", menuName = "Character/Base Character Data")]
public class BaseCharacterData : ScriptableObject
{
    public string characterName = "New Character";
    public Element element;
    public Faction faction;
    public Stats stats;
}

[Serializable]
public class CharacterData
{
    public BaseCharacterData baseData;
    public List<EquipmentData> equipmentList = new();
    public int level = 1;
}

