public class BaseEquipmentCard : AssassinBehaviour
{
    // [TitleGroup("BASE DATA")]
    // protected Equipment SystemData;
    // protected int Level = 0;
    //
    // public void Init(string eqmId, int lv)
    // {
    //     SystemData = DataManager.Instance.GetEquipmentWithID(eqmId);
    //     Level = lv;
    // }
    //
    // public Stats GetStats(Race ownerRace)
    // {
    //     todo: review later
    //     var stats = baseEqm.stats * (baseEqm.race != ownerRace ? 1 : baseEqm.raceBonus);
    //     stats *= Mathf.Pow(baseEqm.GetEquipmentGrowth(), level);
    //     return stats;
    //     return default;
    // }
    //
    // public Stats GetEnhancement(Race ownerRace)
    // {
    //     todo: review later
    //     return GetStats(ownerRace) - baseEqm.stats;
    //     return default;
    // }
}