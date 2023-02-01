using System;
using System.Collections.Generic;



public static partial class UserInfo
{
	public const Int32 SKILL_SLOT_COUNT = 6;

	public static UserData userData = new UserData(); // 저장데이터. 외부에서 직접 접근은 지양.

	/// <summary>
	/// 훈련
	/// </summary>
	public static TrainingInfo training = new TrainingInfo();
	/// <summary>
	/// 특성
	/// </summary>
	public static PropertyInfo prop = new PropertyInfo();
	/// <summary>
	/// 진급
	/// </summary>
	public static HyperModeInfo promo = new HyperModeInfo();
	/// <summary>
	/// 진급능력
	/// </summary>
	public static PromoteAbilityInfo proAbil = new PromoteAbilityInfo();


	/// <summary>
	/// 유저 레벨 변경
	/// </summary>
	public static Action<int/* before level*/, int /*after level*/> onLevelupChanged;

	/// <summary>
	/// 전투력 변경
	/// </summary>
	public static Action<IdleNumber, IdleNumber> onTotalCombatChanged;


	public static string UserName => userData.userName;
	public static int UserLv => UserDataCalculator.GetLevelInfo(userData.userExp).level;
	public static long SelectUnitTid => userData.SelectedUnitTid;
	public static long EquipWeaponTid => userData.EquipWeaponTid;
	public static long EquipArmorTid => userData.EquipArmerTid;
	public static long EquipAccessoryTid => userData.EquipAccessoryTid;

	public static long[] skillSlots => userData.skillSlots;

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
	private static IdleNumber _totalCombatPower = new IdleNumber();
	public static IdleNumber totalCombatPower => _totalCombatPower;

	public static string GetAbiltyTitle(AbilityType _userAbilityType)
	{
		switch (_userAbilityType)
		{
			case AbilityType.AttackPower:
				return "공격력";

			case AbilityType.Hp:
				return "체력";

			case AbilityType.HpRecovery:
				return "체력회복량";

			case AbilityType.CriticalChance:
				return "치명타 확률";

			case AbilityType.CriticalAttackPower:
				return "치명타 피해";

			case AbilityType.CriticalX2AttackChance:
				return "하이퍼 어택 확률";

			case AbilityType.CriticalX2AttackPower:
				return "하이퍼 어택 피해량";

			case AbilityType.AttackSpeed:
				return "공격속도";

			case AbilityType.MoveSpeed:
				return "이동속도";

			case AbilityType.DoubleAttack:
				return "더블어택";

			case AbilityType.TripleAttack:
				return "트리플 어택";

			case AbilityType.SkillColltimeDown:
				return "스킬 쿨타임 감소";

			case AbilityType.SkillAttackPower:
				return "스킬 공격력";

			case AbilityType.BossAttackPower:
				return "보스 피해";

			case AbilityType.FriendAttackPower:
				return "동료 공격력";

			case AbilityType.FriendAttackSpeed:
				return "동료 공격속도";

			case AbilityType.Avoid:
				return "회피율";

			case AbilityType.GoldUp:
				return "골드 획득량";

			default:
				return "";
		}
	}

	public static void LoadUserData()
	{
		CalculateTotalCombatPower();
	}

	public static void AddExp(Int64 _exp)
	{
		Int32 beforeLv = UserLv;
		Int32 afterLv;

		userData.userExp += _exp;
		afterLv = UserLv;

		if (beforeLv != afterLv)
		{
			onLevelupChanged?.Invoke(beforeLv, afterLv);
			CalculateTotalCombatPower();
		}
	}

	/// <summary>
	/// 전투력 계산. 수치가 바뀔때마다 바로 호출해줘야 함
	/// </summary>
	public static void CalculateTotalCombatPower()
	{
		IdleNumber outNumber = new IdleNumber();
		double total = UserLv * 98765;

		total += userData.training.TotalCombatPower();
		total += userData.currProp.TotalCombatPower();

		IdleNumber beforeCombatPower = _totalCombatPower;
		_totalCombatPower = outNumber.Normalize(total);

		onTotalCombatChanged?.Invoke(beforeCombatPower, totalCombatPower);
	}

	public class UserData
	{
		public string userName = "VIVID";
		public int uid = 12345678;

		private long selectedUnitTid;
		public long SelectedUnitTid
		{
			get
			{
				if(selectedUnitTid == 0)
				{
					var defaultUnit = DataManager.it.Get<ItemDataSheet>().GetByHashTag("defaultunit");
					if(defaultUnit == null)
					{
						PopAlert.it.Create(new VResult().SetFail(VResultCode.NO_DEFINED_DEFAULT_CHAR), PopupCallback.GoToIntro);
						return 0;
					}

					selectedUnitTid = defaultUnit.tid;
				}

				return selectedUnitTid;
			}
		}

		public long EquipWeaponTid = 10;
		public long EquipArmerTid;
		public long EquipAccessoryTid;

		public long[] skillSlots = new long[SKILL_SLOT_COUNT];

		public Int64 userExp = 65;


		// 훈련
		public TrainingSave training = new TrainingSave();

		// 특성
		public Int32 selectedPropIndex = 0;
		public PropertySave[] props = new PropertySave[0];
		public PropertySave currProp => props[selectedPropIndex];

		// 진급
		public HyperModeSave promo = new HyperModeSave();

		// 진급능력
		public PromoteAbilitySave proAbil = new PromoteAbilitySave();



		public UserData()
		{
			props = new PropertySave[PROPERTY_PRESET_COUNT];
			for (Int32 i = 0; i < props.Length; i++)
			{
				props[i] = new PropertySave();
			}


			skillSlots[0] = 1004;
			skillSlots[1] = 1005;
		}
	}
}


