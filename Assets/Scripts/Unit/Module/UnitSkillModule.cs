using System.Collections.Generic;
using UnityEngine;

public class UnitSkillModule
{
	public List<SkillBase> skills { get; private set; } = new List<SkillBase>();


	private Unit ownerUnit;

	public bool auto = false;
	Queue<SkillBase> skillQueue = new Queue<SkillBase>();

	public void InitSkill(long[] _skillList)
	{
		skills.Clear();


		foreach (var itemTid in _skillList)
		{
			if (itemTid == 0)
			{
				continue;
			}

			long skillTid = DataManager.Get<SkillItemDataSheet>().Get(itemTid).skillTid;
			if (skillTid == 0)
			{
				VLog.SkillLogError($"스킬 TID가 없음. ItemDataSheet, Itemtid: {itemTid}");
				continue;
			}
			SkillBase skill = new SkillBase(skillTid);
			skills.Add(skill);
		}
	}

	public void SetUnit(Unit _unit)
	{
		ownerUnit = _unit;
		ownerUnit.unitSkillModule = this;
		foreach (var v in skills)
		{
			v.SetUnit(_unit);
		}
	}

	float time;
	public void Update(float _dt)
	{
		if (ownerUnit == null || ownerUnit.currentState == StateType.DEATH)
		{
			return;
		}

		// 스킬 쿨타임 감소
		for (int i = 0; i < skills.Count; i++)
		{
			// 스킬 쿨타임을 계산하지 말아야 하면 무시
			if (!skills[i].coolDowning)
			{
				continue;
			}

			skills[i].UpdateCoolTime(_dt);
		}

		//if (auto)
		//{
		//	foreach (var skill in skills)
		//	{
		//		if (skill.Usable())
		//		{
		//			skill.Action();
		//		}
		//	}
		//}
	}
	public SkillBase GetUsableSkill()
	{
		foreach (var skill in skills)
		{
			if (skill.Usable())
			{
				return skill;
			}
		}
		return null;
	}

	public bool CanUseSkill()
	{
		foreach (var skill in skills)
		{
			if (skill.Usable())
			{
				return true;
			}
		}
		return false;
	}



	public SkillBase FindSkillBase(long _skillTid)
	{
		foreach (var v in skills)
		{
			if (v.tid == _skillTid)
			{
				return v;
			}
		}

		return null;
	}
}
