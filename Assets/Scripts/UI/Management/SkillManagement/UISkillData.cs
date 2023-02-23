using System;
using System.Collections;
using System.Collections.Generic;



public class UISkillData
{
	private static VResult _result = new VResult();

	public ItemData itemData;
	public SkillData skillData;

	public long ItemTid => itemData.tid;
	public long LevelupConsumeItemTid => ItemTid;
	public IdleNumber LevelupConsumeCount
	{
		get
		{
			var item = GetItem();
			if (item != null)
			{
				return new IdleNumber(item.NextExp);
			}
			else
			{
				return new IdleNumber(0);
			}
		}
	}
	public string Icon => skillData.Icon;
	public string SkillName => itemData.name;
	public string SkillDesc => skillData.description;
	public float SkillCooltime => skillData.cooltime;
	public string SkillCooltimeText => $"{SkillCooltime}초";
	public Grade SkillGrade => itemData.itemGrade;
	public string SkillGradeText => itemData.itemGrade.ToString();
	public bool IsHyperSkill => skillData.isHyperSkill;
	public int SkillLevel
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
	public string SkillLevelText => $"Lv. {SkillLevel}";
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
				return $"{item.Exp} / {item.NextExp}";
			}
			else
			{
				var sheet = DataManager.Get<EquipLevelDataSheet>();
				return $"0 / {sheet.Level1NeedCount()}";
			}
		}
	}
	public string ToOwnAbilityText
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
				return $"보유효과: {abilityData.description} {itemData.ToOwnAbilityInfo.value.ToString()}";
			}
		}
	}



	public VResult Setup(long _itemTid)
	{
		var sheet = DataManager.Get<ItemDataSheet>();
		ItemData itemData = sheet.Get(_itemTid);
		if (itemData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"ItemDataSheet. itemTid: {_itemTid}");
		}

		return Setup(itemData);
	}

	public VResult Setup(ItemData _itemData)
	{
		var sheet = DataManager.Get<SkillDataSheet>();
		var skillData = sheet.Get(_itemData.skillTid);

		if (skillData == null)
		{
			return _result.SetFail(VResultCode.NO_META_DATA, $"SkillDataSheet. itemTid: {_itemData.tid}, skillTid: {_itemData.skillTid}");
		}

		return Setup(_itemData, skillData);
	}

	public VResult Setup(ItemData _itemData, SkillData _skillData)
	{
		itemData = _itemData;
		skillData = _skillData;

		return _result.SetOk();
	}

	public ItemSkill GetItem()
	{
		return Inventory.it.FindItemByTid(itemData.tid) as ItemSkill;
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

	public void LevelupItem(Action onLevelupSuccess = null)
	{
		var item = GetItem();

		if (Upgradable())
		{
			if (Inventory.it.ConsumeItem(LevelupConsumeItemTid, LevelupConsumeCount).Ok())
			{
				item.AddLevel(1);
				onLevelupSuccess?.Invoke();
			}
		}
	}
}
