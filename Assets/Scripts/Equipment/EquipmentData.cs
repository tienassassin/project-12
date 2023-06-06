using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Data", menuName = "Equipment/Equipment Data")]
public class EquipmentData: ScriptableObject
{
    public EquipmentType type;
    public Races race;
    public Stats stats;
    public float levelBonus;
    public float raceBonus;
}