using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CostumeSlotDictionary : SerializableDictionary<CostumeType, CostumeSlot>
{ }


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
			return item.itemObject.ItemIcon;
		}
	}

	public void Clear()
	{
		item = null;
	}

	public override void AddModifiers(UserDB userDB)
	{

	}


	public override void RemoveModifiers(UserDB userDB)
	{

	}

	public bool Equip(long tid)
	{
		if (tid == 0)
		{
			return false;
		}
		var info = PlatformManager.UserDB.costumeContainer.FindCostumeItem(tid, type);
		return Equip(info);
	}
	public bool Equip(RuntimeData.CostumeInfo info)
	{
		if (info == null)
		{
			return false;
		}
		if (item != null && item.Tid == info.Tid)
		{
			return false;
		}

		item = info;
		tid = info.Tid;
		//RemoveEquipModifier(PlatformManager.UserDB);
		//AddEquipModifier(PlatformManager.UserDB);

		return true;
	}
}

[CreateAssetMenu(fileName = "Costume Container", menuName = "ScriptableObject/Container/Costume Container", order = 1)]
public class CostumeContainer : BaseContainer
{
	private int totalCostumePoints;
	public int TotalCostumePoints => totalCostumePoints;
	public int spendCostumePoints;



	public CostumeItemObject defaultBody;

	public CostumeItemObject defaultHyperBody;


	[SerializeField] private List<RuntimeData.CostumePointInfo> costumePointList = new List<RuntimeData.CostumePointInfo>();
	public List<RuntimeData.CostumePointInfo> CostumePointList => costumePointList;
	public Dictionary<StatsType, RuntimeData.AbilityInfo> costumeAbilities { get; private set; } = new Dictionary<StatsType, RuntimeData.AbilityInfo>();
	public List<RuntimeData.CostumeInfo> GetList(CostumeType key)
	{
		switch (key)
		{
			case CostumeType.CHARACTER:
				return costumeCharacterList;
			case CostumeType.HYPER:
				return hyperCostumeBodyList;
		}

		return costumeCharacterList;
	}



	public List<RuntimeData.CostumeInfo> costumeCharacterList;

	public List<RuntimeData.CostumeInfo> hyperCostumeBodyList;
	public CostumeSlot this[CostumeType _type]
	{
		get
		{
			return costumeSlots[_type];
		}

	}
	public CostumeSlotDictionary costumeSlots = new CostumeSlotDictionary();


	private const string CostumeAbilityKey = "CostumeAbility";
	public override void Dispose()
	{

	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		CostumeContainer temp = CreateInstance<CostumeContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref costumeCharacterList, temp.costumeCharacterList);
		LoadListTidMatch(ref hyperCostumeBodyList, temp.hyperCostumeBodyList);

		foreach (var slot in costumeSlots)
		{
			if (temp.costumeSlots.ContainsKey(slot.Key) == false)
			{
				continue;
			}
			slot.Value.Equip(temp.costumeSlots[slot.Key].itemTid);
		}

		for (int i = 0; i < costumePointList.Count; i++)
		{
			if (temp.costumePointList != null && temp.costumePointList.Count > i)
			{
				costumePointList[i].GetReward(temp.costumePointList[i].IsGet);
			}
		}
	}
	public override void DailyResetData()
	{

	}

	public override void UpdateData()
	{
		for (int i = 0; i < costumeCharacterList.Count; i++)
		{
			costumeCharacterList[i].UpdateData();
		}
		for (int i = 0; i < hyperCostumeBodyList.Count; i++)
		{
			hyperCostumeBodyList[i].UpdateData();
		}
		totalCostumePoints = 0;
		GetTotalPoints(costumeCharacterList, ref totalCostumePoints);

		PlatformManager.UserDB.RemoveAllModifiers(CostumeAbilityKey);
		costumeAbilities = new Dictionary<StatsType, RuntimeData.AbilityInfo>();
		for (int i = 0; i < costumePointList.Count; i++)
		{
			if (costumePointList[i].IsGet)
			{
				var ability = costumePointList[i].rewardAbility;
				AddCostumeAbility(ability);
			}
		}

		ApplyCostumeAbility();
	}


	public void AddCostumeAbility(RuntimeData.AbilityInfo info)
	{
		if (costumeAbilities.ContainsKey(info.type))
		{
			costumeAbilities[info.type].AddModifiers(new StatsModifier(info.Value, StatModeType.Add));
		}
		else
		{
			costumeAbilities.Add(info.type, info.Clone());
		}
	}

	private void GetTotalPoints(List<RuntimeData.CostumeInfo> list, ref int total)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].unlock)
			{
				total += list[i].rawData.point;
			}
		}
	}

	public override void Load(UserDB _parent)
	{

		parent = _parent;

		LoadScriptableObject();


		SetListRawData(ref costumeCharacterList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.CHARACTER));

		SetListRawData(ref hyperCostumeBodyList, DataManager.Get<CostumeDataSheet>().GetByItemType(CostumeType.HYPER));

		var list = DataManager.Get<CostumePointDataSheet>().GetInfosClone();

		costumePointList = new List<RuntimeData.CostumePointInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			costumePointList.Add(new RuntimeData.CostumePointInfo(list[i]));
		}

		costumeSlots = new CostumeSlotDictionary();

		AddCostumeSlot(CostumeType.CHARACTER, defaultBody != null ? defaultBody.Tid : costumeCharacterList[0].Tid);
		AddCostumeSlot(CostumeType.HYPER, defaultHyperBody != null ? defaultHyperBody.Tid : hyperCostumeBodyList[0].Tid);

	}
	void AddCostumeSlot(CostumeType type, long tid)
	{
		var slot = new CostumeSlot();
		slot.SetInfo(type, null);
		slot.Equip(tid);
		costumeSlots.Add(type, slot);
	}


	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Characters");
		AddDictionary(scriptableDictionary, costumelist);
		costumelist = Resources.LoadAll<CostumeItemObject>("RuntimeDatas/Costumes/Hypers");
		AddDictionary(scriptableDictionary, costumelist);
	}
	public RuntimeData.CostumeInfo FindCostumeItem(long tid, CostumeType type = CostumeType._END)
	{
		switch (type)
		{

			case CostumeType.CHARACTER:
				return costumeCharacterList.Find(x => x.Tid == tid);
			case CostumeType.HYPER:
				return hyperCostumeBodyList.Find(x => x.Tid == tid);
			default:

				RuntimeData.CostumeInfo info = null;
				info = costumeCharacterList.Find(x => x.Tid == tid);
				if (info != null)
				{
					return info;
				}
				info = hyperCostumeBodyList.Find(x => x.Tid == tid);
				if (info != null)
				{
					return info;
				}
				break;
		}
		return null;
	}

	public void Buy(long tid)
	{
		var info = FindCostumeItem(tid);
		if (info == null)
		{
			VLog.ItemLogError($"Costume {tid} Could Not Found");
			return;
		}
		info.unlock = true;

		UpdateData();
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

	public void ApplyCostumeAbility()
	{

		foreach (var ability in costumeAbilities)
		{
			PlatformManager.UserDB.UpdateModifiers(ability.Key, new StatsModifier(ability.Value.Value, StatModeType.Buff, CostumeAbilityKey));
		}
	}
}
