﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleRecord
{
	public List<RecordData> records = new List<RecordData>();

	public void InitCharacter(List<Character> _characters)
	{
		foreach (var character in _characters)
		{
			RecordData record = new RecordData(character.charID);
			records.Add(record);
		}
	}

	public void RecordAttackPower(Int32 _charID, IdleNumber _attackPower, bool _critical)
	{
		foreach(var record in records)
		{
			if(record.charID == _charID)
			{
				record.attackPower += _attackPower;
				if(_critical)
				{
					record.criticalCount++;
				}
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.attackPower += _attackPower;
		if (_critical)
		{
			newRecord.criticalCount++;
		}

		records.Add(newRecord);
	}

	public void RecordHeal(Int32 _charID, IdleNumber _heal)
	{
		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				record.heal += _heal;
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.heal += _heal;

		records.Add(newRecord);
	}

	public void RecordAttackCount(Int32 _charID)
	{
		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				record.attackCount++;
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.attackCount++;

		records.Add(newRecord);
	}

	public void RecordSkillCount(Int32 _charID)
	{
		foreach (var record in records)
		{
			if (record.charID == _charID)
			{
				record.skillCount++;
				return;
			}
		}

		RecordData newRecord = new RecordData(_charID);
		newRecord.skillCount++;

		records.Add(newRecord);
	}

	public RecordData GetCharacterRecord(Int32 _charID)
	{
		foreach(var record in records)
		{
			if(record.charID == _charID)
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
	public IdleNumber attackPower = new IdleNumber();
	public Int32 criticalCount;
	public IdleNumber heal = new IdleNumber();
	public Int32 attackCount;
	public Int32 skillCount;

	public RecordData(Int32 _charID)
	{
		charID = _charID;
	}

	public string ToStringEditor()
	{
		return $"AttackPower: {attackPower.ToString()}, Heal: {heal.ToString()}, AtkCount: {attackCount}, CriCount: {criticalCount}, SklCount: {skillCount}";
	}
}
