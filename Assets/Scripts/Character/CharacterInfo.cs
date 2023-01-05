using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
	public const float DAMAGE_RANGE = 0.1f;

	public CharacterData data;
	public ControlSide controlSide;

	public CharacterInfo(CharacterData _data, ControlSide _controlSide)
	{
		data = _data.Clone();
		controlSide = _controlSide;
	}

	public IdleNumber DefaultDamage(bool _random = true)
	{
		IdleNumber total = data.attackDamage;

		if(_random)
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
		float total = data.criticalRatio;

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
}
