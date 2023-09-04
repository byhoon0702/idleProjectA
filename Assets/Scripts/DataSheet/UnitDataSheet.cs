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
				return infos[i].Clone<UnitData>();
			}
		}
		return null;
	}
#if UNITY_EDITOR
	/// <summary>
	/// 에디터용이 아니면 절대로 사용하면 안됨
	/// </summary>
	public void SetData(long tid, UnitData data)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				infos[i] = data;
				break;
			}
		}
	}

	public void AddData(UnitData data)
	{
		infos.Add(data);
	}
#endif
}


