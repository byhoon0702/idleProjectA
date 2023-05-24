using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Newtonsoft.Json;
using RuntimeData;

[System.Serializable]
public class CostumeSlot : DataSlot
{
	[SerializeField] private long tid;
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

	public void Clear()
	{
		item = null;
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

	public bool Equip(long tid)
	{
		if (tid == 0)
		{
			return false;
		}
		var info = GameManager.UserDB.costumeContainer.FindCostumeItem(tid, type);
		return Equip(info);
	}
	public bool Equip(RuntimeData.CostumeInfo info)
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
		RemoveEquipModifier(GameManager.UserDB);
		AddEquipModifier(GameManager.UserDB);

		return true;
	}
}

[CreateAssetMenu(fileName = "Costume Container", menuName = "ScriptableObject/Container/Costume Container", order = 1)]
public class CostumeContainer : BaseContainer
{
	public CostumeItemObject defaultWeapon;
	public CostumeItemObject defaultBody;
	public CostumeItemObject defaultHead;

	public CostumeItemObject[] hyperFaces;
	public CostumeItemObject defaultHyperBody;

	public List<RuntimeData.CostumeInfo> GetList(CostumeType key)
	{
		switch (key)
		{
			case CostumeType.WEAPON:
				return costumeWeaponList;
			case CostumeType.BODY:
				return costumeBodyList;
			case CostumeType.HEAD:
				return costumeHeadList;
			case CostumeType.WHOLEBODY:
				return hyperCostumeBodyList;

		}

		return costumeHeadList;
	}



	public List<RuntimeData.CostumeInfo> costumeHeadList;

	public List<RuntimeData.CostumeInfo> costumeBodyList;

	public List<RuntimeData.CostumeInfo> costumeWeaponList;
	public CostumeSlot this[CostumeType _type]
	{
		get
		{
			return equipSlot[(int)_type];
		}

	}

	public List<RuntimeData.CostumeInfo> hyperCostumeBodyList;

	public CostumeSlot hyperCostumeSlot;

	public CostumeSlot[] equipSlot;
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		CostumeContainer temp = CreateInstance<CostumeContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		for (int i = 0; i < costumeHeadList.Count; i++)
		{
			if (i < temp.costumeHeadList.Count)
			{
				costumeHeadList[i].Load(temp.costumeHeadList[i]);
			}
		}
		for (int i = 0; i < costumeBodyList.Count; i++)
		{
			if (i < temp.costumeBodyList.Count)
			{
				costumeBodyList[i].Load(temp.costumeBodyList[i]);
			}
		}
		for (int i = 0; i < costumeWeaponList.Count; i++)
		{
			if (i < temp.costumeWeaponList.Count)
			{
				costumeWeaponList[i].Load(temp.costumeWeaponList[i]);
			}
		}
		for (int i = 0; i < costumeWeaponList.Count; i++)
		{
			if (i < temp.hyperCostumeBodyList.Count)
			{
				hyperCostumeBodyList[i].Load(temp.hyperCostumeBodyList[i]);
			}
		}


		for (int i = 0; i < equipSlot.Length; i++)
		{
			long tid = temp.equipSlot[i].itemTid;
			if (tid == 0)
			{
				continue;
			}

			equipSlot[i].Equip(temp.equipSlot[i].itemTid);
		}
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;

		LoadScriptableObject();

		SetItemListRawData(ref costumeHeadList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.HEAD));
		SetItemListRawData(ref costumeBodyList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.BODY));
		SetItemListRawData(ref costumeWeaponList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.WEAPON));
		SetItemListRawData(ref hyperCostumeBodyList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.WHOLEBODY));

		int last = (int)CostumeType._EQUIP_END;
		equipSlot = new CostumeSlot[last];

		for (int i = 0; i < last; i++)
		{
			equipSlot[i] = new CostumeSlot();
			CostumeType type = (CostumeType)i;
			long tid = 0;
			switch (type)
			{
				case CostumeType.HEAD:
					tid = defaultHead != null ? defaultHead.Tid : 0;
					break;
				case CostumeType.BODY:
					tid = defaultBody != null ? defaultBody.Tid : 0;
					break;
				case CostumeType.WEAPON:
					tid = defaultWeapon != null ? defaultWeapon.Tid : 0;
					break;
			}
			equipSlot[i].SetInfo(type, null);
			equipSlot[i].Equip(tid);

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
			case CostumeType.WHOLEBODY:
				return hyperCostumeBodyList.Find(x => x.tid == tid);
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

	public void Equip(long tid)
	{
		var info = FindCostumeItem(tid);

		if (info == null)
		{
			return;
		}
		this[info.itemObject.Type].Equip(info);
	}
	public void Equip(long tid, CostumeType type)
	{
		var info = FindCostumeItem(tid, type);

		if (info == null)
		{
			return;
		}
		this[type].Equip(info);
	}

	public CostumeItemObject GetHyperFace(int index)
	{
		if (index < 0)
		{
			return hyperFaces[0];
		}

		return hyperFaces[index - 1];
	}

	public CostumeItemObject GetHyperCostume()
	{
		if (hyperCostumeSlot.item == null || hyperCostumeSlot.item.itemObject == null || hyperCostumeSlot.item.itemObject.CostumeObject == null)
		{
			return defaultHyperBody;
		}
		return hyperCostumeSlot.item.itemObject;
	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Heads");
		AddDictionary(scriptableDictionary, costumelist);
		costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Bodys");
		AddDictionary(scriptableDictionary, costumelist);
		costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Weapons");
		AddDictionary(scriptableDictionary, costumelist);
		costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Wholebodys");
		AddDictionary(scriptableDictionary, costumelist);
	}
}
