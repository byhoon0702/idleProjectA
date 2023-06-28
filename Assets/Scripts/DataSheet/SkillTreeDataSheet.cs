using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum RequirementType
{
	/// <summary>
	/// 스킬 해금 조건 없음
	/// </summary>
	NONE,
	//이전 스킬 레벨
	BASESKILL,
	//유저 레벨
	USERLEVEL,
	//스테이지
	STAGE,
}
[System.Serializable]
public class RequirementInfo
{
	public RequirementType type;
	public long parameter1;
	public int parameter2;
}

[System.Serializable]
public class SkillTree
{
	public long skillTid;
	public RequirementInfo requirement;
}


[System.Serializable]
public class SkillTreeData : BaseData
{
	public SkillTreeType type;
	public long skillTid;
	public List<SkillTree> skillTreeList;
}

[System.Serializable]
public class SkillTreeDataSheet : DataSheetBase<SkillTreeData>
{

	public SkillTreeData GetSkillTreeData(long baseTid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			var data = infos[i];
			if (data.skillTid == baseTid)
			{
				return data;
			}
		}

		return null;
	}

	public RequirementInfo GetSkillRequirement(long baseTid, long skillTid)
	{
		for (int i = 0; i < infos.Count; i++)
		{
			var data = infos[i];
			if (data.skillTid == baseTid)
			{
				for (int ii = 0; ii < data.skillTreeList.Count; ii++)
				{
					var skillTree = data.skillTreeList[ii];
					if (skillTree.skillTid == skillTid)
					{
						return skillTree.requirement;
					}
				}
			}
		}

		return null;
	}
}
