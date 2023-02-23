using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// <see cref="SaveUserData" /> 데이터 저장
/// </summary>
public static partial class UserInfo
{
	public static string UserDataFilePath
	{
		get
		{
			return Application.dataPath.Replace("/Assets", "") + "/UserData";
		}
	}

	public const int SKILL_SLOT_COUNT = 6;
	public const int PET_SLOT_COUNT = 3;
	public const int CORE_ABILITY_COUNT = 7;
	public const int CORE_ABILITY_PRESET_COUNT = 3;

	public static UserData userData = new UserData(); // 저장데이터. 외부에서 직접 접근은 지양.

	/// 진급능력
	/// </summary>
	public static CoreAbilityInfo coreAbil = new CoreAbilityInfo();


	public static string UserName => userData.userName;
	public static int UID => userData.uid;
	public static long PlayTicks { get => userData.playTicks; set => userData.playTicks = value; }
	public static string PlayTicksToString
	{
		get
		{
			var timeSpan = new TimeSpan(UserInfo.PlayTicks);
			return $"{timeSpan.Days}일 {timeSpan.Hours}시 {timeSpan.Minutes}분 {timeSpan.Seconds}초";
		}
	}
	private static UserGradeData userGrade;
	public static UserGradeData UserGrade
	{
		get
		{
			if(userGrade == null || userGrade.grade != userData.userGrade)
			{
				userGrade = DataManager.Get<UserGradeDataSheet>().Get(userData.userGrade);
			}

			return userGrade;
		}
	}
	public static int UserLv => UserDataCalculator.GetLevelInfo(userData.userExp).level;

	public static long EquipUnitItemTid
	{
		get
		{
			if (userData.equipUnitItemTid == 0)
			{
				var defaultUnit = DataManager.Get<ItemDataSheet>().GetByHashTag("defaultunit");
				if (defaultUnit == null)
				{
					PopAlert.it.Create(new VResult().SetFail(VResultCode.NO_DEFINED_DEFAULT_CHAR), PopupCallback.GoToIntro);
					return 0;
				}

				userData.equipUnitItemTid = defaultUnit.tid;
			}

			return userData.equipUnitItemTid;
		}
	}

	public static long EquipWeaponTid => userData.EquipWeaponTid;
	public static long EquipArmorTid => userData.EquipArmerTid;
	public static long EquipAccessoryTid => userData.EquipAccessoryTid;
	public static int TotalPropertyPoint
	{
		get
		{
			int total = UserDataCalculator.GetPropertyPoint(UserLv);
			return total;
		}
	}
	public static int RemainPropertyPoint
	{
		get
		{
			int result = TotalPropertyPoint - UsingPropertyPoint;

			return result;
		}
	}
	public static int UsingPropertyPoint
	{
		get
		{
			var itemList = Inventory.it.FindItemsByType(ItemType.Property);
			int total = 0;
			foreach (var item in itemList)
			{
				total += (item as ItemProperty).GetUsingPropertyPoint();
			}

			return total;
		}
	}
	public static int TotalMasteryPoint
	{
		get
		{
			var unitList = Inventory.it.FindItemsByType(ItemType.Unit);
			int total = 0;
			foreach(var unit in unitList)
			{
				total += (unit.Level - 1);
			}
			return total;
		}
	}
	public static int RemainMasteryPoint
	{
		get
		{
			int result = TotalMasteryPoint - UsingMasteryPoint;

			return result;
		}
	}
	public static int UsingMasteryPoint
	{
		get
		{
			var itemList = Inventory.it.FindItemsByType(ItemType.Mastery);
			int total = 0;
			foreach (var item in itemList)
			{
				total += (item as ItemMastery).GetUsingMasteryPoint();
			}

			return total;
		}
	}

	public static long[] skills => userData.skillSlots;
	public static long[] pets => userData.petSlots;
	public static List<InstantItem> InstantItems => userData.instantItems;

	public static long expTotal => userData.userExp;
	public static long currExp
	{
		get
		{
			var levelInfo = UserDataCalculator.GetLevelInfo(userData.userExp);
			return expTotal - levelInfo.beginExp;
		}
	}
	public static long nextExp
	{
		get
		{
			var levelInfo = UserDataCalculator.GetLevelInfo(userData.userExp);
			return levelInfo.nextExp - levelInfo.beginExp;
		}
	}
	public static float expRatio
	{
		get
		{
			return (float)Math.Clamp((double)currExp / nextExp, 0, 1);
		}
	}






	public static int GachaEquipLv => Inventory.it.FindItemByHashTag("gachaexp_equip").Level;
	public static long GachaEquipExp => Inventory.it.FindItemByHashTag("gachaexp_equip").Exp;
	public static long NextGachaEquipExp => (Inventory.it.FindItemByHashTag("gachaexp_equip") as ItemGachaExp).nextExp;
	public static int GachaEquipAdSummonCount => Inventory.it.ItemCount("gachaexp_equipad").GetValueToInt();
	public static void IncGachaEquipAdSummonCount()
	{
		Inventory.it.AddItem("gachaexp_equipad", new IdleNumber(1));
	}
	public static void AddGachaEquipExp(int _summonCount)
	{
		Inventory.it.FindItemByHashTag("gachaexp_equip").AddExp(_summonCount);
	}






