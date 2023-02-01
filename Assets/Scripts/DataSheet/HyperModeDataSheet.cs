using System;
using System.Collections.Generic;

[Serializable]
public class HyperModeData : BaseData
{
	public Grade grade;
	public float attackPowerRatio;
	public float hpUpRatio;
	public int promoteAbilitySlot;
	public int needLevel;
	public int abilityLevel;
}

[Serializable]
public class HyperModeDataSheet : DataSheetBase<HyperModeData>
{
	private List<Grade> grade;

	public HyperModeData Get(long tid)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i];
			}
		}

		return null;
	}

	public HyperModeData Get(Grade _grade)
	{
		for (int i = 0 ; i < infos.Count ; i++)
		{
			if (infos[i].grade == _grade)
			{
				return infos[i];
			}
		}

		return null;
	}

	public List<Grade> GetGradeList()
	{
		if (grade == null || grade.Count == 0)
		{
			grade = new List<Grade>();

			for (int i = 0 ; i < infos.Count ; i++)
			{
				if (grade.Contains(infos[i].grade) == false)
				{
					grade.Add(infos[i].grade);
				}
			}
		}

		return grade;
	}
}
