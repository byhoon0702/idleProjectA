using System;

using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
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

	public GameObject equipObject
	{
		get
		{
			if (item == null || item.itemObject == null)
			{
				return null;
			}
			return item.itemObject.equipObject;
		}
	}
	[SerializeField] private long tid;
	public long itemTid => tid;

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
	public void AddEquipModifier(UserDB userDB)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.equipAbilities.Count; i++)
		{
			var ability = item.equipAbilities[i];
			userDB.AddModifiers(ability.isHyper, ability.type, new StatsModifier(ability.GetValue(item.level), ability.modeType, this));
		}
	}
	public void UpdateEquipModifier(UserDB userDB)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.equipAbilities.Count; i++)
		{
			var ability = item.equipAbilities[i];
			userDB.UpdateModifiers(ability.isHyper, ability.type, new StatsModifier(ability.GetValue(item.level), ability.modeType, this));
		}
	}
	public void RemoveEquipModifier(UserDB userDB)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.equipAbilities.Count; i++)
		{
			var ability = item.equipAbilities[i];
			userDB.RemoveModifiers(ability.isHyper, ability.type, this);
		}
	}


	public void Refresh()
	{
		RemoveEquipModifier(GameManager.UserDB);
		AddEquipModifier(GameManager.UserDB);
	}


	public bool Equip(long _tid)
	{
		if (_tid == 0)
		{
			return false;
		}

		var info = GameManager.UserDB.equipContainer.FindEquipItem(_tid, type);

		return Equip(info);
	}


	public bool Equip(RuntimeData.EquipItemInfo info)
	{
		if (info == null)
		{
			return false;
		}
		if (item != null && item.tid == info.tid)
		{
			return false;
		}

		item = info;
		tid = info.tid;
		UpdateEquipModifier(GameManager.UserDB);
		//AddEquipModifier(GameManager.UserDB);

		if (item != null && info.itemObject != null)
		{
			if (UnitManager.it.Player != null)
			{
				UnitManager.it.Player.EquipWeapon();
			}
		}


		return true;
	}
}

[CreateAssetMenu(fileName = "EquipContainer", menuName = "ScriptableObject/Container/EquipContainer", order = 1)]
public class EquipContainer : BaseContainer
{


	public List<RuntimeData.EquipItemInfo> weaponItemList;
	public List<RuntimeData.EquipItemInfo> armorItemList;
	public List<RuntimeData.EquipItemInfo> ringItemList;
	public List<RuntimeData.EquipItemInfo> necklaceItemList;

	public EquipSlot[] equipSlot;
	public EquipSlot GetSlot(EquipType _type)
	{
		return equipSlot[(int)_type];
	}

