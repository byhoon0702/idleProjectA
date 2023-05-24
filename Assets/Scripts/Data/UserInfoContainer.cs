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
	[JsonIgnore] private int _lastUserLevel = 0;

	public long UserExp;
	public string UserName;
	[JsonIgnore] public Sprite UserProfileIcon;

	public string LastLoginTime;
	public string LoginTime;

	public string LoginPlatform;

	public long playTicks;

	public long CurrentExp;

	[JsonIgnore] private long _expForLevelUP = 0;
	public long PlayTicks { get => playTicks; set => playTicks = value; }
	[JsonIgnore]
	public string PlayTicksToString
	{
		get
		{
			var timeSpan = new TimeSpan(PlayTicks);
			return $"{timeSpan.Days}일 {timeSpan.Hours}시 {timeSpan.Minutes}분 {timeSpan.Seconds}초";
		}
	}
	[JsonIgnore] public UserLevelData userLevelData { get; private set; }
	[JsonIgnore]
	public long ExpForLevelUP
	{
		get
		{

			if (UserLevel != _lastUserLevel)
			{
				_expForLevelUP = CalculateExpForLevelUp();
				_lastUserLevel = UserLevel;
			}

			return _expForLevelUP;
		}
	}

	public void SetRawData(UserLevelData data)
	{
		userLevelData = data;
	}
	private long CalculateExpForLevelUp()
	{
		int levelMinus = UserLevel - 1;

		int levelIncrement = levelMinus * userLevelData.expIncrement;
		float weight = userLevelData.expIncrement * ((UserLevel * userLevelData.secondWeight) * userLevelData.firstWeight) * levelMinus;

		long result = (long)Mathf.Floor(userLevelData.baseExp + (levelIncrement * weight));

		return result;
	}
}

public delegate void OnExpEarned(float ratio);

[CreateAssetMenu(fileName = "UserInfo Container", menuName = "ScriptableObject/Container/UserInfo Container", order = 1)]

public class UserInfoContainer : BaseContainer
{
	public event OnExpEarned onExpEarned;
	public UserInfo userInfo;

	public void SetUserInfo(UserInfo info)
	{
		if (userInfo == null)
		{
			userInfo = new UserInfo();
		}
		userInfo.UUID = info.UUID;
		userInfo.UserName = info.UserName;
		//UserProfileIcon = (Sprite)Resources.Load(profile_icon);
		userInfo.UserLevel = info.UserLevel;
		userInfo.UserExp = info.UserExp;
		userInfo.SetRawData(DataManager.Get<UserLevelDataSheet>().GetDataByLevel(userInfo.UserLevel));
		onExpEarned?.Invoke((float)userInfo.CurrentExp / userInfo.ExpForLevelUP);
	}

	public void SetUserInfo(string name, string uuid, string profile_icon, int level, long exp)
	{
		if (userInfo == null)
		{
			userInfo = new UserInfo();
		}
		userInfo.UUID = uuid;
		userInfo.UserName = name;
		//UserProfileIcon = (Sprite)Resources.Load(profile_icon);
		userInfo.UserLevel = level;
		userInfo.UserExp = exp;
		userInfo.SetRawData(DataManager.Get<UserLevelDataSheet>().GetDataByLevel(userInfo.UserLevel));
		onExpEarned?.Invoke((float)userInfo.CurrentExp / userInfo.ExpForLevelUP);
	}

	public void SetLoginPlatform(string platform)
	{
		userInfo.LoginPlatform = platform;
	}

	public void GainUserExp(long exp)
	{
		userInfo.CurrentExp += exp;
		userInfo.UserExp += exp;
		LevelUp();

		onExpEarned?.Invoke((float)userInfo.CurrentExp / userInfo.ExpForLevelUP);
	}

	public void LevelUp()
	{
		if (userInfo.CurrentExp >= userInfo.ExpForLevelUP)
		{
			userInfo.CurrentExp -= userInfo.ExpForLevelUP;
			userInfo.UserLevel++;
			userInfo.SetRawData(DataManager.Get<UserLevelDataSheet>().GetDataByLevel(userInfo.UserLevel));
		}
	}



	public override void Load(UserDB _parent)
	{
		//throw new NotImplementedException();
	}

	public override void LoadScriptableObject()
	{
		//throw new NotImplementedException();
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

		SetUserInfo(temp.userInfo);
	}
}
