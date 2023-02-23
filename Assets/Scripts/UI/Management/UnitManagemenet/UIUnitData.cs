using System;
using System.Collections;
using System.Collections.Generic;

public class UIUnitData
{
	private static VResult _result = new VResult();

	public ItemData itemData;
	public UnitData unitData;
	public SkillData unitSkillData;
	public SkillData finalSkillData;

	public long ItemTid => itemData.tid;
	public string Icon => itemData.Icon;
	public string UnitName => itemData.name;
	public Grade UnitGrade => itemData.itemGrade;
	public string UnitGradeText => itemData.itemGrade.ToString();
	public int UnitLevel
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.Level;
			}
			else
			{
				return 0;
			}
		}
	}
	public string UnitLevelText => $"Lv. {UnitLevel.ToString()}";
	public float ExpRatio
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return item.expRatio;
			}
			else
			{
				return 0;
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
				if (item.IsMaxLv)
				{
					return $"EXP Max ({(item.expRatio * 100).ToString("F2")}%)";
				}
				else
				{
					return $"EXP {item.Exp.ToString("N0")} / {item.nextExp.ToString("N0")} ({(item.expRatio * 100).ToString("F2")}%)";
				}
			}
			else
			{
				return $"0 / 0";
			}
		}
	}
	public string LevelupConsumeHashtag => "unitlevelup";
	public IdleNumber LevelupConsumeCount => new IdleNumber(1);
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
	public string CurrentStatText
	{
		get
		{
			var item = GetItem();

			if (item != null)
			{
				return $"{item.ToOwnAbility.type} {item.ToOwnAbility.GetValue(item.Level).ToString()}";
			}
			else
			{
				return $"{itemData.ToOwnAbilityInfo.type} 0";
			}
		}
	}

	public string NextStatText
	{
		get
		{
			var item = GetItem();

			if (item != null)
			{
				if (item.IsMaxLv)
				{
					return "MAX";
				}
				else
				{
					return $"{item.ToOwnAbility.type} {item.ToOwnAbility.GetValue(item.Level + 1).ToString()}";
				}
			}
			else
			{
				return $"{itemData.ToOwnAbilityInfo.type} {itemData.ToOwnAbilityInfo.value}";
			}
		}
	}


	public VResult Setup(long _itemTid)
	{
		itemData = DataManager.Get<ItemDataSheet>().Get(_itemTid);
		if (itemData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return Setup(itemData);
	}

	public VResult Setup(ItemData _itemData)
	{
		itemData = _itemData;
		unitData = DataManager.Get<UnitDataSheet>().GetData(_itemData.unitTid);
		if (unitData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"UnitDataSheet. itemTid: {_itemData.tid}, unitTid: {_itemData.unitTid}");
		}

		unitSkillData = DataManager.Get<SkillDataSheet>().Get(unitData.skillTid);
		if (unitSkillData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet.itemTid: {_itemData.tid}, skillTid: {_itemData.skillTid}");
		}

		finalSkillData = DataManager.Get<SkillDataSheet>().Get(unitData.finalSkillTid);
		if (finalSkillData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet. itemTid: {_itemData.tid}, finalSkillTid: {unitData.finalSkillTid}");
		}

		return _result.SetOk();
	}

	public ItemUnit GetItem()
	{
		return Inventory.it.FindItemByTid(ItemTid) as ItemUnit;
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

		if (Inventory.it.CheckMoney(LevelupConsumeHashtag, LevelupConsumeCount).Fail())
		{
			return false;
		}

		return true;
	}

	public void LevelupItem(Action onLevelupSuccess = null)
	{
		var item = GetItem();

		if (Levelupable())
		{
			if (Inventory.it.ConsumeItem(LevelupConsumeHashtag, LevelupConsumeCount).Ok())
			{
				item.AddExp(ConfigMeta.it.UNIT_LEVELUP_EXP);
				onLevelupSuccess?.Invoke();
			}
		}
	}
}
