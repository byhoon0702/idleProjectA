using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BgData : BaseData
{
	public string bgCloseName;
	public string bgMiddleName;
	public string bgFarName;

	public BgData Clone()
	{
		BgData bgData = new BgData();

		bgData.bgCloseName = bgCloseName;
		bgData.bgMiddleName = bgMiddleName;
		bgData.bgFarName = bgFarName;
		bgData.tid = tid;

		return bgData;
	}
}

[System.Serializable]
public class BgDataSheet : DataSheetBase<BgData>
{
	public BgData Get(long _tid)
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
