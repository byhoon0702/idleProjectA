using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
	/// <summary>
	/// 랜덤 대미지 범위
	/// </summary>
	public const float ATTACK_POWER_RANGE = 0.1f;

	/// <summary>
	/// 공격속도 최소치
	/// </summary>
	public const float ATTACK_SPEED_MIN = 0.1f;

	/// <summary>
	/// 공격속도 최대치
	/// </summary>
	public const float ATTACK_SPEED_MAX = 3;

	/// <summary>
	/// 크리티컬 확률 최대치
	/// </summary>
	public const float CRITICAL_CHANCE_MAX_RATIO = 0.8f;

	/// <summary>
	/// 받는 피해 최소량
	/// </summary>
	public const float MIN_DAMAGE_MUL = 0.01f;

	/// <summary>
	/// 받는 피해 최대량
	/// </summary>
	public const float MAX_DAMAGE_MUL = 5;



	public Character owner;
	public CharacterData data;
	public ControlSide controlSide;

	/// <summary>
	/// UI표시용
	/// </summary>
	public string charName => data.name;

	/// <summary>
	/// 캐릭터 ID를 같이 표시하는용(디버깅용)
	/// </summary>
	public string charNameAndCharId => $"{data.name}({owner.charID})";


	public CharacterInfo(Character _owner, CharacterData _data, ControlSide _controlSide)
	{
		owner = _owner;

		data = _data.Clone();
		controlSide = _controlSide;
	}

	public IdleNumber AttackPower(bool _random = true)
	{
		float conditionTotalRatio = owner.conditionModule.ability.attackPowerUpRatio - owner.conditionModule.ability.attackPowerDownRatio;
		float multifly = 1 + conditionTotalRatio;

		IdleNumber total = data.attackPower * multifly;

		if (_random)
		{
			total += total * Random.Range(-ATTACK_POWER_RANGE, ATTACK_POWER_RANGE);
		}

		return total;
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public float AttackSpeedMul()
	{
		float total = 1 + owner.conditionModule.ability.attackSpeedUpRatio - owner.conditionModule.ability.attackSpeedDownRatio;

		total = Mathf.Clamp(total, ATTACK_SPEED_MIN, ATTACK_SPEED_MAX);

		return total;
	}

	/// <summary>
	/// 받는 피해량
	/// </summary>
	/// <returns></returns>
	public float DamageMul()
	{
		float conditionTotalRatio = owner.conditionModule.ability.damageUpRatio - owner.conditionModule.ability.damageDownRatio;
		float total = 1 + conditionTotalRatio;

		total = Mathf.Clamp(total, MIN_DAMAGE_MUL, MAX_DAMAGE_MUL);

		return total;
	}

	/// <summary>
	/// 크리티컬 발동여부. true면 크리티컬로 처리하면 됨
	/// </summary>
	public bool IsCritical()
	{
		float conditionTotalRatio = owner.conditionModule.ability.criticalChanceUpRatio - owner.conditionModule.ability.criticalChanceDownRatio;
		float total = data.criticalChanceRatio + conditionTotalRatio;


		if(total > CRITICAL_CHANCE_MAX_RATIO)
		{
			total = CRITICAL_CHANCE_MAX_RATIO;
		}

		return SkillUtility.Cumulative(total);
	}
	
	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public float CriticalDamageMultifly()
	{
		float total = 1;

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public float MoveSpeed()
	{
		float conditionTotalRatio = owner.conditionModule.ability.moveSpeedUpRatio - owner.conditionModule.ability.moveSpeedDownRatio;
		float mul = 1 + conditionTotalRatio;

		float total = data.moveSpeed * mul;

		return total;
	}
}
