
[System.Serializable]
public class ClassData
{
	public long tid;
	//데이터 테이블에만 표시되는 설명 
	public string description;
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
}

[System.Serializable]
public class ClassDataSheet : DataBase<ClassData>
{
	public ClassData Get(long tid)
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
