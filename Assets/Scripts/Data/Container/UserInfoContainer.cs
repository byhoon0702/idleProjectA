using System;

using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class LoginInfo
{
	public string uuid;
	public string nickName;
	public string platform;

}


[System.Serializable]
public class UserInfo
{

	public string UUID;
	public int UserLevel;
	private int _lastUserLevel = 0;

	public IdleNumber UserExp;
	public string UserName;
	public Sprite UserProfileIcon;

	public string LoginTime;

	public string LoginPlatform;

	[SerializeField] private long playTicks;

	public IdleNumber CurrentExp;

	private long _expForLevelUP = 0;
	public long PlayTicks { get => playTicks; set => playTicks = value; }
	[SerializeField] private long dailyPlayTicks;
	public long DailyPlayTicks { get => dailyPlayTicks; set => dailyPlayTicks = value; }
	public int KillPerMinutes;

	public string PlayTicksToString
	{
		get
		{
			var timeSpan = new TimeSpan(PlayTicks);
			return $"{timeSpan.Days}일 {timeSpan.Hours}시 {timeSpan.Minutes}분 {timeSpan.Seconds}초";
		}
	}
	public string DailyPlayTicksToString
	{
		get
		{
			var timeSpan = new TimeSpan(DailyPlayTicks);
			return $"{timeSpan.Days}일 {timeSpan.Hours}시 {timeSpan.Minutes}분 {timeSpan.Seconds}초";
		}
	}
	public UserLevelData userLevelData { get; private set; }

	public long ExpForLevelUP
	{
		get
		{

			if (UserLevel != _lastUserLevel || _expForLevelUP == 0)
			{
				_expForLevelUP = CalculateExpForLevelUp();
				_lastUserLevel = UserLevel;
			}

			return _expForLevelUP;
		}
	}
	public UserInfo()

	{
		UUID = "";
		UserLevel = 1;
		_lastUserLevel = 0;

		UserExp = (IdleNumber)0;
		UserName = "";

		LoginTime = "";
		LoginPlatform = "";

		playTicks = 0;
		KillPerMinutes = 0;
		CurrentExp = (IdleNumber)0;
	}

	public void SetRawData(UserLevelData data)
	{
		userLevelData = data;
	}

	private long CalculateExpForLevelUp()
	{
		if (userLevelData == null)
		{
			return 0;
		}
		int levelMinus = UserLevel - 1;

		int levelIncrement = levelMinus * userLevelData.expIncrement;
		float weight = userLevelData.expIncrement * ((UserLevel * userLevelData.secondWeight) * userLevelData.firstWeight) * levelMinus;

		long result = (long)Mathf.Floor(userLevelData.baseExp + (levelIncrement * weight));

		return result;
	}
}

public delegate void OnExpEarned(float ratio, UserInfo userInfo);

[CreateAssetMenu(fileName = "UserInfo Container", menuName = "ScriptableObject/Container/UserInfo Container", order = 1)]

public class UserInfoContainer : BaseContainer
{
	public event OnExpEarned OnExpEarned;
	public static event Action<int> OnLevelUpEvent;
	public UserInfo userInfo;

	public string LastLoginTime;
	public int dailyKillCount = 0;
	public override void Dispose()
	{

	}
	private void Awake()
	{

	}
	public void SetUserInfo(UserInfo info)
	{
		SetUserInfo("", info.UserLevel, info.CurrentExp, info.UserExp);

		userInfo.KillPerMinutes = info.KillPerMinutes;
	}

	public override void DailyResetData()
	{
		userInfo.DailyPlayTicks = 0;
	}

	public void SetAccountInfo(string name, string uuid)
	{
		if (userInfo == null)
		{
			userInfo = new UserInfo();
		}
		userInfo.UUID = uuid;
		userInfo.UserName = name;

		if (PlayerPrefs.HasKey(UserDBSave.k_LoginSave))
		{
			string json = PlayerPrefs.GetString(UserDBSave.k_LoginSave);
			if (json.IsNullOrEmpty())
			{
				PlayerPrefs.DeleteKey(UserDBSave.k_LocalSave);
				return;
			}
			var loginInfo = (LoginInfo)JsonUtility.FromJson(json, typeof(LoginInfo));

			if (uuid != loginInfo.uuid)
			{
				PlayerPrefs.DeleteKey(UserDBSave.k_LocalSave);
			}
		}
	}

	public void SetUserInfo(string profile_icon, int level, IdleNumber exp, IdleNumber totalExp)
	{
		if (userInfo == null)
		{
			userInfo = new UserInfo();
		}
		userInfo.SetRawData(DataManager.Get<UserLevelDataSheet>().GetDataByLevel(level));

		userInfo.UserLevel = userInfo.UserLevel > level ? userInfo.UserLevel : level;
		userInfo.UserExp = userInfo.UserExp > totalExp ? userInfo.UserExp : totalExp;
		userInfo.CurrentExp = userInfo.CurrentExp > exp ? userInfo.CurrentExp : exp;

		OnExpEarned?.Invoke((float)userInfo.CurrentExp / userInfo.ExpForLevelUP, userInfo);
	}

	public void SetLoginPlatform(string platform)
	{
		userInfo.LoginPlatform = platform;
	}

	public void GainUserExp(IdleNumber exp)
	{
		userInfo.CurrentExp += exp;
		userInfo.UserExp += exp;
		LevelUp();

		OnExpEarned?.Invoke((float)userInfo.CurrentExp / userInfo.ExpForLevelUP, userInfo);
	}

	public void LevelUp()
	{
		bool isLevelUp = false;
		int prevLevel = userInfo.UserLevel;
		while (userInfo.CurrentExp >= userInfo.ExpForLevelUP)
		{
			isLevelUp = true;
			userInfo.CurrentExp -= userInfo.ExpForLevelUP;
			userInfo.UserLevel++;
			userInfo.SetRawData(DataManager.Get<UserLevelDataSheet>().GetDataByLevel(userInfo.UserLevel));

			parent.veterancyContainer.AddVeterancyPoint();
			OnLevelUpEvent?.Invoke(userInfo.UserLevel);

			RemoteConfigManager.Instance.UpdateUserLevel(userInfo.UserLevel);
			PlatformManager.UserDB.questContainer.ProgressAdd(QuestGoalType.USER_LEVELUP, 0, (IdleNumber)1);
			PlatformManager.UserDB.questContainer.ProgressOverwrite(QuestGoalType.USER_LEVEL, 0, (IdleNumber)userInfo.UserLevel);
		}

		if (isLevelUp)
		{
			RemoteConfigManager.Instance.CloudSave();
			//ToastUI.it.Enqueue($"레벨업!! {prevLevel} -> {userInfo.UserLevel}");
		}
	}

	public override void Load(UserDB _parent)
	{
		parent = _parent;
		if (userInfo == null)
		{
			userInfo = new UserInfo();
		}
		LastLoginTime = "";
		dailyKillCount = 0;
		//OnExpEarned = null;
	}
	public override void UpdateData()
	{

	}
	public override void LoadScriptableObject()
	{

	}

	public override string Save()
	{
		var json = JsonUtility.ToJson(this, true);
		return json;
	}

	public override void FromJson(string json)
	{
		UserInfoContainer temp = CreateInstance<UserInfoContainer>();

		JsonUtility.FromJsonOverwrite(json, temp);

		SetAccountInfo(temp.userInfo.UserName, temp.userInfo.UUID);
		SetUserInfo(temp.userInfo);

		LastLoginTime = temp.LastLoginTime;
		dailyKillCount = temp.dailyKillCount;
	}
}
