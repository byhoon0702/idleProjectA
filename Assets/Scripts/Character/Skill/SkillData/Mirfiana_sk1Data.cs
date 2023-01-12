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
	[Tooltip("스턴 정보")]
	public StunConditionData stunData = new StunConditionData();

	/// <summary>
	/// 대미지 증가배율
	/// </summary>
	[Tooltip("대미지 증가배율")]
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

		List<Character> targetList = SkillUtility.GetTargetCharacterNonAlloc(owner, skillData.targetingType);

		if (targetList.Count > 0)
		{
			SkillUtility.SimpleAttack(owner, targetList[0], owner.info.AttackPower() * skillData.attackPowerMul, name, fontColor);
			targetList[0].conditionModule.AddCondition(new StunCondition(owner, skillData.stunData));
		}
	}
	public override void CalculateSkillLevelData(int _skillLevel)
	{
	}
}
