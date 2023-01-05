using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
	/// <summary>
	/// 랜덤 대미지 범위
	/// </summary>
	public const float DAMAGE_RANGE = 0.1f;

	/// <summary>
	/// 크리티컬 확률 최대치
	/// </summary>
	public const float CRITICAL_MAX_RATIO = 0.8f;



	public Character owner;
	public CharacterData data;
	public ControlSide controlSide;


	public CharacterInfo(Character _owner, CharacterData _data, ControlSide _controlSide)
	{
		owner = _owner;

		data = _data.Clone();
		controlSide = _controlSide;
	}

	public IdleNumber DefaultDamage(bool _random = true)
	{
		float multifly = 1 + owner.conditionModule.ability.attackDamageRatio;
		IdleNumber total = data.attackDamage * multifly;

		if (_random)
		{
			total += total * Random.Range(-DAMAGE_RANGE, DAMAGE_RANGE);
		}

		return total;
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
		float total = 1 + data.criticalDamageRatio;

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
