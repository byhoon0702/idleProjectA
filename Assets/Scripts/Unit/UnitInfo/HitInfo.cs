using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AffectedInfo
{
	public Sprite sprite;
	public IdleNumber amount = new IdleNumber();
	public Color fontColor = Color.white;

	public LayerMask LayerMask { get; protected set; }

	public LayerMask targetLayer;
	public AffectedInfo(LayerMask layerMask)
	{
		LayerMask = layerMask;
	}

}
// 게임오브젝트 관련 정보가 여기 있으면 안됨
public class HitInfo : AffectedInfo
{
	public float criticalChanceMul = 1;
	public float CriticalX2ChanceMul = 1;

	public string hitSound = string.Empty;
	public bool IsPlayerCast;


	public bool ShakeCamera
	{
		get
		{
			return false;
		}
	}


	public IdleNumber TotalAttackPower;


	public CriticalType criticalType;


	public HitInfo(LayerMask layerMask, IdleNumber _attackPower, CriticalType criticalType = CriticalType.Normal) : base(layerMask)
	{

		amount = _attackPower;
		this.criticalType = criticalType;

		TotalAttackPower = amount * criticalChanceMul * CriticalX2ChanceMul;

		if (LayerMask == LayerMask.NameToLayer("Enemy"))
		{
			targetLayer = LayerMask.NameToLayer("Player");
		}
		else
		{
			targetLayer = LayerMask.NameToLayer("Enemy");
		}
	}

	//public HitInfo(UnitBase caster, IdleNumber _attackPower, bool _isSkill = false)
	//{
	//	if (caster is PlayerUnit)
	//	{
	//		IsPlayerCast = true;
	//	}

	//	else
	//	{
	//		IsPlayerCast = false;
	//	}

	//	attackPower = _attackPower;
	//	isSkill = _isSkill;
	//}

	//public HitInfo(IdleNumber _attackPower, bool _isSkill)
	//{
	//	attackPower = _attackPower;
	//	isSkill = _isSkill;
	//}
}
public enum AttackerType
{
	Unknown,
	Player,
	Pet,
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
