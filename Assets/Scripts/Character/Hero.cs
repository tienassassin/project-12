using System.Collections.Generic;
using DB.System;
using Sirenix.OdinInspector;

public abstract class Hero : DuztineBehaviour
{
    public int Level { get; private set; } = 1;
    public Tier Tier => BaseData.Tier;
    public Element Element => BaseData.Element;
    public Race Race => BaseData.Race;
    
    [Title("BASE DATA")]
    protected DB.Player.Hero SaveData;
    protected DB.System.Hero BaseData;
    protected float Hp;
    protected float Energy;
    protected List<Equipment> EqmList = new();

    public virtual void Init(DB.Player.Hero saveData)
    {
        SaveData = saveData;
        BaseData = SaveData.GetHeroWithID();
        name = BaseData.Name;
        Level = SaveData.GetLevel();
        Hp = SaveData.curHp;
        Energy = SaveData.energy;
    }
}
