using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 적 전체에게 공격력의 339%만큼 피해를 입히고 50%확률로 5초동안 40%만큼 지속 피해를 입힌다.
/// </summary>
[Serializable]
public class Haru_sk1Data : SkillBaseData
{
	/// <summary>
	/// 독대미지 정보
	/// </summary>
	public PoisonConditionData poisonData = new PoisonConditionData();

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	public float attackPowerMul = 1;

	/// <summary>
	/// 독대미지 적용확률
	/// </summary>
	public float poisonProbability = 0.5f;
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
		if (owner is PlayerCharacter)
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
				target.conditionModule.AddCondition(new PoisonCondition(owner, skillData.poisonData));
			}

			SkillUtility.SimpleAttack(owner, target, target.info.AttackPower() * skillData.attackPowerMul, name, fontColor);
		}
	}
}
