using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class CostumeSlot : DataSlot
{
	public CostumeType type { get; private set; }
	public RuntimeData.CostumeInfo item { get; private set; }

	public void SetInfo(CostumeType _type, RuntimeData.CostumeInfo _item)
	{
		type = _type;
		item = _item;
	}
	public GameObject costume
	{
		get
		{
			if (item == null)
			{
				return null;
			}
			if (item.itemObject == null)
			{
				return null;
			}
			return item.itemObject.CostumeObject;
		}
	}
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


	public bool Equip(RuntimeData.CostumeInfo info)
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
				//UnitManager.it.Player.unitAnimation.ChangeWeapon(costume);
			}
		}

		return true;
	}

	public override void AddEquipValue(ref IdleNumber _value, Ability type)
	{
		if (item == null || item.itemObject == null)
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
[CreateAssetMenu(fileName = "Costume Container", menuName = "ScriptableObject/Container/Costume Container", order = 1)]
public class CostumeContainer : BaseContainer
{
	public CostumeSlot this[CostumeType _type]
	{
		get
		{
			return equipSlot[(int)_type];
		}

	}

	[NonSerialized] public CostumeSlot[] equipSlot;

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		int last = (int)CostumeType._END;
		equipSlot = new CostumeSlot[last];

		for (int i = 0; i < last; i++)
		{
			equipSlot[i] = new CostumeSlot();
			CostumeType type = (CostumeType)i;
			equipSlot[i].SetInfo(type, _parent.inventory[type][0]);
		}
	}

	public void Equip(long tid)
	{
		var info = parent.inventory.FindCostumeItem(tid);

		if (info == null)
		{
			return;
		}
		this[info.itemObject.Type].Equip(info);
	}
	public void Equip(long tid, CostumeType type)
	{
		var info = parent.inventory.FindCostumeItem(tid, type);

		if (info == null)
		{
			return;
		}
		this[type].Equip(info);
	}
}
