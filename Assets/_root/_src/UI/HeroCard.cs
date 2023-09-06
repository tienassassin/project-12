using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class HeroCard : DuztineBehaviour
{
    protected Entity Info;
    protected float Energy;
    protected List<Equipment> EqmList = new();
    protected float Hp;

    [Title("BASE DATA")]
    protected EntitySaveData SaveData;
    public int Level { get; private set; } = 1;
    public Tier Tier => Info.tier;
    public Element Element => Info.element;
    public Race Race => Info.race;

    public virtual void Init(EntitySaveData saveData)
    {
        SaveData = saveData;
        Info = SaveData.GetEntity();
        name = Info.name;
        Level = SaveData.GetLevel();
        Hp = SaveData.currentHp;
        Energy = SaveData.energy;
    }
}