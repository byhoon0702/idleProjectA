using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AddItemInfo
{
	public long tid;
	public IdleNumber value;
	public RewardCategory category;
	public Grade grade;
	public Sprite iconImage;
	public int starGrade;
	public float chance;



	public AddItemInfo(RuntimeData.RewardInfo info)
	{
		tid = info.Tid;
		value = info.fixedCount;
		category = info.Category;
		grade = info.grade;
		iconImage = info.iconImage;
		starGrade = info.Star;
		chance = info.Chance;


	}

	public AddItemInfo(long _tid, IdleNumber _value, RewardCategory _category, float _chance = 0)
	{
		tid = _tid;
		value = _value;
		category = _category;
		grade = Grade.D;
		starGrade = 0;
		chance = _chance;

		switch (category)
		{
			case RewardCategory.Equip:
				{
					var item = PlatformManager.UserDB.equipContainer.FindEquipItem(tid);
					iconImage = item?.Icon;
					grade = item.grade;
					starGrade = item.star;

				}
				break;
			case RewardCategory.Pet:
				{
					var item = PlatformManager.UserDB.petContainer.FindPetItem(tid);
					iconImage = item?.Icon;
					grade = item.grade;

				}
				break;
			case RewardCategory.Skill:
				{
					var item = PlatformManager.UserDB.skillContainer.FindSKill(tid);
					iconImage = item?.Icon;
					grade = item.grade;

				}
				break;
			case RewardCategory.Costume:
				{
					var item = PlatformManager.UserDB.costumeContainer.FindCostumeItem(tid);
					iconImage = item?.itemObject.ItemIcon;
					grade = item.grade;

				}
				break;
			case RewardCategory.Currency:
				{
					var item = PlatformManager.UserDB.inventory.FindCurrency(tid);
					iconImage = item?.IconImage;

				}
				break;
			case RewardCategory.RewardBox:
				{
					var item = PlatformManager.UserDB.inventory.GetRewardBox(tid);
					iconImage = item?.IconImage;


				}
				break;
			case RewardCategory.Persistent:
				{
					var item = PlatformManager.UserDB.inventory.GetPersistent(tid);
					iconImage = item?.icon;
				}
				break;
			case RewardCategory.EXP:
				{
					iconImage = GameUIManager.it.spriteExp;

				}
				break;
			default:
				iconImage = null;
				break;
		}
	}

	public void AddValue(string number)
	{
		AddValue((IdleNumber)number);
	}

	public void AddValue(IdleNumber number)
	{
		value += number;
	}
}


[CreateAssetMenu(fileName = "Inventory Container", menuName = "ScriptableObject/Container/Inventory", order = 1)]
[System.Serializable]
public class InventoryContainer : BaseContainer
{
	public List<RuntimeData.CurrencyInfo> currencyList;
	public List<RuntimeData.RewardBoxInfo> rewardBoxList;
	public List<RuntimeData.PersistentItemInfo> persistentItemList;
	public List<AdsRewardChestItemObject> adsRewardChestList { get; private set; }

	public List<string> saves = new List<string>();
	public AdsRewardChestItemObject SelectRewardChest { get; private set; }

	public const long AdFreeTid = 9990000001;
	public override void Dispose()
	{

	}
	public override void Load(UserDB _parent)
	{
		parent = _parent;
		LoadScriptableObject();
		SetListRawData(ref currencyList, DataManager.Get<CurrencyDataSheet>().GetInfosClone());
		SetListRawData(ref rewardBoxList, DataManager.Get<RewardBoxDataSheet>().GetInfosClone());
		SetListRawData(ref persistentItemList, DataManager.Get<PersistentItemDataSheet>().GetInfosClone());
		for (int i = 0; i < adsRewardChestList.Count; i++)
		{
			adsRewardChestList[i].LoadData();
		}
	}

	public override string Save()
	{

		saves = new List<string>();
		for (int i = 0; i < adsRewardChestList.Count; i++)
		{
			saves.Add(adsRewardChestList[i].ToJson());
		}

		var json = JsonUtility.ToJson(this, true);
		return json;
	}
	public override void FromJson(string json)
	{
		InventoryContainer temp = CreateInstance<InventoryContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref currencyList, temp.currencyList);
		LoadListTidMatch(ref rewardBoxList, temp.rewardBoxList);

