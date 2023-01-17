using System;


[Serializable]
public class UnitLevelupValueData : BaseData
{
	public Grade grade;
	public int starLevel;
	public float hpUpValue;
	public float attackUpValue;
	public long classWeightTid;
	public long elementWeightTid;
}
[Serializable]
public class upValueDataSheet : DataSheetBase<UnitLevelupValueData>
{

	public UnitLevelupValueData Get(long tid)
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

	public UnitLevelupValueData Get(Grade grade, int starLevel)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].grade == grade && infos[i].starLevel == starLevel)
			{
				return infos[i];
			}
		}

		return null;
	}
}
