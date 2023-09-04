using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SettingProperty
{
	[Header("Sound")]
	public bool bgmOn;
	public bool fxOn;

	[Header("Push")]
	public bool pushAlarm;
	public bool pushNightAlarm;

	public bool powerSaveOn;

	public SettingProperty()
	{
		bgmOn = true;
		fxOn = true;

		powerSaveOn = true;
		pushAlarm = false;
		pushNightAlarm = false;
	}

}


public class GameSetting : MonoBehaviour
{
	public static GameSetting Instance;

	[SerializeField] private SettingProperty property;
	public bool Bgm => property.bgmOn;
	public bool Fx => property.fxOn;
	public bool Notification => property.pushAlarm;
	public bool NightNotification => property.pushNightAlarm;


	public bool AutoPowerSave => property.powerSaveOn;
	public event UnityAction<bool> BgmChanged;
	public event UnityAction<bool> FxChanged;
	public event UnityAction<bool> NotificationChanged;
	public event UnityAction<bool> NightNotificationChanged;
	public event UnityAction<bool> AutoPowerSaveChanged;

	private void Awake()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			if (Instance.gameObject != null)
			{
				if (Instance.gameObject != gameObject)
				{
					Destroy(gameObject);
				}
			}
			else
			{
				Instance = null;
				Instance = this;
			}
		}
		DontDestroyOnLoad(gameObject);


	}

	public void OnBgmChanged(bool isOn)
	{
		property.bgmOn = isOn;
		BgmChanged?.Invoke(isOn);
		SaveSettings();
	}
	public void OnFXChanged(bool isOn)
	{
		property.fxOn = isOn;
		FxChanged?.Invoke(isOn);
		SaveSettings();
	}
	public void OnNotificationChanged(bool isOn)
	{
		property.pushAlarm = isOn;
		NotificationChanged?.Invoke(isOn);
		SaveSettings();
	}
	public void OnNightNotificationChanged(bool isOn)
	{
		property.pushNightAlarm = isOn;
		NightNotificationChanged?.Invoke(isOn);
		SaveSettings();
	}
	public void OnAutoPowerSaveChanged(bool isOn)
	{
		property.powerSaveOn = isOn;
		AutoPowerSaveChanged?.Invoke(isOn);
		SaveSettings();
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

		if (json != "")
		{
			JsonUtility.FromJsonOverwrite(json, property);
		}
	}
}
