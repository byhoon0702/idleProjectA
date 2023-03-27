using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
[System.Serializable]
public class InventoryObject : BaseContainer
{
	public List<InstantItem> itemList = new List<InstantItem>();

	public IdleNumber Gold;
	public IdleNumber Dia;

	public List<RuntimeData.EquipItemInfo> this[EquipType key]
	{
		get
		{
			switch (key)
			{
				case EquipType.WEAPON:
					return weaponItemList;
				case EquipType.ARMOR:
					return armorItemList;
				case EquipType.RING:
					return ringItemList;
				case EquipType.NECKLACE:
					return necklaceItemList;
			}

			return weaponItemList;
		}
	}

	public List<RuntimeData.CostumeInfo> this[CostumeType key]
	{
		get
		{
			switch (key)
			{
				case CostumeType.WEAPON:
					return costumeWeaponList;
				case CostumeType.BODY:
					return costumeBodyList;
				case CostumeType.HEAD:
					return costumeHeadList;

			}

			return costumeHeadList;
		}
	}




	public List<RuntimeData.EquipItemInfo> weaponItemList;
	public List<RuntimeData.EquipItemInfo> armorItemList;
	public List<RuntimeData.EquipItemInfo> ringItemList;
	public List<RuntimeData.EquipItemInfo> necklaceItemList;

	public List<RuntimeData.CostumeInfo> costumeHeadList;
	public List<RuntimeData.CostumeInfo> costumeBodyList;
	public List<RuntimeData.CostumeInfo> costumeWeaponList;

	public List<RuntimeData.PetInfo> petList;

	public List<RuntimeData.SkillInfo> skillList;
	public override void Load(UserDB _parent)
	{
		parent = _parent;

		List<RuntimeData.IItemInfo> list = new List<RuntimeData.IItemInfo>(weaponItemList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(armorItemList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(ringItemList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(necklaceItemList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(costumeHeadList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(costumeBodyList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(costumeWeaponList);
		SetItemListRawData(list);
		list = new List<RuntimeData.IItemInfo>(skillList);
		SetItemListRawData(list);

		list = new List<RuntimeData.IItemInfo>(petList);
		SetItemListRawData(list);


	}

	private void SetItemListRawData(List<RuntimeData.IItemInfo> infolist)
	{
		for (int i = 0; i < infolist.Count; i++)
		{
			infolist[i].SetRawData();
		}
	}





	public RuntimeData.CostumeInfo FindCostumeItem(long tid, CostumeType type = CostumeType._END)
	{
		switch (type)
		{
			case CostumeType.WEAPON:
				return costumeWeaponList.Find(x => x.tid == tid);

			case CostumeType.BODY:
				return costumeBodyList.Find(x => x.tid == tid);
			case CostumeType.HEAD:
				return costumeHeadList.Find(x => x.tid == tid);
			default:

				RuntimeData.CostumeInfo info = null;
				info = costumeWeaponList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				info = costumeBodyList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				info = costumeHeadList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				break;
		}
		return null;
	}
	public RuntimeData.EquipItemInfo FindEquipItem(long tid, EquipType type = EquipType._END)
	{
		switch (type)
		{
			case EquipType.WEAPON:
				return weaponItemList.Find(x => x.tid == tid);

			case EquipType.ARMOR:
				return armorItemList.Find(x => x.tid == tid);
			case EquipType.RING:
				return ringItemList.Find(x => x.tid == tid);
			case EquipType.NECKLACE:
				return necklaceItemList.Find(x => x.tid == tid);
			default:

				RuntimeData.EquipItemInfo info = null;
				info = weaponItemList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				info = armorItemList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				info = ringItemList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}
				info = necklaceItemList.Find(x => x.tid == tid);
				if (info != null)
				{
					return info;
				}

				break;
		}
		return null;
	}

	public RuntimeData.PetInfo FindPetItem(long tid)
	{
		var info = petList.Find(x => x.tid == tid);
		return info;
	}

	public void AddEquipItem(EquipType type)
	{

	}

	public void AddInventory(InstantItem item)
	{
		int index = itemList.FindIndex(x => x.tid == item.tid);
		if (index < 0)
		{
			itemList.Add(item);
		}
		else
		{
			itemList[index].count += item.count;
		}
	}

	public void RemoveInventory(InstantItem item)
	{
		int index = itemList.FindIndex(x => x.tid == item.tid);
		if (index < 0)
		{

		}
		else
		{
			itemList[index].count -= item.count;
			if (itemList[index].count <= 0)
			{
				itemList.RemoveAt(index);
			}
		}
	}


	public void GetAllOwnedValue()
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			//itemList[i]
		}
	}
}
