using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = System.Random;



public class GachaResult
{
	public long itemTid;
	public IdleNumber itemCount;

	public GachaResult(long _itemTid, IdleNumber _itemCount)
	{
		itemTid = _itemTid;
		itemCount = _itemCount;
	}
}

public static class GachaGenerator
{
	private static Random gradeSeed = new Random();
	private static Random typeSeed = new Random();
	private static Random itemSeed = new Random();



	public static GachaResult GenerateEquip(int _level, GachaEntryProbabilityInfo[] _probabilities)
	{
		ItemType[] types = new ItemType[] { ItemType.Weapon, ItemType.Armor, ItemType.Accessory };
		ItemType type = types[typeSeed.Next(0, 3)];

		return GenerateItem(_level, _probabilities, type);
	}

	public static GachaResult GenerateSkill(int _level, GachaEntryProbabilityInfo[] _probabilities)
	{
		return GenerateItem(_level, _probabilities, ItemType.Skill);
	}

	public static GachaResult GeneratePet(int _level, GachaEntryProbabilityInfo[] _probabilities)
	{
		return GenerateItem(_level, _probabilities, ItemType.Pet);
	}

	public static GachaResult GenerateItem(int _level, GachaEntryProbabilityInfo[] _probabilities, ItemType _itemType)
	{
		Grade grade = RandomGrade(_level, _probabilities);

		var itemList = DataManager.Get<ItemDataSheet>().GetByItemTypeAndGrade(_itemType, grade);
		int index = itemSeed.Next(0, itemList.Count);

		GachaResult resultItem = new GachaResult(itemList[index].tid, new IdleNumber(1));

		return resultItem;
	}

	/// <summary>
	/// 랜덤 등급
	/// </summary>
	public static Grade RandomGrade(int _level, GachaEntryProbabilityInfo[] _probabilities)
	{
		double value = gradeSeed.NextDouble();
		double accum = 0;
		for (int i = _probabilities.Length - 1 ; i >= 0 ; i--)
		{
			accum += _probabilities[i].GetRatio(_level);
			if (accum >= value)
			{
				return _probabilities[i].grade;
			}
		}

		return Grade.D;
	}
}
