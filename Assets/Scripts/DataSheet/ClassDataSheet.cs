
[System.Serializable]
public class ClassData : BaseData
{
	public ClassType classType;
	public AttackType attackType;
	public float attackRange = 1;
	public float attackSpeed = 1;
	public float moveSpeed = 1;
	public float criticalRate = 1;
	public float criticalPowerRate = 1;
	public float evasionRate = 1;
	public float skillCooltime = 1;
	public float searchRange = 1;
	public float searchTime = 1;

	public ClassData Clone()
	{
		ClassData cloneData = new ClassData();
		cloneData.classType = classType;
		cloneData.attackType = attackType;
		cloneData.attackRange = attackRange;
		cloneData.attackSpeed = attackSpeed;
		cloneData.moveSpeed = moveSpeed;
		cloneData.criticalRate = criticalRate;
		cloneData.criticalPowerRate = criticalPowerRate;
		cloneData.evasionRate = evasionRate;
		cloneData.skillCooltime = skillCooltime;
		cloneData.searchRange = searchRange;
		cloneData.searchTime = searchTime;

		return cloneData;
	}
}

[System.Serializable]
public class ClassDataSheet : DataSheetBase<ClassData>
{
	public ClassData Get(long tid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].tid == tid)
			{
				return infos[i].Clone();
			}
		}
		return null;
	}
}
