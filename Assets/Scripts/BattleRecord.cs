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
			RecordData record = new RecordData();
			record.charID = character.charID;

			records.Add(record);
		}
	}

	public void RecordDamage(Int32 _charID, IdleNumber _damage)
	{
		foreach(var record in records)
		{
			if(record.charID == _charID)
			{
				record.damage += _damage;
				return;
			}
		}
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
	public IdleNumber damage = new IdleNumber();
	public IdleNumber heal = new IdleNumber();
	public Int32 attackCount;
	public Int32 skillCount;

	public string ToStringEditor()
	{
		return $"Dmg: {damage.ToString()}, Heal: {heal.ToString()}, AtkCount: {attackCount}, SklCount: {skillCount}";
	}
}
