using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class EventStageDataSheet : DataSheetBase<StageData>
{
	public List<StageData> GetListByType(StageType type)
	{
		return infos.FindAll(x => x.stageType == type);
	}
}
