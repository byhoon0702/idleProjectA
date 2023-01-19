using System;
using System.Collections.Generic;

[Serializable]
public class UserGradeData : BaseData
{
	public Grade grade;
	public float attackPowerRatio;
	public float hpUpRatio;
	public Int32 promoteAbilitySlot;
	public Int32 needLevel;
	public Int32 abilityLevel;
}

[Serializable]
public class UserGradeDataSheet : DataSheetBase<UserGradeData>
{
	private List<Grade> grade;

	public UserGradeData Get(long tid)
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

	public UserGradeData Get(Grade _grade)
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
