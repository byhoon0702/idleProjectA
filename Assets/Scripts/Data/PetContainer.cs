using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.Linq;

public enum PetType
{ }


[System.Serializable]
public class PetSlot : DataSlot
{
	public short index { get; private set; }
	public RuntimeData.PetInfo item;
	public PetSlot(short _index)
	{
		index = _index;
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
	public bool Equip(long tid)
	{
		if (tid == 0)
		{
			return false;
		}

		var info = GameManager.UserDB.petContainer.FindPetItem(tid);

		return Equip(info);
	}


	public bool Equip(RuntimeData.PetInfo info)
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
		GameManager.UserDB.UpdateUserStats();
		if (item == null)
		{

		}
		else
		{

		}

		return true;
	}



}

[CreateAssetMenu(fileName = "PetContainer", menuName = "ScriptableObject/Container/PetContainer", order = 1)]
public class PetContainer : BaseContainer
{

	public List<RuntimeData.PetInfo> petList;
	[SerializeField] private PetSlot[] petSlots;
	public PetSlot[] PetSlots => petSlots;
	[SerializeField] private short slotCount;
	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		PetContainer temp = CreateInstance<PetContainer>();

		JsonUtility.FromJsonOverwrite(json, temp);
		for (int i = 0; i < petList.Count; i++)
		{
			if (i < temp.petList.Count)
			{
				petList[i].Load(temp.petList[i]);
			}
		}

		for (int i = 0; i < petSlots.Length; i++)
		{
			if (i < temp.petSlots.Length)
			{
				petSlots[i].Equip(temp.petSlots[i].itemTid);
			}
		}
	}
	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetItemListRawData(ref petList, DataManager.Get<PetDataSheet>().GetInfosClone());
		petSlots = new PetSlot[slotCount];

		for (short i = 0; i < slotCount; i++)
		{
			petSlots[i] = new PetSlot(i);
		}
	}
	public RuntimeData.PetInfo FindPetItem(long tid)
	{
		var info = petList.Find(x => x.tid == tid);
		return info;
	}

	public short GetIndex(long tid)
	{
		for (short i = 0; i < petSlots.Length; i++)
		{
			if (petSlots[i].item == null)
			{
				continue;

			}
			if (petSlots[i].itemTid == tid)
			{
				return i;
			}
		}
		return -1;
	}

	public bool Equip(long tid)
	{
		for (short i = 0; i < petSlots.Length; i++)
		{
			if (petSlots[i].item == null)
			{
				petSlots[i].Equip(tid);
				return true;
			}
		}
		return false;
	}

	public void Unequip(long tid)
	{
		for (short i = 0; i < petSlots.Length; i++)
		{
			if (petSlots[i].item == null)
			{
				continue;
			}
			if (petSlots[i].item.tid == tid)
			{
				petSlots[i].item = null;
				break;

			}
		}
	}
	public void EvolutionPet(RuntimeData.PetInfo info)
	{
		info.Evolution();


	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var petlist = Resources.LoadAll<PetItemObject>("RuntimeDatas/Pets");
		AddDictionary(scriptableDictionary, petlist);
	}
}
