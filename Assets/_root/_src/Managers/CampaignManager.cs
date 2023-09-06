using System;
using System.Collections.Generic;

public class CampaignManager : Singleton<CampaignManager>
{
    public List<ChapterData> chapterList;

    public StageData GetStageData(int chapterId, int stageId)
    {
        var chap = chapterList.Find(c => c.chapter == chapterId);
        var stage = chap?.stages.Find(s => s.stage == stageId);
        return stage;
    }
}

[Serializable]
public class ChapterData
{
    public int chapter;
    public List<StageData> stages;
}

[Serializable]
public class StageData
{
    public int stage;
    public List<PhaseData> phases;
    // add rewards
}

[Serializable]
public class PhaseData
{
    public int phase;
    public List<EnemyData> enemies;
}

[Serializable]
public class EnemyData
{
    public string entityId;
    public bool isBoss;
    public int level;
}