using Sirenix.OdinInspector;

public abstract class HeroCard : DuztineBehaviour
{
    protected EntityData EntityData;
    protected float Energy;
    // protected List<Equipment> EqmList = new();
    protected float Hp;

    [Title("BASE DATA")]
    protected EntitySaveData SaveData;
    public int Level { get; private set; } = 1;
    public Tier Tier => EntityData.info.tier;
    public Role Role => EntityData.info.role;
    public Realm Realm => EntityData.info.realm;

    public virtual void Init(EntitySaveData saveData)
    {
        SaveData = saveData;
        EntityData = SaveData.GetEntity();
        name = EntityData.name;
        Level = SaveData.GetLevel();
        Hp = SaveData.currentHp;
        Energy = SaveData.energy;
    }
}