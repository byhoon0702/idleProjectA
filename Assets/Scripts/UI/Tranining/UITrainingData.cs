using System;
using System.Collections;
using System.Collections.Generic;

public class UITrainingData
{
	private static VResult _result = new VResult();

	public ItemData itemData;
	public UserTrainingData trainingData;


	public long ItemTid => itemData.tid;
	public long UserTrainingTid => trainingData.tid;
	public long ConsumeItemTid => Inventory.it.GoldTid;


	public string TitleText
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return $"{itemData.name} LV. 1";
			}
			else
			{
				return $"{itemData.name} LV.{item.Level}";
			}
		}
	}

	public string CurrentStatText
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return $"0";
			}
			else
			{
				return $"{item.ToOwnAbility.GetValue(item.Level).ToString()}";
			}
		}
	}

	public string NextStatText
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return $"{itemData.ToOwnAbilityInfo.value.ToString()}";
			}
			else
			{
				return $"{item.ToOwnAbility.GetValue(item.Level + 1).ToString()}";
			}
		}
	}

	public IdleNumber LevelupCost
	{
		get
		{
			var item = GetItem();
			var sheet = DataManager.Get<UserTrainingDataSheet>();

			IdleNumber outData = sheet.LevelupConsume(UserTrainingTid, item.Level);
			return outData;
		}
	}

	public string LevelupCostText
	{
		get
		{
			var item = GetItem();
			var sheet = DataManager.Get<UserTrainingDataSheet>();
			if (item == null)
			{
				return $"{sheet.LevelupConsume(itemData.userTrainingTid, 1).ToString()}";
			}
			else
			{
				return $"{sheet.LevelupConsume(itemData.userTrainingTid, item.Level).ToString()}";
			}
		}
	}



	public VResult Setup(ItemData _itemData)
	{
		var trainingData = DataManager.Get<UserTrainingDataSheet>().Get(_itemData.userTrainingTid);

		if (trainingData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"UserTrainingDataSheet. itemTid: {_itemData.tid}, userTrainingTid: {_itemData.userTrainingTid}");
		}

		return Setup(_itemData, trainingData);
	}

	public VResult Setup(ItemData _itemData, UserTrainingData _trainingData)
	{
		itemData = _itemData;
		trainingData = _trainingData;

		return _result.SetOk();
	}

	public ItemTraining GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemTraining;
	}

	public bool Levelupable()
	{
		var item = GetItem();

		if (item == null || item.Count == 0)
		{
			return false;
		}

		if (item.Levelupable() == false)
		{
			return false;
		}

		var sheet = DataManager.Get<UserTrainingDataSheet>();
		var cost = sheet.LevelupConsume(itemData.userTrainingTid, item.Level);
		if (Inventory.it.CheckMoney(ConsumeItemTid, cost).Fail())
		{
			return false;
		}

		return true;
	}

	public void AbilityLevelUp(Action onLevelupSuccess = null)
	{
		bool levelupable = Levelupable();
		if (levelupable == false)
		{
			return;
		}

		var item = GetItem();
		if (Inventory.it.ConsumeItem(ConsumeItemTid, LevelupCost).Ok())
		{
			item.AddLevel(1);
			onLevelupSuccess?.Invoke();
		}
	}
}
