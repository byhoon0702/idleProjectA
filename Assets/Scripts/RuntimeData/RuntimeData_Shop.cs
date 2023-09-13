using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace RuntimeData
{
	[Serializable]
	public class ShopInfo : BaseInfo
	{


		[SerializeField] private int buyCount;
		public int BuyCount => buyCount;

		public CurrencyType CurrencyType { get; private set; }
		public IdleNumber Price { get; private set; }

		public int LimitCount { get; private set; }
		public Sprite Icon
		{
			get
			{
				if (itemObject == null)
				{
					return null;
				}
				return itemObject.ItemIcon;
			}
		}

		public DateTime startTime { get; private set; }
		public DateTime endTime { get; private set; }

		public ShopItemObject itemObject { get; private set; }
		public ShopData rawData { get; private set; }

		public TimeLimitType LimitType { get; private set; }
		public List<RewardInfo> rewardList { get; private set; } = new List<RewardInfo>();

		public override void Load<T>(T info)
		{
			if (info == null)
			{
				return;
			}
			base.Load(info);
			ShopInfo temp = info as ShopInfo;
			buyCount = temp.buyCount;
		}

		public bool IsShow()
		{
			var data = rawData.timeData;
			if (data.displayStartTime.IsNullOrEmpty() || data.displayEndTime.IsNullOrEmpty())
			{
				return true;
			}

			if (data.dailyStartSecond == 0 || data.dailyEndSecond == 0)
			{
				return true;
			}

			return false;
		}

		public bool IsUnlock()
		{
			//if (rawData.timeData.buyLimitCount > 0 && buyCount >= rawData.timeData.buyLimitCount)
			//{
			//	//구매 제한
			//	return false;
			//}
			return rawData.openData.openCondition.IsFulFillCondition(out string message);
		}

		public override void SetRawData<T>(T data)
		{
			rawData = data as ShopData;
			tid = rawData.tid;
			buyCount = 0;

			if (DateTime.TryParse(rawData.timeData.displayStartTime, out DateTime _startTime))
			{
				startTime = _startTime;
			}

			if (DateTime.TryParse(rawData.timeData.displayEndTime, out DateTime _endTime))
			{
				endTime = _endTime;
			}

			rewardList = new List<RewardInfo>();
			for (int i = 0; i < rawData.itemList.Count; i++)
			{
				Reward item = rawData.itemList[i];
				rewardList.Add(new RewardInfo(item.tid, item.category, Grade.D, (IdleNumber)item.count));
			}

			CurrencyType = rawData.cost.currency;
			Price = (IdleNumber)rawData.cost.cost;

			LimitCount = rawData.timeData.buyLimitCount;
			LimitType = rawData.timeData.limitType;
			itemObject = PlatformManager.UserDB.shopContainer.GetScriptableObject<ShopItemObject>(tid);

		}

		public virtual void DailyReset()
		{
			buyCount = 0;
		}

		public virtual void WeeklyReset()
		{
			if (TimeManager.Instance.UtcNow.ToLocalTime().DayOfWeek == DayOfWeek.Monday)
			{
				buyCount = 0;
			}
		}
		public virtual void MonthlyReset()
		{
			if (TimeManager.Instance.UtcNow.ToLocalTime().Day == 1)
			{
				buyCount = 0;
			}
		}

		public bool IsAvailable()
		{
			if (BuyCount >= rawData.timeData.buyLimitCount)
			{
				ToastUI.Instance.Enqueue($"구매 제한 초과 {BuyCount} / {rawData.timeData.buyLimitCount}");
				return false;
			}

			return true;
		}

		public void OnPurchaseSuccess(int purchaseNumber = 1)
		{
			switch (CurrencyType)
			{
				case CurrencyType.ADS:
				case CurrencyType.CASH:
				case CurrencyType.FREE:
					break;
				default:
					PlatformManager.UserDB.inventory.FindCurrency(CurrencyType).Pay(Price * purchaseNumber);
					break;
			}

			buyCount += purchaseNumber;

			List<RewardInfo> _rewardList = new List<RewardInfo>();

			List<RewardInfo> result = new List<RewardInfo>();
			for (int i = 0; i < rewardList.Count; i++)
			{
				var reward = rewardList[i].Clone();
				reward.Multiply((IdleNumber)purchaseNumber);
				_rewardList.Add(reward);
			}

			result.AddRange(RewardUtil.ReArrangReward(_rewardList));

			if (result.Count > 0)
			{
				PlatformManager.UserDB.AddRewards(result, true);
			}

			PlatformManager.RemoteSave.CloudSave();
		}

		public void OnPurchaseFail()
		{

		}

		public override void UpdateData()
		{

		}
	}
	[Serializable]
	public class EventShopInfo : ShopInfo
	{

	}
}
