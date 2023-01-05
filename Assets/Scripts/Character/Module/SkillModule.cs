using System.Collections.Generic;
using UnityEngine;


public sealed class SkillModule
{
	public List<SkillBase> skills { get; private set; } = new List<SkillBase>();

	public Character character { get; private set; }
	public DefaultAttack defaultAttack { get; private set; }
	public SkillBase skillAttack { get; private set; }

	/// <summary>
	/// 스킬을 보유중이다(기본공격 제외)
	/// </summary>
	public bool hasSkill => skillAttack != null;

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



	public SkillModule(Character _character, DefaultAttack _defaultAttack)
	{
		character = _character;

		defaultAttack = _defaultAttack;

		if (defaultAttack != null)  // 유닛의 생성자에서 초기화할땐 null로 들어옴.
		{
			defaultAttack.SetCharacter(character);


			if (character.info.controlSide == ControlSide.PLAYER)
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

			if (skills[i].skillUseRemainTime > 0)
			{
				// 스킬이 지속되는동안엔 쿨타임 감소가 되지 않는다.
				skills[i].skillUseRemainTime -= _dt;
			}
			else
			{
				// 스킬 사용시간이 끝나면 쿨타임을 감소시킨다
				skills[i].remainCooltime -= _dt;
				if (skills[i].remainCooltime <= 0)
				{
					skills[i].Ready();
				}
			}
		}
	}

	public void AddSkill(SkillBase _skill)
	{
		// 사용할 유닛을 등록시켜줘야 함.
		_skill.SetCharacter(character);

		skills.Add(_skill);

		if (_skill is DefaultAttack)
		{
			defaultAttack = _skill as DefaultAttack;
		}
		else
		{
			skillAttack = _skill;
		}
	}
}
