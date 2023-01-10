using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleRecord
{
	public List<RecordData> records = new List<RecordData>();
	private Dictionary<Int32, string> nameDictionary = new Dictionary<int, string>();


	public void InitCharacter(List<Character> _characters)
	{
		foreach (var character in _characters)
		{
			if (GetCharacterRecord(character.charID) != null)
			{
				continue;
			}
			RecordData record = new RecordData(character.charID);
			records.Add(record);
		}
	}

	public void InitCharacter(Character character)
	{
		if (GetCharacterRecord(character.charID) != null)
		{
			return;
		}
		RecordData record = new RecordData(character.charID);
		records.Add(record);
	}

	private string GetName(Int32 _charID)
	{
		if (nameDictionary.ContainsKey(_charID) == false)
		{
			Character character = CharacterManager.it.GetCharacter(_charID);
			if (character != null)
			{
				nameDictionary.Add(_charID, character.info.charNameAndCharId);
			}
			else
			{
				nameDictionary.Add(_charID, $"Unknown({_charID})");
			}
		}

		return nameDictionary[_charID];
	}

	public void RecordAttackPower(Int32 _charID, Int32 _targetCharID, string _attackName, IdleNumber _attackPower, bool _critical)
	{
		AttackPowerRecord attackPower = new AttackPowerRecord(_targetCharID, _attackName, _attackPower, _critical);

		VLog.BattleRecordLog($"{_attackName} 공격. {GetName(_charID)} -> {GetName(_targetCharID)} atkPower{_attackPower.ToString()}, cri: {_critical}");


		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				record.attackPowers.Add(attackPower);
				UIController.it.UpdateBattleRecord();
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.attackPowers.Add(attackPower);

		records.Add(newRecord);
		UIController.it.UpdateBattleRecord();
	}

	public void RecordHeal(Int32 _charID, Int32 _targetCharID, string _healName, IdleNumber _heal)
	{
		HealRecord heal = new HealRecord(_targetCharID, _healName, _heal);
		VLog.BattleRecordLog($"{_healName} 회복. {GetName(_charID)} -> {GetName(_targetCharID)} heal: {_heal.ToString()}");

		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				record.heals.Add(heal);
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.heals.Add(heal);

		records.Add(newRecord);
	}

	public RecordData GetCharacterRecord(Int32 _charID)
	{
		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				return record;
			}
		}

		return null;
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
		IdleNumber totalDamage = new IdleNumber(0, 0);
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
