using System;
using Sirenix.OdinInspector;

public class BattleManager : Singleton<BattleManager>
{
    public SkillTargetType targetType;
    public int id;

    [Button]
    public void Test()
    {
        this.PostEvent(EventID.ON_TARGET_FOCUSED, Tuple.Create(targetType, id));
    }

    [Button]
    public void TestUnfocus()
    {
        this.PostEvent(EventID.ON_TARGET_FOCUSED);
    }
}