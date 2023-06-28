using System;
using System.Collections.Generic;

[Serializable]
public class UserGradeData : BaseData
{
	public Grade grade;

	public float attackPowerRatio;
	public float hpUpRatio;

	public float hyperAttackPower;
	public float hyperAttackSpeed;
	public float hyperMoveSpeed;
	public float hyperCriticalAttackPower;
	public float hyperDuration;
	public float hyperGaugeRecovery;

	public int coreAbilitySlot;
	public int coreAbilityMaxLevel;
}

[Serializable]
public class UserGradeDataSheet : DataSheetBase<UserGradeData>
{
	private List<Grade> grade;


	public UserGradeData Get(Grade _grade)
	{
		for (int i = 0; i < infos.Count; i++)
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

			for (int i = 0; i < infos.Count; i++)
			{
				if (grade.Contains(infos[i].grade) == false)
				{
					grade.Add(infos[i].grade);
				}
			}
		}

		return grade;
	}

	public IdleNumber CoreLevelupConsume(int _currentCoreAbilityLevel)
	{
		// 100 + (coreLevel * 100)
		return new IdleNumber(100 + (_currentCoreAbilityLevel * 100));
	}

	public Grade NextGrade(Grade userGrade)
	{
		switch (userGrade)
		{
			case Grade.D:
				return Grade.C;
			case Grade.C:
				return Grade.B;
			case Grade.B:
				return Grade.A;
			case Grade.A:
				return Grade.S;
			case Grade.S:
				return Grade.SS;
			case Grade.SS:
				return Grade.SSS;
		}

		return Grade.D;
	}
}
