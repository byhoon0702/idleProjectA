
using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// 현재 능력치
/// </summary>
[Serializable]
public class StatInfo
{
	public Stats type => rawData.type;

	public IdleNumber value;

	public StatusData rawData
	{
		get; private set;
	}

	public StatInfo(StatusData _data)
	{
		rawData = _data;
	}
}

public class UnitInfo
{
	public Unit owner;
	public UnitData data;
	public UnitData rawData;

	public Dictionary<Stats, StatInfo> status = new Dictionary<Stats, StatInfo>();

	public IdleNumber hp
	{
		set
		{
			status[Stats.Hp].value = value;
		}
		get
		{
			return status[Stats.Hp].value;
		}
	}
	public IdleNumber maxHP;
	public IdleNumber attackPower;

	protected IdleNumber rawHp;
	protected IdleNumber rawAttackPower;

	//public ProjectileData projectileData;
	public SkillEffectData normalSkillEffectData;

	public UnitInfo()
	{

	}
	public UnitInfo(UnitData _data)
	{
		rawData = _data;
		data = _data.Clone();

		for (int i = 0; i < data.statusDataList.Count; i++)
		{
			var statusdata = DataManager.Get<StatusDataSheet>().GetData((long)data.statusDataList[i].type);

			StatInfo statinfo = new StatInfo(statusdata);
			status.Add(statusdata.type, statinfo);
			status[statusdata.type].value = (IdleNumber)data.statusDataList[i].value;
		}
	}
	public UnitInfo(Unit _owner, UnitData _data)
	{
		owner = _owner;

		rawData = _data;
		data = _data.Clone();

		for (int i = 0; i < data.statusDataList.Count; i++)
		{
			var statusdata = DataManager.Get<StatusDataSheet>().GetData((long)data.statusDataList[i].type);

			StatInfo statinfo = new StatInfo(statusdata);
			status.Add(statusdata.type, statinfo);
			status[statusdata.type].value = (IdleNumber)data.statusDataList[i].value;
		}

	}
	public virtual IdleNumber AttackPower()
	{
		return new IdleNumber();
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public virtual float AttackSpeedMul()
	{
		return 1;
	}

	public virtual IdleNumber HPRecovery()
	{
		return new IdleNumber();
	}

	/// <summary>
	/// 크리티컬 발동여부
	/// </summary>
	public virtual CriticalType IsCritical()
	{
		return CriticalType.Normal;
	}


	public virtual float CriticalChanceRatio()
	{
		return 1;
	}

	public virtual float CriticalX2ChanceRatio()
	{
		return 1;
	}

	/// <summary>
	/// 크리티컬 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public virtual float CriticalDamageMultifly()
	{
		return 1;
	}


	/// <summary>
	/// 크리티컬X2 대미지 총 증가량(줄 대미지에 곱하면 됩니다)
	/// </summary>
	public virtual float CriticalX2DamageMultifly()
	{
		return 1;
	}

	/// <summary>
	/// 이동속도
	/// </summary>
	/// <returns></returns>
	public virtual float MoveSpeed()
	{
		return 1;
	}

	public virtual void SetProjectile(long tid)
	{

		//ProjectileData data = DataManager.Get<ProjectileDataSheet>().GetData(tid);

		//projectileData = data.Clone();

		SkillEffectData data = DataManager.Get<SkillEffectDataSheet>().GetData(tid);

		normalSkillEffectData = data;
	}

	protected void CalculateBaseAttackPowerAndHp(int _level = 1)
	{
		for (int i = data.statusPerLevels.Count - 1; i >= 0; i--)
		{
			var item = data.statusPerLevels[i];
			if (_level >= item.level)
			{
				attackPower = (IdleNumber)(item.attackPower) + (IdleNumber)(_level - item.level) * (IdleNumber)item.attackPowerPerLevel;

				IdleNumber hpPerLevel = (IdleNumber)(item.hpPerLevel);

				hp = (IdleNumber)(item.hp) * Mathf.Pow((float)(hpPerLevel.Value), (_level - item.level));
				rawAttackPower = attackPower;
				rawHp = hp;
				maxHP = rawHp;
				break;
			}
		}
	}
}
