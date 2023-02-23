using System;
using System.Collections.Generic;


/// <summary>
/// Calculate(): 계산함수
/// RefreshAbilityTotal() : 계산된 데이터 업데이트. 계산후엔 반드시 호출필요
/// </summary>
public class InventoryAbilityCalculator
{
	public class AbilityCalculator
	{
		public ItemType itemType;


		/// <summary>
		/// 장착시 제일 좋은 아이템
		/// </summary>
		public long bestEquipTid;
		/// <summary>
		/// 장착효과
		/// </summary>
		public Stats equipAbility;
		/// <summary>
		/// 장착효과 값
		/// </summary>
		public IdleNumber equipAbilityValue;
		/// <summary>
		/// 보유 보너스
		/// </summary>
		public Dictionary<Stats, IdleNumber> toOwnAbilities = new Dictionary<Stats, IdleNumber>();


		public AbilityCalculator(ItemType _itemType)
		{
			itemType = _itemType;
		}

		public void Calculate(List<ItemBase> _items)
		{
			toOwnAbilities.Clear();
			ItemBase bestItem = null;
			ItemBase equipItem = null;
			long equipTid = UserInfo.GetUserEquipItem(itemType);

			foreach (var item in _items)
			{
				if (item.Type != itemType)
				{
					continue;
				}

				// 보유효과 계산
				if (toOwnAbilities.ContainsKey(item.ToOwnAbility.type) == false)
				{
					toOwnAbilities.Add(item.ToOwnAbility.type, new IdleNumber());
				}

				toOwnAbilities[item.ToOwnAbility.type] += item.ToOwnAbility.GetValue(item.Level);


				// 장착시 제일 좋은 아이템 체크
				if (bestItem == null)
				{
					bestItem = item;
				}
				else if (bestItem.ToOwnAbility.value < item.ToOwnAbility.value)
				{
					bestItem = item;
				}

				// 장착중인 아이템이 있으면 캐싱
				if (item.Tid == equipTid)
				{
					equipItem = item;
				}
			}

			// 아이템을 아예 보유하지 않은경우 bestItem이 없을 수 있다
			if (bestItem != null)
			{
				bestEquipTid = bestItem.Tid;
			}

			// 장착 아이템 체크
			if (equipItem != null)
			{
				equipAbility = equipItem.EquipAbility.type;
				equipAbilityValue = equipItem.EquipAbility.GetValue(equipItem.Level);
			}
			else
			{
				equipAbility = Stats._NONE;
				equipAbilityValue = new IdleNumber();
			}
		}
	}

	public List<AbilityCalculator> toOwnCalculatorList = new List<AbilityCalculator>();

	public Dictionary<Stats, IdleNumber> abilityTotal = new Dictionary<Stats, IdleNumber>();



	public InventoryAbilityCalculator()
	{
		toOwnCalculatorList = new List<AbilityCalculator>();
		foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
		{
			toOwnCalculatorList.Add(new AbilityCalculator(type));
		}
	}

	/// <summary>
	/// 모든 데이터를 한번에 계산(자주호출하지말것)
	/// </summary>
	public void CalculateAbilityAll(List<ItemBase> _items)
	{
		foreach (var calculator in toOwnCalculatorList)
		{
			calculator.Calculate(_items);
		}
	}

	public AbilityCalculator GetCalculator(ItemType _itemType)
	{
		foreach (var v in toOwnCalculatorList)
		{
			if (v.itemType == _itemType)
			{
				return v;
			}
		}

		//초기화때 모든 데이터를 넣기 때문에 null은 있을수 없음
		return null;
	}

	public void RefreshAbilityTotal()
	{
		abilityTotal.Clear();


		foreach (var calculator in toOwnCalculatorList)
		{
			// 보유보너스 계산
			foreach (var abil in calculator.toOwnAbilities)
			{
				if (abilityTotal.ContainsKey(abil.Key) == false)
				{
					abilityTotal.Add(abil.Key, new IdleNumber());
				}

				abilityTotal[abil.Key] += abil.Value;
			}


			// 장착보너스 계산
			if (calculator.equipAbility != Stats._NONE)
			{
				if (abilityTotal.ContainsKey(calculator.equipAbility) == false)
				{
					abilityTotal.Add(calculator.equipAbility, new IdleNumber());
				}

				abilityTotal[calculator.equipAbility] += calculator.equipAbilityValue;
			}
		}
	}

	public IdleNumber GetAbilityValue(Stats _ability)
	{
		if (abilityTotal.ContainsKey(_ability))
		{
			return abilityTotal[_ability];
		}
		else
		{
			return new IdleNumber();
		}
	}
}
