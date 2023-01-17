using UnityEngine;
using System;
/// <summary>
/// 캐릭터 클래스별 행동 패턴을 정의 하는 클래스
/// 서브 클래스의 경우 상속 받아 처리 
/// 상속 순서
/// Warrior -> Knight
///			-> Crusader
///			-> SwordDancer
///	Archer	-> Crossbow
///			-> Sniper
///			-> Hunter
///	Wizard	-> Elementalist
///			-> Necromancer
///			-> Warlock
/// </summary>

public abstract class UnitClass
{
	protected Character owner;
	public abstract void OnAttack();
	public virtual void OnInitSkill(SkillModule _skillModule)
	{
		Int64 skillTid = owner.info.data.skillTid;

		if (skillTid != 0)
		{
			if (SkillMeta.it.dic.ContainsKey(skillTid) == false)
			{
				VLog.SkillLogError($"스킬 타입이 테이블에 없음. tid: {skillTid}");
				return;
			}

			SkillBaseData skillBaseData = SkillMeta.it.dic[owner.info.data.skillTid];

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

			_skillModule.AddSkill(skill);
		}
	}
}

public class Warrior : UnitClass
{
	public Warrior(Character character)
	{
		owner = character;
	}

	public override void OnAttack()
	{
		if (owner.attackInterval == 0)
		{
			if (owner.target != null)
			{
				SkillUtility.SimpleAttack(owner, owner.target, owner.info.AttackPower(), owner.skillModule.defaultAttack.name, Color.white);
			}
		}
	}
}

public class Archer : UnitClass
{
	public Archer(Character character)
	{
		owner = character;
	}
	public override void OnAttack()
	{
		if (owner.attackInterval == 0)
		{
			if (owner.target != null)
			{
				ProjectileManager.it.Create(owner);
			}
		}
	}
}

public class Wizard : UnitClass
{
	public Wizard(Character character)
	{
		owner = character;
	}

	public override void OnAttack()
	{
		if (owner.attackInterval == 0)
		{
			if (owner.target != null)
			{
				ProjectileManager.it.Create(owner);
			}
		}
	}
}

public class RewardGem : UnitClass
{
	public RewardGem(Character character)
	{
		owner = character;
	}

	public override void OnAttack()
	{

	}
}
public class Wall : UnitClass
{
	public Wall(Character character)
	{
		owner = character;
	}

	public override void OnAttack()
	{

	}
}

