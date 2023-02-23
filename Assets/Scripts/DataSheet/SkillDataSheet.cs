using System;
using System.Collections.Generic;

[Serializable]
public class SkillData : BaseData
{
	public string skillName;
	public string skillDesc;
	public float cooltime;
	public string hashTag;
	public long skillEffectTid;
	public bool isHyperSkill;

	public string Icon => tid.ToString();
}

[Serializable]
public class SkillDataSheet : DataSheetBase<SkillData>
{
	public SkillData Get(long tid)
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
	public SkillData Get(string _hash)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			if (infos[i].hashTag == _hash)
			{
				return infos[i];
			}
		}
		return null;
	}
}
