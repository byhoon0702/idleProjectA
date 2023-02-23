using System.Collections.Generic;
using UnityEngine;


public sealed class SkillModule
{
	public List<SkillBase> skills { get; private set; } = new List<SkillBase>();

	public Unit unit { get; private set; }
	public DefaultAttack defaultAttack { get; private set; }
	public SkillBase mainSkill { get; private set; }

	/// <summary>
	/// 스킬사용이 진행중이다.
	/// </summary>
	public bool skillRunning
	{
		get
		{
			for (int i = 0; i < skills.Count; i++)
			{
				if (skills[i].skillUseRemainTime > 0)
					return true;
			}

			return false;
		}
	}



	public SkillModule(Unit _unit, DefaultAttack _defaultAttack)
	{
		unit = _unit;

		defaultAttack = _defaultAttack;

		if (defaultAttack != null)  // 유닛의 생성자에서 초기화할땐 null로 들어옴.
		{
			defaultAttack.SetUnit(unit);


			if (unit.ControlSide == ControlSide.PLAYER)
			{
				defaultAttack.fontColor = Color.white;
			}
			else
			{
				defaultAttack.fontColor = Color.red;
			}

		}

		skills.Add(defaultAttack);
	}

	public void Update(float _dt)
	{
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
	}

	public void AddSkill(SkillBase _skill)
	{
		// 사용할 유닛을 등록시켜줘야 함.
		_skill.SetUnit(unit);
		skills.Add(_skill);

		if (_skill is DefaultAttack)
		{
			defaultAttack = _skill as DefaultAttack;
		}
	}

	public void RegistMainSkill(SkillBase _skill)
	{
		mainSkill = _skill;
	}
}
