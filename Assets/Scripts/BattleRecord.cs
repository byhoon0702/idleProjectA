using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BattleRecord
{
	public DPSInfo playerDPS = new DPSInfo();
	public DPSInfo petDPS = new DPSInfo();
	public DPSInfo enemyDPS = new DPSInfo();
	public DPSInfo unknownDPS = new DPSInfo();
	public int killCount;
	public int bossKillCount;
	public IdleNumber totalDamage;


	public void RecordAttackPower(HitInfo _hitInfo)
	{
		//DPSInfo info = FinddDPSInfo(_hitInfo.attackerType);

		//info.attackPower += _hitInfo.TotalAttackPower;
		//info.criticalCount += _hitInfo.criticalType == CriticalType.Critical ? 1 : 0;
		//info.hyperAttackCount += _hitInfo.criticalType == CriticalType.CriticalX2 ? 1 : 0;
	}

	/// <summary>
	/// _healValue는 실제로 회복된 량을 넣어줘야 함. (최대체력보다 높게 충전되는경우)
	/// </summary>
	public void RecordHeal(HealInfo _heal, IdleNumber _healValue)
	{
		DPSInfo info = FinddDPSInfo(_heal.healer);

		info.hpRecovery += _healValue;
	}

	private DPSInfo FinddDPSInfo(AttackerType attackerType)
	{
		switch (attackerType)
		{
			case AttackerType.Player:
				return playerDPS;
			case AttackerType.Pet:
				return petDPS;
			case AttackerType.Enemy:
				return enemyDPS;
			default:
				return unknownDPS;
		}
	}
}

public class RecordData
{
	public Int32 charID;
	public List<AttackPowerRecord> attackPowers = new List<AttackPowerRecord>();
	public List<HealRecord> heals = new List<HealRecord>();

	public RecordData(Int32 _charID)
	{
		charID = _charID;
	}

	public string ToStringEditor()
	{
		IdleNumber totalAttackPower = new IdleNumber();
		Int32 criticalAttackPowerCount = 0;

		foreach (var attackPower in attackPowers)
		{
			totalAttackPower += attackPower.value;
			if (attackPower.isCritical)
			{
				criticalAttackPowerCount++;
			}
		}

		IdleNumber totalHeal = new IdleNumber();
		foreach (var heal in heals)
		{
			totalHeal += heal.value;
		}

		return $"AttackPower: {totalAttackPower.ToString()}, CriCount: {criticalAttackPowerCount}, Heal: {totalHeal.ToString()}";
	}

	public IdleNumber TotalDamage()
	{
		IdleNumber totalDamage = new IdleNumber(0);
		for (int i = 0; i < attackPowers.Count; i++)
		{
			var attackPower = attackPowers[i];
			totalDamage += attackPower.value;
		}
		return totalDamage;
	}
}

public struct AttackPowerRecord
{
	public Int32 targetCharID;
	public string attackName;
	public IdleNumber value;
	public bool isCritical;

	public AttackPowerRecord(Int32 _targetCharID, string _attackName, IdleNumber _value, bool _isCritical)
	{
		targetCharID = _targetCharID;
		attackName = _attackName;
		value = _value;
		isCritical = _isCritical;
	}
}

public struct HealRecord
{
	public Int32 targetCharID;
	public string healName;
	public IdleNumber value;

	public HealRecord(Int32 _targetCharID, string _healName, IdleNumber _value)
	{
		targetCharID = _targetCharID;
		healName = _healName;
		value = _value;
	}
}


[Serializable]
public class DPSInfo
{
	public IdleNumber attackPower = new IdleNumber();
	public IdleNumber hpRecovery = new IdleNumber();
	public int criticalCount = 0;
	public int hyperAttackCount = 0;

	public List<DPSSkillInfo> skillData = new List<DPSSkillInfo>();

	public override string ToString()
	{
		string outString = $"AttackPower: {attackPower.ToString()}, Cri: {criticalCount}, hyp: {hyperAttackCount}\n HP Recovery: {hpRecovery.ToString()}\n";
		foreach (var v in skillData)
		{
			outString += $"[{v.skillTid}] {v.skillValue}\n";
		}

		return outString;
	}
}

[Serializable]
public class DPSSkillInfo
{
	public long skillTid;
	public IdleNumber skillValue;
}
