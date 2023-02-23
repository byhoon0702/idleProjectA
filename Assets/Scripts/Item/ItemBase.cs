using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class ItemBase
{
	public InstantItem instantItem;
	public ItemData data;

	public long Tid => instantItem.tid;
	public ItemType Type => instantItem.type;
	public IdleNumber Count
	{
		get
		{
			return instantItem.count;
		}
		set
		{
			instantItem.count = value;
		}
	}
	public virtual string Icon => data.tid.ToString();
	public string ItemName => data.name;
	public int Level => instantItem.level;
	public virtual int MaxLevel => int.MaxValue;
	public bool IsMaxLv => Level >= MaxLevel;
	public virtual int Exp => instantItem.exp;
	public Grade Grade => instantItem.grade;

	public AbilityInfo EquipAbility => data.EquipAbilityInfo;
	public AbilityInfo ToOwnAbility => data.ToOwnAbilityInfo;



	public virtual VResult Setup(InstantItem _instantItem)
	{
		VResult result = new VResult();
		instantItem = _instantItem;

		data = DataManager.Get<ItemDataSheet>().Get(_instantItem.tid);
		if (data == null)
		{
			return result.SetFail(VResultCode.NO_META_DATA);
		}

		return result.SetOk();
	}
	public InstantItem DeepClone()
	{
		return instantItem.DeepClone() as InstantItem;
	}


	public virtual void AddExp(int _exp)
	{
		instantItem.exp += _exp;
	}

	public virtual void AddLevel(int _level)
	{
		instantItem.level += _level;
	}

	public override string ToString()
	{
		return $"[{ItemName}({Tid})] -  [{Grade}] count: {Count.ToString()}, exp: {Exp}";
	}
}
