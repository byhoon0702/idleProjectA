using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum UnitCondition
{
	None,
	/// <summary>
	/// 넉백
	/// </summary>
	Knockback,
	/// <summary>
	/// 방어력 감소
	/// </summary>
	ReducedDefense,
	/// <summary>
	/// 공격속도 증가
	/// </summary>
	AttackSpeedUp,
	/// <summary>
	/// 이동속도 증가
	/// </summary>
	MoveSpeedUp,
	/// <summary>
	/// 스턴
	/// </summary>
	Stun,
	/// <summary>
	/// 독
	/// </summary>
	Poison,
	/// <summary>
	/// 공격력 증가
	/// </summary>
	DamageUp,
	/// <summary>
	/// 크리티컬확률 증가
	/// </summary>
	CriticalUp
}
