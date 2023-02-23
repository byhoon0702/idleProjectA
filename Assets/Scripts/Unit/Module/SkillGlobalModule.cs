using System.Collections.Generic;
using UnityEngine;


public sealed class SkillGlobalModule
{
	public List<SkillBase> skills { get; private set; } = new List<SkillBase>();
	private Unit playerUnit;

	public bool auto = false;


	public void InitSkill(long[] _skillList)
	{
		skills.Clear();


		foreach (var itemTid in _skillList)
		{
			if(itemTid == 0)
			{
				continue;
			}

			long skillTid = DataManager.Get<ItemDataSheet>().Get(itemTid).skillTid;
			if(skillTid == 0)
			{
				VLog.SkillLogError($"스킬 TID가 없음. ItemDataSheet, Itemtid: {itemTid}");
				continue;
			}

			if (DataManager.SkillMeta.dic.ContainsKey(skillTid) == false)
			{
				VLog.SkillLogError($"Skill Raw정보가 없음(Resources폴더 내 SkillRaw폴더). skilltid: {skillTid}");
				return;
			}

			SkillBaseData skillBaseData = DataManager.SkillMeta.dic[skillTid];

			string id = skillBaseData.skillPreset;
			id = id.Substring(0, id.Length - 4); // 끝에 'Data' 텍스트를 지우기 위함
			var type = System.Type.GetType(id);

			if (type == null)
			{
				VLog.SkillLogError($"스킬 타입 정의 안됨. Skill ID: {id}");
				return;
			}

			// 스킬 생성
			SkillBase skill;
			try
			{
				object classObject = System.Activator.CreateInstance(type, new object[] { skillBaseData });

				skill = classObject as SkillBase;
			}
			catch (System.Exception e)
			{
				VLog.SkillLogError($"스킬 생성실패. SKill ID : {id}\n{e}");
				return;
			}

			skills.Add(skill);
		}
	}

	public void SetUnit(Unit _unit)
	{
		playerUnit = _unit;

		foreach (var v in skills)
		{
			v.SetUnit(_unit);
		}
	}

	public void Update(float _dt)
	{
		if(playerUnit == null || playerUnit.currentState == StateType.DEATH)
		{
			return;
		}

		// 스킬 쿨타임 감소
		for (int i = 0 ; i < skills.Count ; i++)
		{
			// 스킬 쿨타임을 계산하지 말아야 하면 무시
			if (!skills[i].coolDowning)
			{
				continue;
			}

			skills[i].UpdateCoolTime(_dt);
		}


		if (auto)
		{
			foreach (var skill in skills)
			{
				if (skill.Usable())
				{
					skill.Action();
				}
			}
		}
	}

	public SkillBase FindSkillBase(long _skillTid)
	{
		foreach(var v in skills)
		{
			if(v.tid == _skillTid)
			{
				return v;
			}
		}

		return null;
	}
}
