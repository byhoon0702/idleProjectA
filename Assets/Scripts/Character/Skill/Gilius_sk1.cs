using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 아군 전체를 공격력의 546%만큼 회복시키고 6초동안 공격력을 8.4%, 치명률을 3.5% 증가시킨다.
/// </summary>
[Serializable]
public class Gilius_sk1Data : SkillBaseData
{

	/// <summary>
	/// 공격력 증가 데이터
	/// </summary>
	public AttackPowerUpConditionData attackPowerUpData;

	/// <summary>
	/// 치명타확률 증가 데이터
	/// </summary>
	public CriticalChanceUpConditionData criticalChanceUpData;

	/// <summary>
	/// 힐량 증가량
	/// </summary>
	public float healMul = 1;
}



public class Gilius_sk1 : SkillBase
{
	public override string iconPath => "";

	public override float skillUseTime => 0;

	public override bool needAttackState => false;

	private Gilius_sk1Data skillData;


	public Gilius_sk1(Gilius_sk1Data _skillData) : base(_skillData)
	{
		skillData = _skillData;
		fontColor = Color.green;
	}
	public override void Action()
	{
		base.Action();
		List<Character> targetList;
		if (owner is PlayerCharacter)
		{
			targetList = CharacterManager.it.GetPlayerCharacters();
		}
		else
		{
			targetList = CharacterManager.it.GetEnemyCharacters();
		}

		// 힐하면 풀피됨
		foreach (var target in targetList)
		{
			target.conditionModule.AddCondition(new AttackPowerUpCondition(owner, skillData.attackPowerUpData));
			target.conditionModule.AddCondition(new CriticalChanceUpCondition(owner, skillData.criticalChanceUpData));

			target.Heal(target, target.info.AttackPower() * skillData.healMul, name, fontColor);
		}
	}
}
