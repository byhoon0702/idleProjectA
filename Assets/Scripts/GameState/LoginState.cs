using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginState : RootState
{

	public override void OnEnter()
	{
		elapsedTime = 0;

		Intro.it.SetActiveTabToStart(true);

		string userFileName = PlayerPrefs.GetString("LoadUserInfoFileName", "");
		if (userFileName.HasStringValue())
		{
			try
			{
				string json = System.IO.File.ReadAllText($"{UserInfo.UserDataFilePath}/{userFileName}.json");
				JsonUtility.FromJsonOverwrite(json, UserInfo.userData);
				Debug.Log($"UserData 로드. 파일명: {userFileName}. name: {UserInfo.UserName}({UserInfo.UID})");
			}
			catch (Exception e)
			{
				Debug.LogError($"UserData 로드 실패. 파일명: {userFileName}\n{e}");
				UserInfo.LoadTestUserData();
			}
		}
		else
		{
			Debug.Log($"UserData TestData 로드. name: {UserInfo.UserName}({UserInfo.UID})");
			UserInfo.LoadTestUserData();
		}

		UserInfo.CalculateTotalCombatPower();
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float time)
	{
		elapsedTime += time;
	}
}
