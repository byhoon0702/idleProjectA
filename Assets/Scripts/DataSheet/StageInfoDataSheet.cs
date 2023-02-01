using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfoDataSheet : DataSheetBase<StageInfo>
{
	public StageInfo GetNormalStage(int _act, int _stage)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			var info = infos[i];

			if (info.act == _act && info.stage == _stage && info.stageType == StageType.NORMAL)
			{
				return info;
			}
		}
		return null;
	}

	//public StageInfo GetBossStage(int _act, int _stage)
	//{
	//	for (int i = 0; i < infos.Count; i++)
	//	{
	//		var info = infos[i];

	//		if (info.act == _act && info.stage == _stage && info.stageType == StageType.BOSS)
	//		{
	//			return info;
	//		}
	//	}
	//	return null;
	//}

	public StageInfo Get(long _tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}

		return null;
	}
}
