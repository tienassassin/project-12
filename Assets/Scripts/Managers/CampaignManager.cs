using System;
using System.Collections.Generic;

public class CampaignManager : Singleton<CampaignManager>
{
    public List<CampaignChapterData> chapterList;

    public CampaignStageData GetStageData(int chapterId, int stageId)
    {
        var chap = chapterList.Find(c => c.chapter == chapterId);
        var stage = chap?.stageList.Find(s => s.stage == stageId);
        return stage;
    }
}

[Serializable]
public class CampaignChapterData
{
    public int chapter;
    public List<CampaignStageData> stageList;
}

[Serializable]
public class CampaignStageData
{
    public int stage;
    public List<DevilPhaseData> phaseList;
    // add rewards
}

[Serializable]
public class DevilPhaseData
{
    public int phase;
    public List<DevilData> devilList;
}

[Serializable]
public class DevilData
{
    public string devilId;
    public bool isBoss;
    public int level;
}