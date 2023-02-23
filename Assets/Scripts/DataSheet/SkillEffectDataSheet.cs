using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AttackData
{
	//public int level;
	public float timing;
	public string hitFxResource;
	public string objectResource;

	public float damageRate;
	public int damageCount;
	public float damageInterval;

	public string actionBehavior;
	public string damageBehavior;

	public AttackData()
	{
		//level = 1;
		timing = 0;
		hitFxResource = "";
		objectResource = "";
		damageRate = 1;
		damageCount = 1;
		damageInterval = 0;
		actionBehavior = "";
		damageBehavior = "";
	}
}

[System.Serializable]
public class SkillEffectData : BaseData
{
	public string name;
	public float speed;
	public bool isDependent;
	public float duration;
	public string spawnFxResource;
	public string abilitySO;
	//public AttackRange attackRange;
	//public List<AttackData> attackData = new List<AttackData>();

	public SkillEffectData()
	{
		description = "New Skill";
		name = "NewSkill";
		speed = 10;
		duration = 1;
		spawnFxResource = "";
		abilitySO = "";
		isDependent = false;
	}

	public SkillEffectData Clone()
	{
		SkillEffectData clone = new SkillEffectData();
		clone.tid = tid;
		clone.description = description;
		clone.name = name;
		clone.speed = speed;
		clone.duration = duration;
		clone.abilitySO = abilitySO;
		clone.spawnFxResource = spawnFxResource;
		//clone.attackData = new List<AttackData>(attackData);
		clone.isDependent = isDependent;
		return clone;
	}
}

[System.Serializable]
public class SkillEffectDataSheet : DataSheetBase<SkillEffectData>
{

	public SkillEffectData GetData(long tid)
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
#if UNITY_EDITOR
	/// <summary>
	/// 에디터용이 아니면 절대로 사용하면 안됨
	/// </summary>
	public void SetData(long tid, SkillEffectData data)
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

	public void AddData(SkillEffectData data)
	{
		infos.Add(data);
	}
#endif
}