	public static int GachaSkillLv => Inventory.it.FindItemByHashTag("gachaexp_skill").Level;
	public static long GachaSkillExp => Inventory.it.FindItemByHashTag("gachaexp_skill").Exp;
	public static long NextGachaSkillExp => (Inventory.it.FindItemByHashTag("gachaexp_skill") as ItemGachaExp).nextExp;
	public static int GachaSkillAdSummonCount => Inventory.it.ItemCount("gachaexp_skillad").GetValueToInt();
	public static void IncGachaSkillAdSummonCount()
	{
		Inventory.it.AddItem("gachaexp_skillad", new IdleNumber(1));
	}
	public static void AddGachaSkillExp(int _summonCount)
	{
		Inventory.it.FindItemByHashTag("gachaexp_skill").AddExp(_summonCount);
	}





	public static int GachaPetLv => Inventory.it.FindItemByHashTag("gachaexp_pet").Level;
	public static long GachaPetExp => Inventory.it.FindItemByHashTag("gachaexp_pet").Exp;
	public static long NextGachaPetExp => (Inventory.it.FindItemByHashTag("gachaexp_pet") as ItemGachaExp).nextExp;
	public static int GachaPetAdSummonCount => Inventory.it.ItemCount("gachaexp_petad").GetValueToInt();
	public static void IncGachaPetAdSummonCount()
	{
		Inventory.it.AddItem("gachaexp_petad", new IdleNumber(1));
	}
	public static void AddGachaPetExp(int _summonCount)
	{
		Inventory.it.FindItemByHashTag("gachaexp_pet").AddExp(_summonCount);
	}






	private static IdleNumber _totalCombatPower = new IdleNumber();
	public static IdleNumber totalCombatPower => _totalCombatPower;

	public static void LoadTestUserData()
	{
		userData.MakeTestInstantItems();
		userData.MakeTestSkillSlots();
		userData.MakeTestPetSlots();
	}

	public static void UserGradeUp()
	{
		var sheet = DataManager.Get<UserGradeDataSheet>();
		Grade nextGrade = sheet.NextGrade(UserInfo.UserGrade.grade);

		if(nextGrade < userGrade.grade)
		{
			return;
		}

		userData.userGrade = nextGrade;
	}

	public static void AddExp(Int64 _exp)
	{
		Int32 beforeLv = UserLv;
		Int32 afterLv;

		userData.userExp += _exp;
		afterLv = UserLv;

		if (beforeLv != afterLv)
		{
			EventCallbacks.CallLevelupChanged(beforeLv, afterLv);
			CalculateTotalCombatPower();
		}
	}

	public static bool IsEquipSkill(long _itemTid)
	{
		foreach(var v in userData.skillSlots)
		{
			if(v == _itemTid)
			{
				return true;
			}
		}

		return false;
	}

	public static void EquipSkill(int _index, long _itemTid)
	{
		userData.skillSlots[_index] = _itemTid;
	}

	public static bool IsEquipPet(long _itemTid)
	{
		foreach (var v in userData.petSlots)
		{
			if (v == _itemTid)
			{
				return true;
			}
		}

		return false;
	}

	public static void EquipPet(int _index, long _itemTid)
	{
		userData.petSlots[_index] = _itemTid;
	}


	public static void EquipUnit(long _itemTid)
	{
		userData.equipUnitItemTid = _itemTid;
	}

	public static void EquipWeapon(long _itemTid)
	{
		userData.EquipWeaponTid = _itemTid;
	}
	public static void EquipArmor(long _itemTid)
	{
		userData.EquipArmerTid = _itemTid;
	}
	public static void EquipAccessory(long _itemTid)
	{
		userData.EquipAccessoryTid = _itemTid;
	}

	public static long GetUserEquipItem(ItemType _itemType)
	{
		switch (_itemType)
		{
			case ItemType.Unit:
				return EquipUnitItemTid;
			case ItemType.Weapon:
				return EquipWeaponTid;
			case ItemType.Armor:
				return EquipArmorTid;
			case ItemType.Accessory:
				return EquipAccessoryTid;
			default:
				return 0;
		}
	}

	public static void ResetProperty()
	{
		Inventory.it.ResetProperty();
	}

	public static void ResetMastery()
	{
		Inventory.it.ResetMastery();
	}

	/// <summary>
	/// 전투력 계산. 수치가 바뀔때마다 바로 호출해줘야 함
	/// </summary>
	public static void CalculateTotalCombatPower()
	{
		IdleNumber outNumber = new IdleNumber();
		double total = UserLv * 98765;

		IdleNumber beforeCombatPower = _totalCombatPower;
		_totalCombatPower = outNumber.Normalize(total);

		EventCallbacks.CallTotalCombatChanged(beforeCombatPower, totalCombatPower);
	}

	public static void SaveUserData()
	{
		userData.SaveUserData();
	}
}
