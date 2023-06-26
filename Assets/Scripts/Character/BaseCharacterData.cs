using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Base Character Data", menuName = "Character/Base Character Data")]
public class BaseCharacterData : ScriptableObject
{
    public string characterName = "New Character";
    public Element element;
    public Race race;
    public Stats stats;
}

[Serializable]
public class CharacterData
{
    public BaseCharacterData baseData;
    public int level = 1;
    [TableList(ShowIndexLabels = true)]
    public List<EquipmentData> equipmentList = new();
}

