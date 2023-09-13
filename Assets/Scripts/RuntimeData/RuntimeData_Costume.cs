using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeData
{
	[System.Serializable]
	public class CostumePointInfo
	{
		public long tid { get; private set; }
		public int needPoint;
		public AbilityInfo rewardAbility;

		[SerializeField] private bool isGet;
		public bool IsGet => isGet;
		public CostumePointInfo(CostumePointData data)
		{
			tid = data.tid;
			needPoint = data.needPoint;
			rewardAbility = new AbilityInfo(data.rewardStats);
			isGet = false;
		}

		public void GetReward(bool isTrue)
		{
			isGet = isTrue;
		}

		public void GetCostumeReward()
		{
			if (isGet)
			{
				return;
			}
			isGet = true;
			PlatformManager.UserDB.costumeContainer.AddCostumeAbility(rewardAbility);

			PlatformManager.UserDB.costumeContainer.UpdateData();
		}
	}


	[System.Serializable]
	public class CostumeInfo : ItemInfo
	{
		public override string ItemName => PlatformManager.Language[rawData.name];
		public CostumeType Type => rawData.costumeType;

		public Cost Cost => rawData.cost;
		public IdleNumber Price => (IdleNumber)Cost.cost;
		public Grade grade => rawData.itemGrade;
		public CostumeItemObject itemObject { get; private set; }
		public CostumeData rawData { get; private set; }

		public override Sprite IconImage => itemObject != null ? itemObject.ItemIcon : null;
		public HyperData hyperData { get; private set; }
		public HyperClassObject hyperClassObject { get; private set; }
		public CurrencyItemObject Currency { get; private set; }

		public bool CanLevelUp()
		{
			return false;
		}
		public override void SetRawData<T>(T data)
		{
			rawData = data as CostumeData;
			tid = rawData.tid;

			itemObject = PlatformManager.UserDB.costumeContainer.GetScriptableObject<CostumeItemObject>(tid);

			if (rawData.defaultGet)
			{
				unlock = true;
			}
		}

		public override void UpdateData()
		{
			var currencyInfo = PlatformManager.UserDB.inventory.FindCurrency(Cost.currency);
			if (currencyInfo != null)
			{
				Currency = currencyInfo.itemObject;
			}

			hyperData = DataManager.Get<HyperDataSheet>().Get(rawData.hyperTid);
			if (hyperData != null)
			{
				hyperClassObject = PlatformManager.UserDB.awakeningContainer.GetScriptableObject<HyperClassObject>(hyperData.tid);
			}

			if (rawData.defaultGet)
			{
				unlock = true;
			}
		}
	}

}