public enum AbilityType
{
	_NONE,
	/// <summary>
	/// 공격력
	/// </summary>
	AttackPower,
	/// <summary>
	/// HP
	/// </summary>
	Hp,
	/// <summary>
	/// 체력 회복량
	/// </summary>
	HpRecovery,
	/// <summary>
	/// 치명타 확률
	/// </summary>
	CriticalChance,
	/// <summary>
	/// 치명타 피해
	/// </summary>
	CriticalAttackPower,
	/// <summary>
	/// 하이퍼 어택 확률
	/// </summary>
	CriticalX2AttackChance,
	/// <summary>
	/// 하이퍼 어택 피해량
	/// </summary>
	CriticalX2AttackPower,
	/// <summary>
	/// 공격속도
	/// </summary>
	AttackSpeed,
	/// <summary>
	/// 이동속도
	/// </summary>
	MoveSpeed,
	/// <summary>
	/// 더블어택
	/// </summary>
	DoubleAttack,
	/// <summary>
	/// 트리플 어택
	/// </summary>
	TripleAttack,
	/// <summary>
	/// 스킬 쿨타임 감소
	/// </summary>
	SkillColltimeDown,
	/// <summary>
	/// 스킬피해
	/// </summary>
	SkillAttackPower,
	/// <summary>
	/// 보스피해
	/// </summary>
	BossAttackPower,
	/// <summary>
	/// 동료 공격력
	/// </summary>
	FriendAttackPower,
	/// <summary>
	/// 동료 공격속도
	/// </summary>
	FriendAttackSpeed,
	/// <summary>
	/// 회피율
	/// </summary>
	Avoid,
	/// <summary>
	/// 골드 획득량
	/// </summary>
	GoldUp,
}


[Serializable]
public class AbilityInfo
{
	public AbilityType type;
	public IdleNumber value;

	public AbilityInfo()
	{

	}

	public AbilityInfo(AbilityType _type, IdleNumber _value)
	{
		type = _type;
		value = _value;
	}

	public override string ToString()
	{
		return $"{type}. {value}";
	}
}

public abstract class UserInfoLevelSaveBase
{
	public abstract Int32 defaultLevel { get; }
	public List<UserInfoLevelSaveData> saveData = new List<UserInfoLevelSaveData>();

	public abstract double TotalCombatPower();

	public virtual Int32 GetLevel(AbilityType _ability)
	{
		Int64 tid = DataManager.it.Get<AbilityInfoDataSheet>().GetTid(_ability);

		if (tid == 0)
		{
			VLog.LogError($"UserAbilityInfoDataSheet에 정의되지 않은 어빌리티. abil: {_ability}, type: {GetType()}");
			return defaultLevel;
		}

		for (Int32 i = 0; i < saveData.Count; i++)
		{
			if (saveData[i].tid == tid)
			{
				return saveData[i].value;
			}
		}

		return defaultLevel;
	}

	public virtual void SetLevel(AbilityType _ability, Int32 _value)
	{
		Int64 tid = DataManager.it.Get<AbilityInfoDataSheet>().GetTid(_ability);
		if (tid == 0)
		{
			VLog.LogError($"UserAbilityInfoDataSheet에 정의되지 않은 어빌리티. abil: {_ability}, type: {GetType()}");
			return;
		}

		for (Int32 i = 0; i < saveData.Count; i++)
		{
			if (saveData[i].tid == tid)
			{
				saveData[i].value = _value;
				return;
			}
		}

		saveData.Add(new UserInfoLevelSaveData(tid, _value));
	}
}


public abstract class UserInfoValueSaveBase
{
	public List<UserInfoValueSaveData> saveData = new List<UserInfoValueSaveData>();

	public abstract double TotalCombatPower();

	public virtual IdleNumber GetValue(AbilityType _ability)
	{
		Int64 tid = DataManager.it.Get<AbilityInfoDataSheet>().GetTid(_ability);

		if (tid == 0)
		{
			VLog.LogError($"UserAbilityInfoDataSheet에 정의되지 않은 어빌리티. abil: {_ability}, type: {GetType()}");
			return new IdleNumber();
		}

		for (Int32 i = 0 ; i < saveData.Count ; i++)
		{
			if (saveData[i].tid == tid)
			{
				return saveData[i].value;
			}
		}

		return new IdleNumber();
	}

	public virtual void SetValue(AbilityType _ability, IdleNumber _value)
	{
		Int64 tid = DataManager.it.Get<AbilityInfoDataSheet>().GetTid(_ability);
		if (tid == 0)
		{
			VLog.LogError($"UserAbilityInfoDataSheet에 정의되지 않은 어빌리티. abil: {_ability}, type: {GetType()}");
			return;
		}

		for (Int32 i = 0 ; i < saveData.Count ; i++)
		{
			if (saveData[i].tid == tid)
			{
				saveData[i].value = _value;
				return;
			}
		}

		saveData.Add(new UserInfoValueSaveData(tid, _value));
	}
}


[Serializable]
public class UserInfoLevelSaveData
{
	public Int64 tid;
	public Int32 value;


	public UserInfoLevelSaveData()
	{

	}

	public UserInfoLevelSaveData(Int64 _tid, Int32 _value)
	{
		tid = _tid;
		value = _value;
	}
}

[Serializable]
public class UserInfoValueSaveData
{
	public Int64 tid;
	public IdleNumber value;


	public UserInfoValueSaveData()
	{

	}

	public UserInfoValueSaveData(Int64 _tid, IdleNumber _value)
	{
		tid = _tid;
		value = _value;
	}
}
