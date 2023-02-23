using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HyperModule
{
	public enum State
	{
		/// <summary>
		/// 게이지 차징중
		/// </summary>
		Charge,
		/// <summary>
		/// 하이퍼모드상태
		/// </summary>
		HyperMode,
		/// <summary>
		/// 하이퍼 브레이크 발동상태
		/// </summary>
		HyperBreak
	}


	public State state;
	public bool auto = false;

	/// <summary>
	/// 0 ~ 1
	/// </summary>
	public float hyperModeGauge;

	/// <summary>
	/// 0 ~ 1
	/// </summary>
	public float hyperBreakGauge;

	/// <summary>
	/// 하이퍼 모드 발동중
	/// </summary>
	public bool IsHyperMode => state != State.Charge;



	/// <summary>
	/// 초기화
	/// </summary>
	public void Reset()
	{
		state = State.Charge;

		hyperModeGauge = 0;
		hyperBreakGauge = 0;
	}

	/// <summary>
	/// 하이퍼 모드게이지 누적
	/// </summary>
	public void AccumGauge()
	{
		if (state != State.Charge)
		{
			return;
		}

		hyperModeGauge += UserInfo.UserGrade.hyperGaugeRecovery;
		if (hyperModeGauge > 1)
		{
			hyperModeGauge = 1;
		}
	}

	/// <summary>
	/// 하이퍼 모드 발동가능
	/// </summary>
	public bool IsActiveHyperMode()
	{
		if (state != State.Charge)
		{
			return false;
		}

		bool gaugeMax = hyperModeGauge >= 1;
		bool readyChar = UnitManager.it.Player.IsActiveHyperMode();

		return gaugeMax && readyChar;
	}

	/// <summary>
	/// 하이퍼 모드 발동
	/// </summary>
	public void ActiveHyperMode()
	{
		state = State.HyperMode;
		UnitManager.it.Player.ActiveHyperEffect();
	}

	/// <summary>
	/// 하이퍼 브레이크 발동가능
	/// </summary>
	public bool IsActiveHyperBreak()
	{
		if (state != State.HyperMode)
		{
			return false;
		}

		bool gaugeMax = hyperBreakGauge >= 1;
		bool useableChar = UnitManager.it.Player.UseableHyperBreak();

		return gaugeMax && useableChar;
	}

	/// <summary>
	/// 하이퍼 브레이크 발동
	/// </summary>
	public void UseHyperBreak()
	{
		state = State.HyperBreak;
		UnitManager.it.Player.StartHyperBreak();
	}

	/// <summary>
	/// 하이퍼 모드 비활성화
	/// </summary>
	public void InactiveHyperMode()
	{
		Reset();
		UnitManager.it.Player.InactiveHyperEffect();
	}

	public void Update(float _dt)
	{
		switch (state)
		{
			case State.Charge:
				{
					//하이퍼 모드 발동가능여부 체크
					if (auto && IsActiveHyperMode())
					{
						ActiveHyperMode();
					}
				}
				break;

			case State.HyperMode:
				{
					hyperBreakGauge += 1.0f / UserInfo.UserGrade.hyperDuration * _dt;
					if (hyperBreakGauge > 1)
					{
						hyperBreakGauge = 1;
					}

					// 하이퍼 브레이크 발동가능여부 체크
					if (auto && IsActiveHyperBreak())
					{
						UseHyperBreak();
					}
				}
				break;
		}
	}

	/// <summary>
	/// 하이퍼 보너스 어빌리티
	/// 하이퍼 모드가 꺼져있으면 0 리턴
	/// </summary>
	public float GetHyperAbilityRatio(UnitBase _unit, Stats _ability)
	{
		if (IsHyperMode == false)
		{
			return 0;
		}

		switch (_ability)
		{
			case Stats.Attackpower:
				return UserInfo.UserGrade.hyperAttackPower;

			case Stats.AttackSpeed:
				return UserInfo.UserGrade.hyperAttackSpeed;

			case Stats.Movespeed:
				return UserInfo.UserGrade.hyperMoveSpeed;

			case Stats.SuperCriticalDamage:
				return UserInfo.UserGrade.hyperCriticalAttackPower;

			default:
				return 0;
		}
	}
}
