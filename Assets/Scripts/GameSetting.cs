using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingProperty
{
	[Header("Sound")]
	public float bgmVolume;
	public float fxVolume;
	public float voiceVolume;

	[Header("Push")]
	public bool pushAlarm;
	public bool pushNightAlarm;

	public SettingProperty()
	{
		bgmVolume = 40;
		fxVolume = 40;
		voiceVolume = 90;

		pushAlarm = false;
		pushNightAlarm = false;
	}

}


public class GameSetting : MonoBehaviour
{
	public static GameSetting it => instance;
	private static GameSetting instance;

	public SettingProperty property;



	private void Awake()
	{
		instance = this;
	}

	public void SaveSettings()
	{
		string toJson = JsonUtility.ToJson(property);
		PlayerPrefs.SetString("GameSettings", toJson);
	}

	public void LoadSettings()
	{
		property = new SettingProperty();
		string json = PlayerPrefs.GetString("GameSettings", "");

		if(json != "")
		{
			JsonUtility.FromJsonOverwrite(json, property);
		}
	}
}
