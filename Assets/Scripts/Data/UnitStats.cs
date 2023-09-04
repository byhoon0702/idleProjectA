using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

using UnityEngine;

public enum BuffType
{

}

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

	Buff_Gain_Gold = 140,
	Buff_Gain_Exp = 150,
	Buff_Gain_Item = 160,

	Final_Damage_Buff = 170,
	Damage_Reduce = 180,

	Knockback_Resist = 190,
}

public enum StatModeType
{
	None = 0,
	/// <summary>
	/// 원본 값에 더하기
	/// </summary>
	Add = 1,
	/// <summary>
	/// 원본 값에 곱하기
	/// </summary>
	Multi = 100,
	/// <summary>
	/// 버프
	/// </summary>
	Buff = 130,

	/// <summary>
	/// 광고 버프
	/// </summary>
	AdsBuff = 150,
	/// <summary>
	/// 하이퍼 버프 
	/// </summary>
	Hyper = 200,


	SkillBuff = 300,
	/// <summary>
	/// 디버프
	/// </summary>
	SkillDebuff = 500,


	Replace = 1000,
}


public class StatsModifier
{
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

	//public void Init()
	//{
	//	BaseValue = (IdleNumber)baseValue;
	//}

	//public void SetBaseValue(IdleNumber value)
	//{
	//	BaseValue = value;
	//}
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
public class UnitStats
{
	public List<Stat> stats;

	public void CopyTo(UnitStats _stats)
	{
		Stat[] array = new Stat[stats.Count];
		stats.CopyTo(array, 0);
		_stats.stats = new List<Stat>(array);
	}
	public void Load()
	{

	}

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
	public void RemoveAllModifiers(object source)
	{
		for (int i = 0; i < stats.Count; i++)
		{
			bool remove = stats[i].RemoveAllModifiersFromSource(source);
			if (remove)
			{
				VLog.Log($"Remove Modifier {source}");
			}
		}
	}
	public void RemoveAllModifiers(StatModeType type)
	{
		for (int i = 0; i < stats.Count; i++)
		{
			stats[i].RemoveAllModifiersFromSource(type);
		}
	}

	public void RemoveModifier(StatsType type, object source)
	{
		GetStat(type)?.RemoveAllModifiersFromSource(source);
	}
	public void Generate()
	{
		StatsType[] types = (StatsType[])System.Enum.GetValues(typeof(StatsType));
		stats = new List<Stat>();
		stats.Add(new Stat() { type = StatsType.Atk, baseValue = "10" });
		stats.Add(new Stat() { type = StatsType.Hp, baseValue = "100" });
		stats.Add(new Stat() { type = StatsType.Hp_Recovery, baseValue = "5" });
		stats.Add(new Stat() { type = StatsType.Crits_Chance, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Crits_Damage, baseValue = "120" });
		stats.Add(new Stat() { type = StatsType.Super_Crits_Chance, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Super_Crits_Damage, baseValue = "120" });
		stats.Add(new Stat() { type = StatsType.Atk_Speed, baseValue = "100" });
		stats.Add(new Stat() { type = StatsType.Move_Speed, baseValue = "100" });
		stats.Add(new Stat() { type = StatsType.Skill_Cooltime, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Skill_Damage, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Mob_Damage_Buff, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Boss_Damage_Buff, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Buff_Gain_Gold, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Buff_Gain_Exp, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Buff_Gain_Item, baseValue = "0" });
		stats.Add(new Stat() { type = StatsType.Final_Damage_Buff, baseValue = "0" });
		//for (int i = 0; i < types.Length; i++)
		//{
		//	Stat item = new Stat();
		//	item.type = types[i];
		//	item.baseValue = "0";
		//	item.name = types[i].ToString();
		//	stats.Add(item);
		//}

	}

	public void UpdateAll()
	{
		for (int i = 0; i < stats.Count; i++)
		{
			stats[i].SetDirty();
		}
	}

	public IdleNumber GetTotalPower()
	{
		IdleNumber totalPower = (IdleNumber)0;
		IdleNumber atkPower = GetStat(StatsType.Atk).Value;
		IdleNumber atkSpeed = GetStat(StatsType.Atk_Speed).Value / 100f;

		IdleNumber averageAtk = atkPower / atkSpeed;

		IdleNumber critChance = GetStat(StatsType.Crits_Chance).Value;
		IdleNumber critDamage = GetStat(StatsType.Crits_Damage).Value / 100f;
		IdleNumber superCritChance = GetStat(StatsType.Super_Crits_Chance).Value;
		IdleNumber superCritDmg = GetStat(StatsType.Super_Crits_Damage).Value / 100f;

		IdleNumber critResult = averageAtk * critChance * (critDamage);
		IdleNumber superCritResult = critResult * superCritChance * superCritDmg;

		critResult.Check();
		superCritResult.Check();
		totalPower = atkPower + averageAtk + critResult + superCritResult;
		totalPower.Turncate();
		return totalPower;
	}
}