		LoadListTidMatch(ref persistentItemList, temp.persistentItemList);
		saves = temp.saves;
		for (int i = 0; i < adsRewardChestList.Count; i++)
		{
			if (i < saves.Count)
			{
				adsRewardChestList[i].FromJson(saves[i]);
			}
		}
	}

	public override void DailyResetData()
	{
		var dungeon_key = FindCurrency(CurrencyType.TOMB_DUNGEON_KEY);
		dungeon_key.ResetCount(dungeon_key.refill);
		dungeon_key = FindCurrency(CurrencyType.HATCHERY_DUNGEON_KEY);
		dungeon_key.ResetCount(dungeon_key.refill);
		dungeon_key = FindCurrency(CurrencyType.IMMORTAL_DUNGEON_KEY);
		dungeon_key.ResetCount(dungeon_key.refill);
		dungeon_key = FindCurrency(CurrencyType.SESAME_DUNGEON_KEY);
		dungeon_key.ResetCount(dungeon_key.refill);
		dungeon_key = FindCurrency(CurrencyType.VITALITY_DUNGEON_TICKET);
		dungeon_key.ResetCount(dungeon_key.refill);
		dungeon_key = FindCurrency(CurrencyType.GUARDIAN_DUNGEON_TICKET);
		dungeon_key.ResetCount(dungeon_key.refill);
	}

	public RuntimeData.RewardBoxInfo GetRewardBox(long tid)
	{
		return rewardBoxList.Find(x => x.Tid == tid);
	}
	public RuntimeData.PersistentItemInfo GetPersistent(long tid)
	{
		return persistentItemList.Find(x => x.Tid == tid);
	}

	public RuntimeData.CurrencyInfo FindCurrency(long tid)
	{
		for (int i = 0; i < currencyList.Count; i++)
		{
			if (currencyList[i].Tid == tid)
			{
				return currencyList[i];
			}
		}
		return null;
	}

	public void AddRandomBox(long tid, int count)
	{
		DataManager.Get<RewardBoxDataSheet>().Get(tid);

		var data = rewardBoxList.Find(x => x.Tid == tid);

		if (data != null)
		{
			data.AddRewardBox(count);
		}
		else
		{
			var info = new RuntimeData.RewardBoxInfo(tid);
			info.SetReward();
			info.SetCount(count);
			rewardBoxList.Add(info);
		}
	}

	public void OpenAllRewardBox()
	{

	}

	public RuntimeData.CurrencyInfo FindCurrency(CurrencyType type)
	{
		for (int i = 0; i < currencyList.Count; i++)
		{
			if (currencyList[i].type == type)
			{
				return currencyList[i];
			}
		}
		return null;
	}


	public override void UpdateData()
	{

	}

	public override void LoadScriptableObject()
	{
		scriptableDictionary = new ScriptableDictionary();
		var costumelist = Resources.LoadAll<CurrencyItemObject>("RuntimeDatas/CurrencyItems");
		AddDictionary(scriptableDictionary, costumelist);
		var adsChest = Resources.LoadAll<AdsRewardChestItemObject>("RuntimeDatas/AdsRewardChests");
		adsRewardChestList = new List<AdsRewardChestItemObject>(adsChest);
		var rewardbox = Resources.LoadAll<RewardBoxItemObject>("RuntimeDatas/RewardBoxs");
		AddDictionary(scriptableDictionary, rewardbox);
		//costumelist = Resources.LoadAll<CurrencyItemObject>("RuntimeDatas/Costumes/Bodys");
		//AddDictionary(scriptableDictionary, costumelist);
	}

	public void SelectAdsRewardChest(bool change = true)
	{
		if (change || SelectRewardChest == null)
		{
			int index = 0;
			List<int> randomIndex = new List<int>();
			for (int i = 0; i < adsRewardChestList.Count; i++)
			{
				if (adsRewardChestList[i].IsWatchAll())
				{
					continue;
				}
				randomIndex.Add(i);
			}
			if (randomIndex.Count == 0)
			{
				SelectRewardChest = null;
				return;
			}

			index = randomIndex[Random.Range(0, randomIndex.Count)];
			SelectRewardChest = adsRewardChestList[index];
		}
		SelectRewardChest.Select();
	}

}
