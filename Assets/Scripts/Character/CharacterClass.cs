using UnityEngine;
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

public abstract class CharacterClass
{
	protected Character owner;
	public abstract void OnAttack();
	public virtual void OnInitSkill(SkillModule _skillModule)
	{
		string id = owner.info.data.skillID;
		if (string.IsNullOrEmpty(id) == false)
		{
			var type = System.Type.GetType(id);

			if (type == null)
			{
				VLog.SkillLogError($"스킬 타입 정의 안됨. Skill ID: {id}");
				return;
			}

			if (SkillDictionary.dic.ContainsKey(id) == false)
			{
				VLog.SkillLogError($"스킬 생성정보를 찾을 수 없음. Skill ID: {id}");
				return;
			}

			var paramerts = SkillDictionary.dic[id];
			object classObject;
			SkillBase skill;

			try
			{
				classObject = System.Activator.CreateInstance(type, paramerts);
				skill = classObject as SkillBase;
			}
			catch
			{
				VLog.SkillLogError($"스킬 생성실패. SKill ID : {id}");
				return;
			}

			_skillModule.AddSkill(skill);
		}
	}
}

public class Warrior : CharacterClass
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
				SkillUtility.SimpleDamage(owner, owner.target, owner.info.DefaultDamage(), Color.white);
			}
		}
	}
}

public class Archer : CharacterClass
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

public class Wizard : CharacterClass
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

public class RewardGem : CharacterClass
{
	public RewardGem(Character character)
	{
		owner = character;
	}

	public override void OnAttack()
	{

	}
}
