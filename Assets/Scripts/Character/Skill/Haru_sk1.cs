using System.Collections.Generic;
using UnityEngine;



public class Haru_sk1Data : SkillBaseData
{
	/// <summary>
	/// 독대미지 정보
	/// </summary>
	public PoisonConditionData poisonData;

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	public float damageMul;

	/// <summary>
	/// 독대미지 적용확률
	/// </summary>
	public float poisonProbability;


	/// <summary>
	/// 적 전체에게 공격력의 339%만큼 피해를 입히고 50%확률로 5초동안 40%만큼 지속 피해를 입힌다.
	/// </summary>
	/// <param name="_cooltime"></param>
	public Haru_sk1Data(PoisonConditionData _poisonData, float _damageMul, float _poisonProbability, float _cooltime) : base(_cooltime)
	{
		poisonData = _poisonData;
		damageMul = _damageMul;
		poisonProbability = _poisonProbability;
	}
}


public class Haru_sk1 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Haru_sk1Data skillData;


	public Haru_sk1(Haru_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		base.Action();
		List<Character> targetList;
		if (character is PlayerCharacter)
		{
			targetList = CharacterManager.it.GetEnemyCharacters();
		}
		else
		{
			targetList = CharacterManager.it.GetPlayerCharacters();
		}


		bool addPoison = SkillUtility.Cumulative(skillData.poisonProbability);
		foreach (var target in targetList)
		{
			if (addPoison)
			{
				target.conditionModule.AddCondition(new PoisonCondition(character, skillData.poisonData));
			}

			SkillUtility.SimpleDamage(character, target, target.info.DefaultDamage() * skillData.damageMul, fontColor);
		}
	}
}
