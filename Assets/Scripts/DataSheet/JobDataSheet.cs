
[System.Serializable]
public class JobData
{
	public long tid;
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
public class JobDataSheet : DataBase<JobData>
{
	public JobData Get(long tid)
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
