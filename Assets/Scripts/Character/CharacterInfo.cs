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
	/// 크리티컬 확률 최대치
	/// </summary>
	public const float CRITICAL_MAX_RATIO = 0.8f;

	/// <summary>
	/// 받는 피해 최소량
	/// </summary>
	public const float MIN_REDUCE_DEFENSE_MUL = 0.01f;

	/// <summary>
	/// 받는 피해 최대량
	/// </summary>
	public const float MAX_REDUCE_DEFENSE_MUL = 5;



	public Character owner;
	public CharacterData data;
	public ControlSide controlSide;


	public CharacterInfo(Character _owner, CharacterData _data, ControlSide _controlSide)
	{
		owner = _owner;

		data = _data.Clone();
		controlSide = _controlSide;
	}

	public IdleNumber AttackPower(bool _random = true)
	{
		float multifly = 1 + owner.conditionModule.ability.attackPowerUpRatio;
		IdleNumber total = data.attackPower * multifly;

		if (_random)
		{
			total += total * Random.Range(-ATTACK_POWER_RANGE, ATTACK_POWER_RANGE);
		}

		return total;
	}

	/// <summary>
	/// 피해감소(받는 피해량). 1이 기본값
	/// </summary>
	/// <returns></returns>
	public float ReduceDefenseMul()
	{
		//float total = 1 + (1 - owner.conditionModule.ability.reducedDefenseRatio);
		return 1;
	}

	/// <summary>
	/// 크리티컬 발동여부. true면 크리티컬로 처리하면 됨
	/// </summary>
	public bool IsCritical()
	{
		float total = data.criticalRatio + owner.conditionModule.ability.criticalUpRatio;


		if(total > CRITICAL_MAX_RATIO)
		{
			total = CRITICAL_MAX_RATIO;
		}

		return SkillUtility.Cumulative(total);
	}
	
	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public float CriticalMultifly()
	{
		float total = 1 + data.criticalChangeUpRatio;

		return total;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public float MoveSpeed()
	{
		float mul = 1 + owner.conditionModule.ability.moveSpeedUpRatio;
		float total = data.moveSpeed * mul;

		return total;
	}
}
