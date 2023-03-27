using System;
using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;


[System.Serializable]
public class SkillSlot : DataSlot
{
	public int index;
	public RuntimeData.SkillInfo item;

	public long itemTid
	{
		get
		{
			return item != null ? item.tid : 0;
		}
	}
	public Sprite icon
	{
		get
		{
			if (item == null || item.icon == null)
			{
				return null;
			}
			return item.icon;
		}
	}

	public bool Equip(RuntimeData.SkillInfo info)
	{
		if (item != null && item.tid == info.tid)
		{
			return false;
		}

		item = info;
		VGameManager.it.userDB.UpdateUserStats();

		return true;
	}

	public override void AddEquipValue(ref IdleNumber _value, Ability type)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.itemObject.EquipAbilities.Length; i++)
		{
			if (item.itemObject.EquipAbilities[i].type == type)
			{
				_value += item.itemObject.EquipAbilities[i].GetValue(item.level);
			}
		}
	}

	public override void AddOwnedValue(ref IdleNumber _value, Ability type)
	{
		if (item == null || item.itemObject == null)
		{
			return;
		}
		for (int i = 0; i < item.itemObject.OwnedAbilities.Length; i++)
		{
			if (item.itemObject.OwnedAbilities[i].type == type)
			{
				_value += item.itemObject.OwnedAbilities[i].GetValue(item.level);
			}
		}
	}
}

[CreateAssetMenu(fileName = "SkillContainer", menuName = "ScriptableObject/Container/SkillContainer", order = 1)]
public class SkillContainer : BaseContainer
{
	public List<SkillInfo> skillList { get; private set; } = new List<SkillInfo>();
	public SkillSlot this[int index]
	{
		get
		{
			return skillSlot[index];
		}

	}

	[NonSerialized] public SkillSlot[] skillSlot;

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		int last = 6;
		skillSlot = new SkillSlot[last];


		//임시로 데이터 설정
		for (int i = 0; i < last; i++)
		{
			skillSlot[i] = new SkillSlot();
			//skillSlot[i].item = parent.inventory.skillList[i];
		}
	}
}
