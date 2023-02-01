using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConditionDataBase : ICloneable
{
	public abstract object Clone();
}


/// <summary>
/// 컨디션 기본 클래스
/// </summary>
public abstract class ConditionBase
{
	#region Define
	/// <summary>
	/// 컨디션 타입
	/// </summary>
	public abstract CharacterCondition conditionType { get; }

	/// <summary>
	/// 컨디션이름(UI표시용)
	/// </summary>
	public string conditionName => GetType().ToString();

	/// <summary>
	/// 컨디션 키
	/// </summary>
	public string conditionKey => GetType().ToString();

	/// <summary>
	/// UI에 표시되는 아이콘 이미지 경로
	/// </summary>
	public abstract string iconPath { get; }

	/// <summary>
	/// 걸린 유닛에게 표시되는 이펙트
	/// </summary>
	public abstract string effectPath { get; }

	/// <summary>
	/// 이펙트가 재생되는 포지션. 계산은 월드 포지션으로 처리됨.
	/// </summary>
	public virtual Vector3 effectPosition => character.transform.position;

	public abstract BuffType buffType { get; }
	#endregion Define

	/// <summary>
	/// 컨디션이 적용되는 유닛. ConditionModule에 컨디션이 추가될때 초기화 된다.
	/// </summary>
	public Unit character;

	/// <summary>
	/// 나를 공격한 유닛
	/// </summary>
	protected UnitBase attacker { get; private set; }

	/// <summary>
	/// 컨디션 남은 시간
	/// </summary>
	public float remainTime { get; protected set; }

	/// <summary>
	/// 컨디션 지속시간.
	/// </summary>
	public float duration { get; protected set; }





	/// <summary>
	/// Duration이 0이면 지속시간이 없음
	/// 생성자에선 초기화만 할것.
	/// </summary>
	public ConditionBase(UnitBase _attacker, float _duration)
	{
		attacker = _attacker;
		duration = _duration;
	}

	/// <summary>
	/// 컨디션이 시작될 때, 호출된다. 필요한 데이터의 처리가 모두 끝난 상태이다.
	/// </summary>
	public virtual void Start()
	{
		remainTime = duration;
	}

	/// <summary>
	/// 컨디션이 적용중일 때, 지속적으로 호출된다
	/// </summary>
	public virtual void Update(float dt)
	{
		if (duration > 0)
		{
			remainTime -= dt;
		}
	}

	/// <summary>
	/// 컨디션이 종료되면 호출된다.
	/// </summary>
	public virtual void Finish()
	{
	}

	/// <summary>
	/// 컨디션을 리셋 시킨다.
	/// </summary>
	public virtual void Reset()
	{
		remainTime = duration;
	}

	/// <summary>
	/// 컨디션이 끝났다.
	/// </summary>
	/// <returns></returns>
	public virtual bool IsEnd()
	{
		return duration > 0 && remainTime <= 0;
	}
}


public static class ConditionUtility
{
	public static float GetDefaultBuffDuration(Grade _grade)
	{
		if (_grade == Grade.SSS)
		{
			return ConfigMeta.it.SSS_BUFF_DURATION;
		}
		else
		{
			return ConfigMeta.it.DSS_BUFF_DURATION;
		}
	}

	public static float GetDefaultDebuffDuration(Grade _grade)
	{
		if (_grade == Grade.SSS)
		{
			return ConfigMeta.it.SSS_DEBUFF_DURATION;
		}
		else
		{
			return ConfigMeta.it.DSS_DEBUFF_DURATION;
		}
	}
}
