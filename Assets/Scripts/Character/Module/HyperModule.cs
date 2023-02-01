using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperModule
{
	/// <summary>
	/// 0 ~ 1
	/// </summary>
	public float hyperModeGauge;

	/// <summary>
	/// 하이퍼모드 남은시간
	/// </summary>
	public float hyperRemainTime;

	/// <summary>
	/// 하이퍼 모드 발동중
	/// </summary>
	public bool IsHyperMode => hyperRemainTime > 0;


	
	/// <summary>
	/// 하이퍼 모드게이지 누적
	/// </summary>
	public void AccumGauge()
	{
		if(IsHyperMode)
		{
			return;
		}

		hyperModeGauge += 0.05f;
	}

	/// <summary>
	/// 하이퍼 모드 발동
	/// </summary>
	public void ActiveHyperMode()
	{
		hyperRemainTime = 6;
		hyperModeGauge = 0;

		foreach(var playerUnit in CharacterManager.it.GetPlayerCharacters())
		{
			(playerUnit as PlayerCharacter).ActiveHyperEffect();
		}
	}

	/// <summary>
	/// 하이퍼 모드 비활성화
	/// </summary>
	public void InactiveHyperMode()
	{
		hyperRemainTime = 0;
		hyperModeGauge = 0;

		foreach (var playerUnit in CharacterManager.it.GetPlayerCharacters())
		{
			(playerUnit as PlayerCharacter).InactiveHyperEffect();
		}
	}

	public void Update(float _dt)
	{
		if (hyperRemainTime > 0)
		{
			hyperRemainTime -= _dt;
			if(hyperRemainTime <= 0)
			{
				InactiveHyperMode();
			}
		}
	}

	/// <summary>
	/// 하이퍼 보너스 어빌리티
	/// 하이퍼 모드가 꺼져있으면 0 리턴
	/// </summary>
	public float GetHyperAbility(UnitBase _unit, AbilityType _ability)
	{
		if(IsHyperMode == false)
		{
			return 0;
		}
		if(_unit.controlSide != ControlSide.PLAYER)
		{
			return 0;
		}

		switch (_ability)
		{
			case AbilityType.AttackPower:
				return 10f;
			case AbilityType.MoveSpeed:
				return 3f;
			case AbilityType.AttackSpeed:
				return 3f;
			case AbilityType.CriticalAttackPower:
				return 20f;
			case AbilityType.SkillAttackPower:
				return 1f;
			case AbilityType.BossAttackPower:
				return 1f;

			default:
				return 0;
		}
	}
}
