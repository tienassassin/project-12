using System.Collections.Generic;
using Sirenix.OdinInspector;

public abstract class Hero : DuztineBehaviour
{
    public int Level => level;
    public Tier Tier => baseHero.tier;
    public Element Element => baseHero.element;
    public Race Race => baseHero.race;
    
    [Title("BASE DATA")]
    protected HeroSaveData saveData;
    protected BaseHero baseHero;
    protected int level = 1;
    protected float curHP;
    protected float energy;
    protected List<Equipment> eqmList = new();

    public virtual void Init(HeroSaveData data)
    {
        saveData = data;
        baseHero = saveData.GetHeroWithID();
        name = baseHero.name;
        level = saveData.GetLevel();
        curHP = saveData.curHp;
        energy = saveData.energy;
    }
}
