using System.Collections.Generic;
[System.Serializable]
public class UnitDataSheet : DataSheetBase<UnitData>
{
	public UnitData GetData(long _tid)
	{
		if (_tid == 0)
		{
			return null;
		}
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


