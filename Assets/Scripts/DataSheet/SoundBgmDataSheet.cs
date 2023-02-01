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
	public SoundBgmData Get(long _tid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].tid == _tid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public SoundBgmData Get(string _key)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].key == _key)
			{
				return infos[i];
			}
		}

		return null;
	}
}
