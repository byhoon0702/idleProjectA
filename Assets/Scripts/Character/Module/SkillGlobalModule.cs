using System.Collections.Generic;
using UnityEngine;


public sealed class SkillGlobalModule
{
	public List<SkillBase> skills { get; private set; } = new List<SkillBase>();
	private Unit playerUnit;



	public void InitSkill(long[] _skillList)
	{
		skills.Clear();


		foreach (var skillTid in _skillList)
		{
			if(skillTid == 0)
			{
				continue;
			}

			if (skillTid != 0)
			{
				if (SkillMeta.it.dic.ContainsKey(skillTid) == false)
				{
					VLog.SkillLogError($"스킬 타입이 테이블에 없음. tid: {skillTid}");
					return;
				}

				SkillBaseData skillBaseData = SkillMeta.it.dic[skillTid];

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
	}
}
