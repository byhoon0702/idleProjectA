using System;


[Serializable]
public class UnitLevelupConsumeData : BaseData
{
	public Grade grade;
	public int starLevel;
	public long unitLevel;

	public int defaultConsume;
	public int defaultUpValue;
}

[Serializable]
public class UnitLevelupConsumeDataSheet : DataSheetBase<UnitLevelupConsumeData>
{

	public UnitLevelupConsumeData Get(long tid)
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

	public UnitLevelupConsumeData Get(Grade grade, int starLevel)
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