	public List<RuntimeData.EquipItemInfo> GetList(EquipType key)
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
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		EquipContainer temp = CreateInstance<EquipContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < weaponItemList.Count; i++)
		{
			if (i < temp.weaponItemList.Count)
			{
				weaponItemList[i].Load(temp.weaponItemList[i]);
			}
		}
		for (int i = 0; i < armorItemList.Count; i++)
		{
			if (i < temp.armorItemList.Count)
			{
				armorItemList[i].Load(temp.armorItemList[i]);
			}
		}
		for (int i = 0; i < ringItemList.Count; i++)
		{
			if (i < temp.ringItemList.Count)
			{
				ringItemList[i].Load(temp.ringItemList[i]);
			}
		}
		for (int i = 0; i < necklaceItemList.Count; i++)
		{
			if (i < temp.necklaceItemList.Count)
			{
				necklaceItemList[i].Load(temp.necklaceItemList[i]);
			}
		}

		if (weaponItemList[0].count == 0)
		{
			AddEquipItem(weaponItemList[0]);
		}

		for (int i = 0; i < equipSlot.Length; i++)
		{
			equipSlot[i].Equip(temp.equipSlot[i].itemTid);
		}

	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetItemListRawData(ref weaponItemList, DataManager.Get<EquipItemDataSheet>().GetByItemType(EquipType.WEAPON));
		SetItemListRawData(ref armorItemList, DataManager.Get<EquipItemDataSheet>().GetByItemType(EquipType.ARMOR));
		SetItemListRawData(ref ringItemList, DataManager.Get<EquipItemDataSheet>().GetByItemType(EquipType.RING));
		SetItemListRawData(ref necklaceItemList, DataManager.Get<EquipItemDataSheet>().GetByItemType(EquipType.NECKLACE));

		int last = (int)EquipType._END;

		equipSlot = new EquipSlot[last];
		for (int i = 0; i < last; i++)
		{
			equipSlot[i] = new EquipSlot();
			equipSlot[i].type = (EquipType)i;
		}

		if (weaponItemList[0].count == 0)
		{
			AddEquipItem(weaponItemList[0]);
		}

	}

	public void AddModifiers(UserDB userDB)
	{
		for (int i = 0; i < equipSlot.Length; i++)
		{
			if (equipSlot[i] != null)
			{
				equipSlot[i].AddEquipModifier(userDB);
			}
		}
	}


	public void RemoveModifiers(UserDB userDB)
	{
		for (int i = 0; i < equipSlot.Length; i++)
		{
			if (equipSlot[i] != null)
			{
				equipSlot[i].RemoveEquipModifier(userDB);
			}
		}
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var list = Resources.LoadAll<EquipItemObject>("RuntimeDatas/EquipItems/Weapons");
		AddDictionary(scriptableDictionary, list);
		list = Resources.LoadAll<EquipItemObject>("RuntimeDatas/EquipItems/Armors");
		AddDictionary(scriptableDictionary, list);
		list = Resources.LoadAll<EquipItemObject>("RuntimeDatas/EquipItems/Rings");
		AddDictionary(scriptableDictionary, list);
		list = Resources.LoadAll<EquipItemObject>("RuntimeDatas/EquipItems/Necklaces");
		AddDictionary(scriptableDictionary, list);
	}
	public RuntimeData.EquipItemInfo FindNextEquipItem(RuntimeData.EquipItemInfo item)
	{
		long tid = item.tid;
		int index = 0;
		switch (item.type)
		{
			case EquipType.WEAPON:
				index = weaponItemList.FindIndex(x => x.tid == tid) + 1;
				if (index < 0 || index >= weaponItemList.Count)
				{
					return item;
				}
				return weaponItemList[index];

			case EquipType.ARMOR:
				index = armorItemList.FindIndex(x => x.tid == tid) + 1;
				if (index < 0 || index >= armorItemList.Count)
				{
					return item;
				}
				return armorItemList[index];
			case EquipType.RING:
				index = ringItemList.FindIndex(x => x.tid == tid) + 1;
				if (index < 0 || index >= ringItemList.Count)
				{
					return item;
				}
				return ringItemList[index];
			case EquipType.NECKLACE:
				index = necklaceItemList.FindIndex(x => x.tid == tid) + 1;
				if (index < 0 || index >= necklaceItemList.Count)
				{
					return item;
				}
				return necklaceItemList[index];

		}
		return null;

	}
	public RuntimeData.EquipItemInfo FindEquipItem(RuntimeData.EquipItemInfo item)
	{
		return FindEquipItem(item.tid, item.type);
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


	public RuntimeData.EquipItemInfo RemoveEquipItem(RuntimeData.EquipItemInfo _info, int count)
	{
		var item = FindEquipItem(_info);
		item.count -= count;

		return item;
	}

	public RuntimeData.EquipItemInfo AddEquipItem(RuntimeData.EquipItemInfo _info, int count = 1)
	{
		long tid = _info.tid;

		switch (_info.type)
		{
			case EquipType.WEAPON:
				{
					var item = weaponItemList.Find(x => x.tid == tid);
					if (item == null)
					{
						_info.unlock = true;
						_info.level = 1;
						_info.count = count;
						weaponItemList.Add(_info);
						item = _info;
					}
					else
					{
						item.unlock = true;
						item.count += count;
					}
					return item;
				}
			case EquipType.ARMOR:
				{
					var item = armorItemList.Find(x => x.tid == tid);
					if (item == null)
					{
						_info.unlock = true;
						_info.level = 1;
						_info.count = count;
						armorItemList.Add(_info);
						item = _info;
					}
					else
					{
						item.unlock = true;
						item.count += count;
					}
					return item;
				}

			case EquipType.RING:
				{
					var item = ringItemList.Find(x => x.tid == tid);
					if (item == null)
					{
						_info.unlock = true;
						_info.level = 1;
						_info.count = count;
						ringItemList.Add(_info);
						item = _info;
					}
					else
					{
						item.unlock = true;
						item.count += count;
					}
					return item;
				}

			case EquipType.NECKLACE:
				{
					var item = necklaceItemList.Find(x => x.tid == tid);
					if (item == null)
					{
						_info.unlock = true;
						_info.level = 1;
						_info.count = count;
						item = _info;
						necklaceItemList.Add(_info);
					}
					else
					{
						item.unlock = true;
						item.count += count;
					}
					return item;
				}
		}

		return _info;
	}
	public const int needCount = 5;
	public void UpgradeEquipItem(ref RuntimeData.EquipItemInfo itemInfo, ref RuntimeData.EquipItemInfo newItemInfo, int upgradeCount)
	{
		itemInfo = RemoveEquipItem(itemInfo, upgradeCount * needCount);
		newItemInfo = AddEquipItem(newItemInfo, upgradeCount);


		if (GetSlot(itemInfo.type).itemTid == itemInfo.tid)
		{
			GetSlot(itemInfo.type).Refresh();
		}
		if (GetSlot(newItemInfo.type).itemTid == newItemInfo.tid)
		{
			GetSlot(newItemInfo.type).Refresh();
		}
	}

	public void LevelUpEquipItem(ref RuntimeData.EquipItemInfo itemInfo)
	{
		itemInfo.LevelUP();
		var slot = GetSlot(itemInfo.type);
		if (itemInfo.tid == slot.itemTid)
		{
			slot.Refresh();
		}
	}
}
