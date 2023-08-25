using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class HeroCard : DuztineBehaviour
{
    protected Hero BaseData;
    protected float Energy;
    protected List<Equipment> EqmList = new();
    protected float Hp;

    [Title("BASE DATA")]
    protected HeroData SaveData;
    public int Level { get; private set; } = 1;
    public Tier Tier => BaseData.tier;
    public Element Element => BaseData.element;
    public Race Race => BaseData.race;

    public virtual void Init(HeroData saveData)
    {
        SaveData = saveData;
        BaseData = SaveData.GetHero();
        name = BaseData.name;
        Level = SaveData.GetLevel();
        Hp = SaveData.curHp;
        Energy = SaveData.energy;
    }
}