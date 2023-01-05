﻿using System.Collections.Generic;
using UnityEngine;


public class Mirfiana_sk1Data : SkillBaseData
{
	/// <summary>
	/// 스턴 정보
	/// </summary>
	public StunConditionData stunData;

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	public float damageMul;


	/// <summary>
	/// 체력이 가장 많은 적에게 공격력 1142%만큼 피해를 입히고 치명타 적중시 2초 동안 기절 상태로 만든다.
	/// </summary>
	public Mirfiana_sk1Data(StunConditionData _stunData, float _damageMul, float _cooltime) : base(_cooltime)
	{
		stunData = _stunData;
		damageMul = _damageMul;
	}
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

		var targetList = SkillUtility.GetCharacterRange(owner.transform.position, owner.info.data.attackRange, searchList, false);

		// 체력이 가장 많은 적 찾기
		Character target = owner.target;

		foreach (var checkTarget in targetList)
		{
			if (checkTarget.info.data.hp.GetValue() > target.info.data.hp.GetValue())
			{
				target = checkTarget;
			}
		}

		SkillUtility.SimpleDamage(owner, target, owner.info.DefaultDamage() * skillData.damageMul, fontColor);
		target.conditionModule.AddCondition(new StunCondition(owner, skillData.stunData));
	}
}