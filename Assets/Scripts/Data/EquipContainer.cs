using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using RuntimeData;
using UnityEditor.Build.Content;
using UnityEngine;

public enum EquipType
{
	WEAPON = 0,
	ARMOR = 1,
	NECKLACE = 2,
	RING = 3,

	_END,

}

[System.Serializable]
public class EquipSlot : DataSlot
{
	public EquipType type;
	public RuntimeData.EquipItemInfo item;

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
			if (item == null || item.itemObject == null)
			{
				return null;
			}
			return item.itemObject.Icon;
		}
	}


	public bool Equip(RuntimeData.EquipItemInfo info)
	{
		if (item != null && item.tid == info.tid)
		{
			return false;
		}

		item = info;



		VGameManager.it.userDB.UpdateUserStats();
		if (item == null)
		{

			//UnitManager.it.Player.unitAnimation.SetDefaultWeapon();
		}
		else
		{
			if (info.itemObject == null)
			{
				//UnitManager.it.Player.unitAnimation.SetDefaultWeapon();
			}
			else
			{
				var data = DataManager.Get<EquipItemDataSheet>().Get(item.tid);
				if (data.costumeTid > 0)
				{
					VGameManager.it.userDB.costumeContainer.Equip(data.costumeTid);
					UnitManager.it.Player.ChangeCostume();
				}
			}
		}

		return true;
	}

	public override void AddEquipValue(ref IdleNumber _value, Ability _type)
	{
		if (item == null || item.itemObject == null)
		{
			return;
		}
		for (int i = 0; i < item.itemObject.EquipAbilities.Length; i++)
		{
			if (item.itemObject.EquipAbilities[i].type == _type)
			{
				_value += item.itemObject.EquipAbilities[i].GetValue(item.level);
			}
		}
	}

	public override void AddOwnedValue(ref IdleNumber _value, Ability _type)
	{
		if (item == null || item.itemObject == null)
		{
			return;
		}
		for (int i = 0; i < item.itemObject.OwnedAbilities.Length; i++)
		{
			if (item.itemObject.OwnedAbilities[i].type == _type)
			{
				_value += item.itemObject.OwnedAbilities[i].GetValue(item.level);
			}
		}
	}
}

[CreateAssetMenu(fileName = "EquipContainer", menuName = "ScriptableObject/Container/EquipContainer", order = 1)]
public class EquipContainer : BaseContainer
{
	public EquipSlot this[EquipType _type]
	{
		get
		{
			return equipSlot[(int)_type];
		}

	}

	[NonSerialized] public EquipSlot[] equipSlot;

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		int last = (int)EquipType._END;
		equipSlot = new EquipSlot[last];

		for (int i = 0; i < last; i++)
		{
			equipSlot[i] = new EquipSlot();
			equipSlot[i].type = (EquipType)i;
		}

	}


	//public IdleNumber GetTotalAdd(Ability type)
	//{
	//	//for (int i = 0; i < equipSlot.Length; i++)
	//	//{
	//	//	if (equipSlot[i] == null)
	//	//	{
	//	//		continue;
	//	//	}
	//	//}
	//}
}
