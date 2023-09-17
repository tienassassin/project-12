using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDatabase", menuName = "DB/EntityDatabase")]
public class EntityDatabase : ScriptableDatabase
{
    public List<EntityData> entities = new();

    public override void Import()
    {
    }

    public override void Delete()
    {
        entities.Clear();
    }

    public List<EntityData> GetAllEntities()
    {
        return entities.Where(x => x.canUnlock).ToList();
    }

    public EntityData GetEntityWithID(string id)
    {
        return entities.Find(x => x.info.id.Equals(id));
    }

    public List<EntityData> GetEntitiesWithConditions(params object[] conditions)
    {
        var raceOpts = new List<Realm>();
        var elementOpts = new List<Role>();
        var tierOpts = new List<Tier>();

        foreach (var condition in conditions)
        {
            switch (condition)
            {
                case Realm race:
                    raceOpts.Add(race);
                    break;
                case Role element:
                    elementOpts.Add(element);
                    break;
                case Tier tier:
                    tierOpts.Add(tier);
                    break;
            }
        }

        return entities.Where(x =>
            (
                (raceOpts.Contains(x.info.realm) || raceOpts.Count < 1) && // race filter
                (elementOpts.Contains(x.info.role) || elementOpts.Count < 1) && // element filter
                (tierOpts.Contains(x.info.tier) || tierOpts.Count < 1) // tier filter
            ))
            .ToList();
    }
}