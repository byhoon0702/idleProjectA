using System;
using System.Collections.Generic;

[Serializable]
public class SoundBgmData : BaseData
{
	public string key;
	public string resource;
}

[Serializable]
public class SoundBgmDataSheet : DataSheetBase<SoundBgmData>
{


	public override SoundBgmData Get(string key)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].key == key)
			{
				return infos[i];
			}
		}

		return null;
	}
}
