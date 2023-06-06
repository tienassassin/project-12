using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character/Character Data")]
public class CharacterData: ScriptableObject
{
    public Elements element;
    public List<Elements> weakness;
    public Races race;
    public Stats stats;
}