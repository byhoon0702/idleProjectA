using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			return item.itemObject.ItemIcon;
		}
	}


	public void UnEquip()
	{
		if (item == null)
		{
			return;
		}
		RemoveModifiers(PlatformManager.UserDB);
		PlatformManager.UserDB.skillContainer.UnEquipPetSkill(index, item.SkillTid);
		item = null;
		tid = 0;
		UIController.it.SkillGlobal.OnUpdate();
	}

	public bool Equip(long tid)
	{
		if (tid == 0)
		{
			return false;
		}

		var info = PlatformManager.UserDB.petContainer.FindPetItem(tid);

		return Equip(info);
	}


	public bool Equip(RuntimeData.PetInfo info)
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

		Refresh();
		PlatformManager.UserDB.UpdateUserStats();

		//PlatformManager.UserDB.skillContainer.EquipPetSkill(UnitManager.it.Player, index, info.rawData.skillTid);
		PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.EQUIP_PET, tid, (IdleNumber)1);
		return true;
	}
	public void Refresh()
	{
		RemoveModifiers(PlatformManager.UserDB);
		AddModifiers(PlatformManager.UserDB);
	}


	public override void AddModifiers(UserDB userDB)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.equipAbilities.Count; i++)
		{
			var ability = item.equipAbilities[i];
			userDB.AddModifiers(ability.type, new StatsModifier(ability.GetValue(item.Level), ability.modeType, this));
		}
	}

	public override void RemoveModifiers(UserDB userDB)
	{
		if (item == null)
		{
			return;
		}
		for (int i = 0; i < item.equipAbilities.Count; i++)
		{
			var ability = item.equipAbilities[i];
			userDB.RemoveModifiers(ability.type, this);
		}
	}
}

[CreateAssetMenu(fileName = "PetContainer", menuName = "ScriptableObject/Container/PetContainer", order = 1)]
public class PetContainer : BaseContainer
{

	public List<RuntimeData.PetInfo> petList;
	[SerializeField] private PetSlot[] petSlots;
	public PetSlot[] PetSlots => petSlots;
	[SerializeField] private short slotCount = 3;

	public const int needCount = 5;
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
		PetContainer temp = CreateInstance<PetContainer>();

		JsonUtility.FromJsonOverwrite(json, temp);

		LoadListTidMatch(ref petList, temp.petList);

		for (int i = 0; i < petSlots.Length; i++)
		{
			if (i < temp.petSlots.Length)
			{
				petSlots[i].Equip(temp.petSlots[i].itemTid);
			}
		}
	}

	public PetSlot GetSlot(long tid)
	{
		for (int i = 0; i < petSlots.Length; i++)
		{
			if (petSlots[i].itemTid == tid)
			{
				return petSlots[i];
			}
		}
		return null;
	}
	public override void UpdateData()
	{
		RemoveModifiers();
		AddModifiers();
	}
	public override void DailyResetData()
	{

	}
	public void RemoveModifiers()
	{
		for (int i = 0; i < petSlots.Length; i++)
		{
			var slot = petSlots[i];
			if (slot.item == null)
			{
				continue;
			}
			slot.RemoveModifiers(PlatformManager.UserDB);
		}


		for (int i = 0; i < petList.Count; i++)
		{
			var pet = petList[i];
			if (pet.unlock == false)
			{
				continue;
			}

			pet.RemoveModifiers(PlatformManager.UserDB);
		}

	}


	public void AddModifiers()
	{
		for (int i = 0; i < petSlots.Length; i++)
		{
			var slot = petSlots[i];
			if (slot.item == null)
			{
				continue;
			}
			slot.AddModifiers(PlatformManager.UserDB);
		}
		for (int i = 0; i < petList.Count; i++)
		{
			var pet = petList[i];
			if (pet.unlock == false)
			{
				continue;
			}

			pet.AddModifiers(PlatformManager.UserDB);
		}
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetListRawData(ref petList, DataManager.Get<PetDataSheet>().GetInfosClone());
		petSlots = new PetSlot[slotCount];

		for (short i = 0; i < slotCount; i++)
		{
			petSlots[i] = new PetSlot(i);
		}
	}
	public RuntimeData.PetInfo FindPetItem(long tid)
	{
		var info = petList.Find(x => x.Tid == tid);
		return info;
	}

	public void AddPetItem(long tid, int count)
	{
		var info = FindPetItem(tid);
		if (info == null)
		{
			return;
		}
		info.unlock = true;
		if (info.Level == 0)
		{
			info.SetLevel(1);
		}
		info.CalculateCount(count);

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
			if (petSlots[i].itemTid == tid)
			{
				petSlots[i].UnEquip();

				break;
			}
		}
	}

	public bool EvolutionPet(RuntimeData.PetInfo info)
	{
		if (info.Evolution() == false)
		{
			return false;
		}

		var slot = GetSlot(info.Tid);
		if (slot != null)
		{
			slot.Refresh();
		}
		return true;
	}

	public void LevelUpPet(ref RuntimeData.PetInfo info)
	{
		info.LevelUP();

		var slot = GetSlot(info.Tid);
		if (slot != null)
		{
			slot.Refresh();
		}

	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var petlist = Resources.LoadAll<PetItemObject>("RuntimeDatas/Pets");
		AddDictionary(scriptableDictionary, petlist);
	}
}
