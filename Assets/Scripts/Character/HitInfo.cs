using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 게임오브젝트 관련 정보가 여기 있으면 안됨
public class HitInfo
{
	public AttackerType attackerType;
	public AttackType attackType = AttackType.NOATTACK;
	public IdleNumber attackPower = new IdleNumber();
	public Color fontColor = Color.white;
	public float criticalChanceMul = 1;
	public float CriticalX2ChanceMul = 1;

	public bool IsPlayerAttack => attackerType == AttackerType.Player || attackerType == AttackerType.Companion;

	public string hitSound = string.Empty;
	public string hitEffect
	{
		get
		{
			if(attackerType != AttackerType.Player)
			{
				return "FX_Melee_Attacked_lighting_01";
			}
			else
			{
				return "";
			}
		}
	}

	public bool ShakeCamera
	{
		get
		{
			if (attackerType != AttackerType.Player)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}

	public IdleNumber TotalAttackPower
	{
		get 
		{
			return attackPower * criticalChanceMul * CriticalX2ChanceMul;
		}
	}

	public CriticalType criticalType
	{
		get
		{
			if(CriticalX2ChanceMul > 1)
			{
				return CriticalType.CriticalX2;
			}
			else if(criticalChanceMul > 1)
			{
				return CriticalType.Critical;
			}
			else
			{
				return CriticalType.Normal;
			}
		}
	}

	public HitInfo(AttackerType _attackerType, AttackType _attackType, IdleNumber _attackPower)
	{
		attackerType = _attackerType;
		attackType = _attackType;
		attackPower = _attackPower;
	}
}

public enum AttackerType
{
	Unknown,
	Player,
	Companion,
	Enemy
}

public enum CriticalType
{ 
	/// <summary>
	/// 일반(치명타 아님)
	/// </summary>
	Normal,
	/// <summary>
	/// 크리티컬
	/// </summary>
	Critical,
	/// <summary>
	/// 크리티컬두배
	/// </summary>
	CriticalX2
}
