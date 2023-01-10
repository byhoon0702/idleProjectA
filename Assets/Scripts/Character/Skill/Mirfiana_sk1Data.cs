using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 체력이 가장 많은 적에게 공격력 1142%만큼 피해를 입히고 치명타 적중시 2초 동안 기절 상태로 만든다.
/// </summary>
[Serializable]
public class Mirfiana_sk1Data : SkillBaseData
{
	/// <summary>
	/// 스턴 정보
	/// </summary>
	public StunConditionData stunData = new StunConditionData();

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	public float attackPowerMul = 1;
}


public class Mirfiana_sk1 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	private Mirfiana_sk1Data skillData;


	public Mirfiana_sk1(Mirfiana_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.magenta;
	}

	public override void Action()
	{
		base.Action();

		List<Character> searchList;
		if (owner is PlayerCharacter)
		{
			searchList = CharacterManager.it.GetEnemyCharacters();
		}
		else
		{
			searchList = CharacterManager.it.GetPlayerCharacters();
		}

		var targetList = SkillUtility.GetCharacterRangeNonAlloc(owner.transform.position, owner.info.jobData.attackRange, searchList);

		// 체력이 가장 많은 적 찾기
		Character target = owner.target;

		foreach (var checkTarget in targetList)
		{
			if (checkTarget.info.data.hp.GetValue() > target.info.data.hp.GetValue())
			{
				target = checkTarget;
			}
		}

		SkillUtility.SimpleAttack(owner, target, owner.info.AttackPower() * skillData.attackPowerMul, name, fontColor);
		target.conditionModule.AddCondition(new StunCondition(owner, skillData.stunData));
	}
}
