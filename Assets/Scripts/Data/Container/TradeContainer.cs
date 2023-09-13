using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{

	public class TradeItemInfo
	{
		public ItemInfo itemInfo;
		public TradeItem rawData;
		public TradeItemInfo(TradeItem rawData, ItemInfo itemInfo)
		{
			this.rawData = rawData;
			this.itemInfo = itemInfo;
		}
	}

	[System.Serializable]
	public class TradeInfo : BaseInfo
	{
		[SerializeField] private int _tradeCount;
		public int TradeCount => _tradeCount;
		public TradeData rawData { get; private set; }

		public override void SetRawData<T>(T data)
		{
			rawData = data as TradeData;
			tid = rawData.tid;
		}

		public void Reset()
		{

		}

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			TradeInfo temp = info as TradeInfo;

			_tradeCount = temp._tradeCount;
		}

		public bool CheckLimit()
		{
			if (rawData.limitType == TimeLimitType.NONE)
			{
				return false;
			}

			return rawData.limitCount <= _tradeCount;
		}


		public TradeItemInfo GetInputItemInfo()
		{
			return Get(rawData.inputItem);
		}

		public List<TradeItemInfo> GetOutputItemInfo()
		{
			List<TradeItemInfo> list = new List<TradeItemInfo>();
			for (int i = 0; i < rawData.outputItem.Count; i++)
			{
				list.Add(Get(rawData.outputItem[i]));
			}
			return list;
		}

		public void Trade(int count)
		{
			if (CheckLimit())
			{
				return;
			}

			var input = Get(rawData.inputItem);


			if (rawData.inputItem.category == RewardCategory.Currency || rawData.inputItem.category == RewardCategory.Event_Currency)
			{
				IdleNumber cost = (IdleNumber)rawData.inputItem.count * count;
				CurrencyInfo currency = input.itemInfo as CurrencyInfo;
				if (currency == null)
				{
					//커런시 데이터가 아님
					return;
				}

				if (currency.Check(cost) == false)
				{
					return;
				}

				currency.Pay(cost);
			}
			else
			{
				if (input.itemInfo.Count < (IdleNumber)rawData.inputItem.count)
				{
					return;
				}

				input.itemInfo.CalculateCount(0, false);
			}

			//RewardInfo reward = new RewardInfo(rawData.outputItem.tid, rawData.outputItem.category, (IdleNumber)rawData.outputItem.count);

			//PlatformManager.UserDB.AddRewards(new List<RewardInfo>() { reward }, true);
		}

		public TradeItemInfo Get(TradeItem item)
		{

			switch (item.category)
			{
				case RewardCategory.Equip:
					{
						var result = PlatformManager.UserDB.equipContainer.FindEquipItem(item.tid);
						return new TradeItemInfo(item, result);

					}
				case RewardCategory.Pet:
					{
						var result = PlatformManager.UserDB.petContainer.FindPetItem(item.tid);
						return new TradeItemInfo(item, result);
					}
				case RewardCategory.RewardBox:
					{
						var result = PlatformManager.UserDB.inventory.GetRewardBox(item.tid);
						return new TradeItemInfo(item, result);
					}
				case RewardCategory.Skill:
					{
						var result = PlatformManager.UserDB.skillContainer.FindSKill(item.tid);
						return new TradeItemInfo(item, result);
					}
				case RewardCategory.Relic:
					{
						var result = PlatformManager.UserDB.relicContainer.Find(item.tid);
						return new TradeItemInfo(item, result);
					}
				case RewardCategory.Event_Currency:
				case RewardCategory.Currency:
					{
						var result = PlatformManager.UserDB.inventory.FindCurrency(item.tid);
						return new TradeItemInfo(item, result);
					}
				case RewardCategory.Costume:
					{
						var result = PlatformManager.UserDB.costumeContainer.FindCostumeItem(item.tid);
						return new TradeItemInfo(item, result);
					}
			}
			return null;
		}
	}
}



public class TradeContainer : BaseContainer
{

	public List<RuntimeData.TradeInfo> tradeList = new List<RuntimeData.TradeInfo>();
	public override void DailyResetData()
	{
		for (int i = 0; i < tradeList.Count; i++)
		{
			tradeList[i].Reset();
		}
	}

	public override void Dispose()
	{

	}

	public override void FromJson(string json)
	{
		TradeContainer temp = CreateInstance<TradeContainer>();
		JsonUtility.FromJsonOverwrite(json, temp);
		LoadListTidMatch(ref tradeList, temp.tradeList);
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		SetListRawData(ref tradeList, DataManager.Get<TradeDataSheet>().GetInfosClone());
	}

	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		string json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void UpdateData()
	{
		for (int i = 0; i < tradeList.Count; i++)
		{
			tradeList[i].UpdateData();
		}
	}
}
