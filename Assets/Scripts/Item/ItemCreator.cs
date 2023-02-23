using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class ItemCreator
{
	public static InstantItem MakeInstantItem(ItemData _itemData)
	{
		var instantItem = new InstantItem();
		instantItem.tid = _itemData.tid;
		instantItem.type = _itemData.itemType;
		instantItem.grade = _itemData.itemGrade;
		if (_itemData.itemType == ItemType.Property)
		{
			instantItem.level = ItemProperty.DEFAULT_LEVEL;
		}
		else if (_itemData.itemType == ItemType.Mastery)
		{
			instantItem.level = 0;
		}
		else if (_itemData.itemType == ItemType.GachaExp)
		{
			instantItem.level = 1;
		}		
		else
		{
			instantItem.level = 1;
		}
		instantItem.exp = 0;
		instantItem.count = new IdleNumber(0);

		return instantItem;
	}

	public static ItemBase MakeItemBase(InstantItem _instantItem, out VResult _vResult)
	{
		ItemBase itemBase;
		_vResult = new VResult();

		switch (_instantItem.type)
		{
			case ItemType.Money:
				itemBase = new ItemMoney();
				break;

			case ItemType.Unit:
				itemBase = new ItemUnit();
				break;

			case ItemType.Weapon:
			case ItemType.Armor:
			case ItemType.Accessory:
				itemBase = new ItemEquip();
				break;

			case ItemType.Skill:
				itemBase = new ItemSkill();
				break;

			case ItemType.Pet:
				itemBase = new ItemPet();
				break;

			case ItemType.Relic:
				itemBase = new ItemRelic();
				break;

			case ItemType.Training:
				itemBase = new ItemTraining();
				break;

			case ItemType.Property:
				itemBase = new ItemProperty();
				break;

			case ItemType.Mastery:
				itemBase = new ItemMastery();
				break;

			case ItemType.Core:
				itemBase = new ItemCore();
				break;

			case ItemType.GachaExp:
				itemBase = new ItemGachaExp();
				break;

			default:
				itemBase = new ItemBase();
				break;
		}

		_vResult = itemBase.Setup(_instantItem);
		if (_vResult.Fail())
		{
			return null;
		}

		return itemBase;
	}
}
