
using System;

[Serializable]
public class RaceData
{
	public long tid;
	//데이터 테이블에만 표시되는 설명 
	public string description;
	public RaceType raceType;
}
[Serializable]
public class RaceDataSheet : DataBase<RaceData>
{
	public RaceData Get(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}
		return null;
	}
}
