using System;
using System.Collections.Generic;



public static partial class UserInfo
{
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
	/// 유물
	/// </summary>
	public static RelicInfo relic = new RelicInfo();
	/// <summary>
	/// 보급소
	/// </summary>
	public static AgentInfo agent = new AgentInfo();
	/// <summary>
	/// 진급
	/// </summary>
	public static PromoteInfo promo = new PromoteInfo();
	/// <summary>
	/// 진급능력
	/// </summary>
	public static PromoteAbilityInfo proAbil = new PromoteAbilityInfo();


	/// <summary>
	/// 유저 레벨 변경
	/// </summary>
	public static Action<Int32/* before level*/, Int32 /*after level*/> onLevelupChanged;

	/// <summary>
	/// 전투력 변경
	/// </summary>
	public static Action<IdleNumber, IdleNumber> onTotalCombatChanged;


	public static string userName => userData.userName;
	public static Int32 userLv => UserDataCalculator.GetLevelInfo(userData.userExp).level;
	public static Int64 expTotal => userData.userExp;
	public static Int64 currExp
	{
		get
		{
			var levelInfo = UserDataCalculator.GetLevelInfo(userData.userExp);
			return expTotal - levelInfo.beginExp;
		}
	}
	public static Int64 nextExp
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

	public static string GetAbiltyTitle(UserAbilityType _userAbilityType)
	{
		switch (_userAbilityType)
		{
			case UserAbilityType.AttackPower:
				return "공격력";
			case UserAbilityType.Hp:
				return "HP";
			case UserAbilityType.CriticalChance:
				return "치명타 확률";
			case UserAbilityType.CriticalAttackPower:
				return "치명타 피해";
			case UserAbilityType.GoldUp:
				return "골드 획득량";
			case UserAbilityType.ExpUp:
				return "경험치 획득량";
			case UserAbilityType.ItemUp:
				return "아이템 획득량";
			case UserAbilityType.MoveSpeed:
				return "이동속도";
			case UserAbilityType.AttackSpeed:
				return "공격속도";
			case UserAbilityType.SkillAttackPower:
				return "스킬피해";
			case UserAbilityType.BossAttackPower:
				return "보스피해";
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
		Int32 beforeLv = userLv;
		Int32 afterLv;

		userData.userExp += _exp;
		afterLv = userLv;

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
		double total = userLv * 98765;

		total += userData.training.TotalCombatPower();
		total += userData.currProp.TotalCombatPower();
		total += userData.relic.TotalCombatPower();
		total += userData.agent.TotalCombatPower();

		IdleNumber beforeCombatPower = _totalCombatPower;
		_totalCombatPower = outNumber.Normalize(total);

		onTotalCombatChanged?.Invoke(beforeCombatPower, totalCombatPower);
	}

	public class UserData
	{
		public string userName = "VIVID";
		public Int64 userExp = 65;

		// 훈련
		public TrainingSave training = new TrainingSave();

		// 특성
		public Int32 selectedPropIndex = 0;
		public PropertySave[] props = new PropertySave[0];
		public PropertySave currProp => props[selectedPropIndex];

		// 유물
		public RelicSave relic = new RelicSave();

		// 보급소
		public AgentSave agent = new AgentSave();

		// 진급
		public PromoteSave promo = new PromoteSave();

		// 진급능력
		public PromoteAbilitySave proAbil = new PromoteAbilitySave();



		public UserData()
		{
			props = new PropertySave[PROPERTY_PRESET_COUNT];
			for (Int32 i = 0; i < props.Length; i++)
			{
				props[i] = new PropertySave();
			}
		}
	}
}


public enum UserAbilityType
{
	/// <summary>
	/// 공격력
	/// </summary>
	AttackPower,
	/// <summary>
	/// HP
	/// </summary>
	Hp,
	/// <summary>
	/// 치명타 확률
	/// </summary>
	CriticalChance,
	/// <summary>
	/// 치명타 피해
	/// </summary>
	CriticalAttackPower,
	/// <summary>
	/// 골드 획득량
	/// </summary>
	GoldUp,
	/// <summary>
	/// 경험치 획득량
	/// </summary>
	ExpUp,
	/// <summary>
	/// 아이템 획득량
	/// </summary>
	ItemUp,
	/// <summary>
	/// 이동속도
	/// </summary>
	MoveSpeed,
	/// <summary>
	/// 공격속도
	/// </summary>
	AttackSpeed,
	/// <summary>
	/// 스킬피해
	/// </summary>
	SkillAttackPower,
	/// <summary>
	/// 보스피해
	/// </summary>
	BossAttackPower,
	/// <summary>
	/// 보병 체력증가
	/// </summary>
	WarriorHp,
	/// <summary>
	/// 아쳐 치명타확률 증가
	/// </summary>
	ArcherCriticalChance,
	/// <summary>
	/// 마법사 공격력 증가
	/// </summary>
	WizardAttackPower,
	/// <summary>
	/// 창병 치명타대미지 증가
	/// </summary>
	SpearManCriticalAttackPower,
	/// <summary>
	/// 보급소(병영)
	/// </summary>
	Agent
}


[Serializable]
public class UserAbility
{
	public UserAbilityType type;
	public double value;

	public UserAbility()
	{

	}

	public UserAbility(UserAbilityType _type, double _value)
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

	public virtual Int32 GetLevel(UserAbilityType _ability)
	{
		Int64 tid = DataManager.it.Get<UserAbilityInfoDataSheet>().GetTid(_ability);

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

	public virtual void SetLevel(UserAbilityType _ability, Int32 _value)
	{
		Int64 tid = DataManager.it.Get<UserAbilityInfoDataSheet>().GetTid(_ability);
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

	public virtual double GetValue(UserAbilityType _ability)
	{
		Int64 tid = DataManager.it.Get<UserAbilityInfoDataSheet>().GetTid(_ability);

		if (tid == 0)
		{
			VLog.LogError($"UserAbilityInfoDataSheet에 정의되지 않은 어빌리티. abil: {_ability}, type: {GetType()}");
			return 0;
		}

		for (Int32 i = 0 ; i < saveData.Count ; i++)
		{
			if (saveData[i].tid == tid)
			{
				return saveData[i].value;
			}
		}

		return 0;
	}

	public virtual void SetValue(UserAbilityType _ability, double _value)
	{
		Int64 tid = DataManager.it.Get<UserAbilityInfoDataSheet>().GetTid(_ability);
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
	public double value;


	public UserInfoValueSaveData()
	{

	}

	public UserInfoValueSaveData(Int64 _tid, double _value)
	{
		tid = _tid;
		value = _value;
	}
}
