using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

public enum StatsType
{
	None,
	/// <summary>
	/// 공격력
	/// </summary>
	Atk = 1,
	/// <summary>
	/// 공격력 증폭
	/// </summary>
	Atk_Buff,
	/// <summary>
	/// 체력
	/// </summary>
	Hp = 10,
	/// <summary>
	/// 체력 증폭
	/// </summary>
	Hp_Buff,

	Hp_Recovery = 20,
	Hp_Recovery_Buff,
	/// <summary>
	/// 치명타
	/// </summary>
	Crits_Chance = 30,

	Crits_Damage = 40,

	Super_Crits_Chance = 50,

	Super_Crits_Damage = 60,

	Atk_Speed = 70,

	Move_Speed = 80,

	Skill_Cooltime = 90,
	Skill_Damage = 100,

	Mob_Damage_Buff = 110,
	Boss_Damage_Buff = 120,

	Evasion = 130,
	Evasion_Buff,

	Gold_Buff = 140,
	EXP_Buff = 150,

	Item_Buff = 160,
	Final_Damage_Buff = 170,
	Damage_Reduce = 180,

	Hyper_Atk = 200,
	Hyper_Hp = 210,
	Hyper_Atk_Speed = 220,
	Hyper_Move_Speed = 230,
	Hyper_Duration = 240,

}

public enum StatModeType
{
	None = 0,
	Replace = 1,
	Flat = 100,
	FlatAdd = 130,
	FlatMulti = 150,
	PercentAdd = 200,
	PercentMulti = 300,
}


public class StatsModifier
{
	public bool calcWithBase;
	public readonly IdleNumber Value;
	public readonly StatModeType Type;
	public readonly int Order;
	public readonly object Source;

	public StatsModifier(IdleNumber value, StatModeType type, int order, object source)
	{
		Value = value;
		Type = type;
		Order = order;
		Source = source;
	}

	public StatsModifier(IdleNumber value, StatModeType type) : this(value, type, (int)type, null) { }
	public StatsModifier(IdleNumber value, StatModeType type, int order) : this(value, type, order, null) { }
	public StatsModifier(IdleNumber value, StatModeType type, object source) : this(value, type, (int)type, source) { }


	public bool RemoveModifier()
	{
		return true;
	}
}

[System.Serializable]
public class Stat : ModifyInfo
{
	[HideInInspector] public string name;
	public StatsType type;
	public string baseValue;

	public Stat() : base()
	{
		BaseValue = (IdleNumber)baseValue;

	}

	public void Init()
	{
		BaseValue = (IdleNumber)baseValue;
	}

	public void SetBaseValue(IdleNumber value)
	{
		BaseValue = value;
	}
	public override void SetDirty()
	{
		isDirty = true;
		if ((baseValue != "0" || baseValue != "") && BaseValue == 0)
		{
			BaseValue = (IdleNumber)baseValue;
		}
	}
}


[CreateAssetMenu(fileName = "Unit Stats", menuName = "ScriptableObject/Unit Stats", order = 1)]
public class UnitStats : ScriptableObject
{
	public List<Stat> stats;

	public IdleNumber GetValue(StatsType type)
	{
		var stat = stats.Find(x => x.type == type);
		if (stat == null)
		{
			return (IdleNumber)0;
		}
		return stat.Value;
	}
	public IdleNumber GetBaseValue(StatsType type)
	{
		var stat = stats.Find(x => x.type == type);
		if (stat == null)
		{
			return (IdleNumber)0;
		}
		return stat.BaseValue;
	}

	public void AddModifier(StatsType type, StatsModifier modifier)
	{


		GetStat(type)?.AddModifiers(modifier);
	}

	private Stat GetStat(StatsType type)
	{
		if (stats == null)
		{
			stats = new List<Stat>();
		}
		StatsType effectType = type;
		if (type == StatsType.Atk_Buff)
		{
			effectType = StatsType.Atk;
		}
		if (type == StatsType.Hp_Buff)
		{
			effectType = StatsType.Hp;
		}


		Stat stat = stats.Find(x => x.type == effectType);
		if (stat == null)
		{
			stat = new Stat();
			stat.type = effectType;
			stat.baseValue = "0";
			stat.name = effectType.ToString();
			stats.Add(stat);
		}

		return stat;
	}

	public void UpdataModifier(StatsType type, StatsModifier modifier)
	{
		GetStat(type)?.UpdateModifiers(modifier);
	}

	public void RemoveModifier(StatsType type, object source)
	{

		GetStat(type)?.RemoveAllModifiersFromSource(source);
	}



	public void Generate()
	{
		StatsType[] types = (StatsType[])System.Enum.GetValues(typeof(StatsType));
		stats = new List<Stat>();
		for (int i = 0; i < types.Length; i++)
		{
			Stat item = new Stat();
			item.type = types[i];
			item.baseValue = "0";
			item.name = types[i].ToString();
			stats.Add(item);
		}

	}

	public void UpdateAll()
	{
		for (int i = 0; i < stats.Count; i++)
		{
			stats[i].SetDirty();

		}
	}
}
