using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PetType
{ }


[System.Serializable]
public class PetSlot : DataSlot
{

	public RuntimeData.PetInfo item;

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
	public bool Equip(long tid)
	{
		if (tid == 0)
		{
			return false;
		}

		var info = VGameManager.UserDB.inventory.FindPetItem(tid);

		item = info;

		VGameManager.it.userDB.UpdateUserStats();
		if (item == null)
		{

		}
		else
		{
			if (info.itemObject == null)
			{

			}
			else
			{

			}
		}

		return true;
	}


	public bool Equip(RuntimeData.PetInfo info)
	{
		if (item != null && item.tid == info.tid)
		{
			return false;
		}

		item = info;

		VGameManager.it.userDB.UpdateUserStats();
		if (item == null)
		{

		}
		else
		{

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

[CreateAssetMenu(fileName = "PetContainer", menuName = "ScriptableObject/Container/PetContainer", order = 1)]
public class PetContainer : BaseContainer
{
	public PetSlot this[int i]
	{
		get
		{
			return petSlot[i];
		}

	}

	public PetSlot[] petSlot;
	[SerializeField] private int slotCount;
	public override void Load(UserDB _parent)
	{
		parent = _parent;

		petSlot = new PetSlot[slotCount];


		for (int i = 0; i < slotCount; i++)
		{
			petSlot[i] = new PetSlot();
			//임시로 데이터 설정
			if (parent.inventory.petList[i].count > 0)
			{
				petSlot[i].item = parent.inventory.petList[i];
			}
		}
	}

	public void Equip(long tid)
	{
		for (int i = 0; i < petSlot.Length; i++)
		{
			if (petSlot[i].item == null)
			{
				petSlot[i].Equip(tid);
				break;
			}
		}
	}

	public void Unequip(long tid)
	{
		for (int i = 0; i < petSlot.Length; i++)
		{
			if (petSlot[i].item == null)
			{
				continue;
			}
			if (petSlot[i].item.tid == tid)
			{
				petSlot[i].item = null;
				break;

			}
		}
	}

}
