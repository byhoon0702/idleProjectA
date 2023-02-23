using System;
using System.Collections;
using System.Collections.Generic;

public class UIPetData
{
	private VResult _result = new VResult();

	public ItemData itemData;
	public PetData petData;


	public long ItemTid => itemData.tid;
	public long PetTid => itemData.petTid;
	public long LevelupConsumeItemTid => ItemTid;
	public IdleNumber ConsumeItemCount
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return new IdleNumber(GetItem().NextExp);
			}
			else
			{
				return new IdleNumber(0);
			}
		}
	}
	public string ItemName => itemData.name;
	public string ItemDesc => petData.description;
	public string CategoryText
	{
		get
		{
			string outText = "";
			foreach (PetCategory category in Enum.GetValues(typeof(PetCategory)))
			{
				if ((((int)category) & ((int)petData.category)) == (int)petData.category)
				{
					outText += $"{category.ToString()}, ";
				}
			}

			return outText;
		}
	}
	public Grade ItemGrade => itemData.itemGrade;
	public string ItemGradeText => ItemGrade.ToString();
	public string Icon => itemData.Icon;
	public string LevelText
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return $"Lv. {item.Level}";
			}
			else
			{
				return "Lv. 0";
			}
		}
	}

	public string ExpText
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return $"{item.Exp}/{LevelupNeedCount}";
			}
			else
			{
				return $"0 / {LevelupNeedCount}";
			}
		}
	}

	public float ExpRatio
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.ExpRatio;
			}
			else
			{
				return 0;
			}
		}
	}

	public int LevelupNeedCount
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.NextExp;
			}
			else
			{
				var sheet = DataManager.Get<EquipLevelDataSheet>();
				return sheet.Level1NeedCount();
			}
		}
	}

	public string ToOwnText
	{
		get
		{
			var item = GetItem();
			var abilityData = DataManager.Get<StatusDataSheet>().GetData(itemData.ToOwnAbilityInfo.type);

			if (abilityData == null)
			{
				return $"보유효과: 없음";
			}
			else if (item != null)
			{
				return $"보유효과: {abilityData.description} {item.ToOwnAbility.GetValue(item.Level).ToString()}";
			}
			else
			{
				return $"보유효과: {abilityData.description} {itemData.ToOwnAbilityInfo.ToString()}";
			}
		}
	}

	public IdleNumber ToOwnAbilityValue
	{
		get
		{
			var item = GetItem();
			if (item == null)
			{
				return new IdleNumber(0);
			}
			else
			{
				return item.ToOwnAbility.GetValue(item.Level);
			}
		}
	}


	public VResult Setup(long _itemTid)
	{
		ItemData data = DataManager.Get<ItemDataSheet>().Get(_itemTid);
		if (data == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return Setup(data);
	}

	public VResult Setup(ItemData _itemData)
	{
		var sheet = DataManager.Get<PetDataSheet>();
		PetData data = sheet.GetData(_itemData.petTid);
		if (data == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"PetDataSheet. itemTid: {_itemData.tid}, petTid: {_itemData.petTid}");
		}

		return Setup(_itemData, data);
	}

	public VResult Setup(ItemData _itemData, PetData _petData)
	{
		itemData = _itemData;
		petData = _petData;

		return _result.SetOk();
	}

	public ItemPet GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemPet;
	}

	public bool Upgradable()
	{
		var item = GetItem();
		bool hasItem = item != null && item.Count > 0;

		if (hasItem == false)
		{
			return false;
		}

		if (item.Levelupable() == false)
		{
			return false;
		}

		return true;
	}
	public void LevelupItem(Action onUpgradeSuccess = null)
	{
		if (Upgradable())
		{
			if (Inventory.it.ConsumeItem(LevelupConsumeItemTid, ConsumeItemCount).Ok())
			{
				var item = GetItem();
				item.AddLevel(1);

				onUpgradeSuccess?.Invoke();
			}
		}
	}
}
