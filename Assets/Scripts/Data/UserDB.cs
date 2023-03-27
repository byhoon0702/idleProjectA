using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseContainer : ScriptableObject
{
	protected UserDB parent;
	public abstract void Load(UserDB _parent);
}
public class CharacterInfo
{ }



[CreateAssetMenu]
public class UserDB : ScriptableObject
{
	public ConfigMeta configMeta;
	public TrainingObject training;
	public EquipContainer equipContainer;
	public SkillContainer skillContainer;
	public CostumeContainer costumeContainer;
	public PetContainer petContainer;
	public InventoryObject inventory;

	public long characterTid;
	public string userName = "VIVID";
	public int uid = 12345678;
	public long playTicks;
	public Grade userGrade;

	public CharacterInfo characterInfo;

	[NonSerialized] public List<UserAbility> abilityinfos;

	public UserAbility Find(Ability type)
	{
		return abilityinfos.Find(x => x.Type == type);
	}

	public void Clear()
	{
		if (abilityinfos != null)
		{
			abilityinfos.Clear();
		}

	}

	public void Load(string json)
	{
		training.InitTrainingInfo(this);

		inventory.Load(this);
		equipContainer.Load(this);
		skillContainer.Load(this);
		costumeContainer.Load(this);
		petContainer.Load(this);
		InitStats();
	}

	public void InitStats()
	{
		abilityinfos = new List<UserAbility>();

		var data = DataManager.Get<UnitDataSheet>().Get(characterTid);

		for (int i = 0; i < data.statusDataList.Count; i++)
		{
			UserAbility info = new UserAbility(data.statusDataList[i].type, (IdleNumber)0/*training.Find(data.statusDataList[i].type).GetValue()*/, (IdleNumber)0, (IdleNumber)"999ZZ");
			abilityinfos.Add(info);
		}

		for (int i = 0; i < abilityinfos.Count; i++)
		{
			abilityinfos[i].RegisterTrainingEvent(training.Find(abilityinfos[i].Type).Get);
		}

		for (int i = 0; i < abilityinfos.Count; i++)
		{
			for (int ii = 0; ii < equipContainer.equipSlot.Length; ii++)
			{
				if (equipContainer.equipSlot[ii] != null)
				{
					abilityinfos[i].RegisterEquipEvent((equipContainer.equipSlot[ii].AddEquipValue));
					abilityinfos[i].RegisterBuffEvent((equipContainer.equipSlot[ii].AddOwnedValue));
				}
			}

			for (int ii = 0; ii < costumeContainer.equipSlot.Length; ii++)
			{
				if (costumeContainer.equipSlot[ii] != null)
				{
					abilityinfos[i].RegisterEquipEvent((costumeContainer.equipSlot[ii].AddEquipValue));
					abilityinfos[i].RegisterBuffEvent((costumeContainer.equipSlot[ii].AddOwnedValue));
				}
			}
		}

		UpdateUserStats();
	}

	public void UpdateUserStats()
	{
		for (int i = 0; i < abilityinfos.Count; i++)
		{
			abilityinfos[i].UpdateValue();
		}
	}
}

