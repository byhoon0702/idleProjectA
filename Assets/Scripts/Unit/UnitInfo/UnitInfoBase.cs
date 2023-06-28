
using UnityEngine;
using System.Collections.Generic;
using System;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 현재 능력치
/// </summary>
[Serializable]
public class StatInfo
{
	public StatsType type { get; private set; }

	public IdleNumber value;

	public StatusData rawData
	{
		get; private set;
	}

	public StatInfo(StatusData _data)
	{
		rawData = _data;
		type = rawData.type;
	}

	public StatInfo(StatsType _type, IdleNumber _value)
	{
		type = _type;
		value = _value;
	}

}

public class UnitInfo
{
	public Unit owner;
	public UnitData data;
	public UnitData rawData;

	public SerializableDictionary<StatsType, StatInfo> rawStatus = new SerializableDictionary<StatsType, StatInfo>();
	public UnitStats stats;

	public IdleNumber hp;

	protected IdleNumber _maxHp;
	public IdleNumber maxHp
	{
		get { return _maxHp; }
		set { _maxHp = value; }
	}
	public IdleNumber prevMaxHp;

	public IdleNumber attackPower;

	public virtual UnitAdvancementInfo upgradeInfo
	{
		get
		{
			if (data.upgradeInfoList == null)
			{
				return new UnitAdvancementInfo();
			}
			if (data.upgradeInfoList.Count == 0)
			{
				return new UnitAdvancementInfo();
			}

			return data.upgradeInfoList[0];
		}
	}

	public virtual string resource
	{
		get
		{

			if (upgradeInfo == null || upgradeInfo.level == 0 || upgradeInfo.resource.IsNullOrEmpty())
			{
				return data.resource;
			}
			return upgradeInfo.resource;
		}
	}
	public virtual bool HyperAvailable { get; protected set; }

	public UnitInfo()
	{

	}
	public UnitInfo(UnitData _data)
	{
		rawData = _data;
		data = _data.Clone();

		//for (int i = 0; i < upgradeInfo.stats.Count; i++)
		//{
		//	var statusdata = DataManager.Get<StatusDataSheet>().GetData((long)upgradeInfo.stats[i].type);

		//	StatInfo statinfo = new StatInfo(statusdata);
		//	rawStatus.Add(statusdata.type, statinfo);
		//	rawStatus[statusdata.type].value = (IdleNumber)data.statusDataList[i].value;
		//}
	}
	public UnitInfo(Unit _owner, UnitData _data)
	{
		owner = _owner;

		rawData = _data;
		data = _data.Clone();

		//for (int i = 0; i < data.statusDataList.Count; i++)
		//{
		//	var statusdata = DataManager.Get<StatusDataSheet>().GetData((long)data.statusDataList[i].type);

		//	StatInfo statinfo = new StatInfo(statusdata);
		//	rawStatus.Add(statusdata.type, statinfo);
		//	rawStatus[statusdata.type].value = (IdleNumber)data.statusDataList[i].value;

		//	var abilityData = rawStatus[statusdata.type];
		//	UserAbility ability = new UserAbility(abilityData.type, abilityData.value, abilityData.rawData.MinValue(), abilityData.rawData.MaxValue());
		//	stats.abilities.Add(ability);

		//}

	}
	public virtual IdleNumber AttackPower()
	{
		return new IdleNumber();
	}

	/// <summary>
	/// 공격속도
	/// </summary>
	public virtual float AttackSpeed()
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

	}
}
