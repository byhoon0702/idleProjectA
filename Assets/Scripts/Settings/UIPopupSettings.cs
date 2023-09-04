using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIPopupSettings : UIBase
{
	[SerializeField] private TextMeshProUGUI textNickname;
	[SerializeField] private TextMeshProUGUI textUuid;
	[SerializeField] private TextMeshProUGUI textPlatform;
	[SerializeField] private Button buttonLogout;

	[Header("BGM")]
	[SerializeField] private Toggle toggleBgmOff;
	[SerializeField] private TextMeshProUGUI textBgmOff;
	[SerializeField] private Toggle toggleBgmOn;
	[SerializeField] private TextMeshProUGUI textBgmOn;

	[Header("FX")]
	[SerializeField] private Toggle toggleFxOff;
	[SerializeField] private TextMeshProUGUI textFxOff;
	[SerializeField] private Toggle toggleFxOn;
	[SerializeField] private TextMeshProUGUI textFxOn;

	[Header("PUSH")]
	[SerializeField] private Toggle togglePushOff;
	[SerializeField] private TextMeshProUGUI textPushOff;
	[SerializeField] private Toggle togglePushOn;
	[SerializeField] private TextMeshProUGUI textPushOn;

	[Header("NIGHT PUSH")]
	[SerializeField] private Toggle toggleNightPushOff;
	[SerializeField] private TextMeshProUGUI textNightPushOff;
	[SerializeField] private Toggle toggleNightPushOn;
	[SerializeField] private TextMeshProUGUI textNightPushOn;

	[Header("POWER SAVE")]
	[SerializeField] private Toggle togglePowerSaveOff;
	[SerializeField] private TextMeshProUGUI textPowerSaveOff;
	[SerializeField] private Toggle togglePowerSaveOn;
	[SerializeField] private TextMeshProUGUI textPowerSaveOn;


	public void Open()
	{
		if (Activate() == false)
		{
			return;
		}
		gameObject.SetActive(true);
		var userInfo = PlatformManager.UserDB.userInfoContainer.userInfo;
		textNickname.text = userInfo.UserName;

		textUuid.text = userInfo.UUID;
		textPlatform.text = "Google";

		buttonLogout.gameObject.SetActive(false);

#if UNITY_EDITOR
		buttonLogout.gameObject.SetActive(true);
#endif

		toggleBgmOff.SetIsOnWithoutNotify(!GameSetting.Instance.Bgm);
		toggleBgmOn.SetIsOnWithoutNotify(GameSetting.Instance.Bgm);

		toggleFxOff.SetIsOnWithoutNotify(!GameSetting.Instance.Fx);
		toggleFxOn.SetIsOnWithoutNotify(GameSetting.Instance.Fx);

		togglePushOff.SetIsOnWithoutNotify(!GameSetting.Instance.Notification);
		togglePushOn.SetIsOnWithoutNotify(GameSetting.Instance.Notification);

		toggleNightPushOff.SetIsOnWithoutNotify(!GameSetting.Instance.NightNotification);
		toggleNightPushOn.SetIsOnWithoutNotify(GameSetting.Instance.NightNotification);

		togglePowerSaveOff.SetIsOnWithoutNotify(!GameSetting.Instance.AutoPowerSave);
		togglePowerSaveOn.SetIsOnWithoutNotify(GameSetting.Instance.AutoPowerSave);

		OnValueChangeBgm(GameSetting.Instance.Bgm);
		OnValueChangeSoundFx(GameSetting.Instance.Fx);
		OnValueChangePush(GameSetting.Instance.Notification);
		OnValueChangeNightPush(GameSetting.Instance.NightNotification);
		OnValueChangePowerSave(GameSetting.Instance.AutoPowerSave);
	}

	public async void OnClickLogout()
	{
		await RemoteConfigManager.Instance.CloudSaveAsync();
		PlatformManager.Instance.LogOut();
	}

	public void CopyClipboard()
	{
		GUIUtility.systemCopyBuffer = PlatformManager.UserDB.userInfoContainer.userInfo.UUID;
		ToastUI.Instance.Enqueue("클립보드 복사");
	}
	public void CopyMailClipboard()
	{
		GUIUtility.systemCopyBuffer = "nclo_help@nclo.net";
		ToastUI.Instance.Enqueue("클립보드 복사");
	}
	public void OnValueChangeBgm(bool isOn)
	{
		GameSetting.Instance.OnBgmChanged(isOn);

		OnChangeToggleColor(textBgmOff, !isOn);
		OnChangeToggleColor(textBgmOn, isOn);
	}

	public void OnValueChangeSoundFx(bool isOn)
	{
		GameSetting.Instance.OnFXChanged(isOn);
		OnChangeToggleColor(textFxOff, !isOn);
		OnChangeToggleColor(textFxOn, isOn);
	}

	public void OnValueChangePush(bool isOn)
	{
		GameSetting.Instance.OnNotificationChanged(isOn);
		OnChangeToggleColor(textPushOff, !isOn);
		OnChangeToggleColor(textPushOn, isOn);

		if (isOn == false)
		{
			OnValueChangeNightPush(false);
			toggleNightPushOff.SetIsOnWithoutNotify(true);
			toggleNightPushOn.SetIsOnWithoutNotify(false);

			toggleNightPushOff.enabled = false;
			toggleNightPushOn.enabled = false;
		}
		else
		{
			toggleNightPushOff.enabled = true;
			toggleNightPushOn.enabled = true;
		}
	}

	public void OnValueChangeNightPush(bool isOn)
	{
		GameSetting.Instance.OnNightNotificationChanged(isOn);
		OnChangeToggleColor(textNightPushOff, !isOn);
		OnChangeToggleColor(textNightPushOn, isOn);
	}

	private void OnChangeToggleColor(TextMeshProUGUI text, bool isOn)
	{

		if (isOn)
		{
			text.color = Color.yellow;
		}
		else
		{
			text.color = Color.gray;
		}
	}

	public void OnValueChangePowerSave(bool isOn)
	{
		GameSetting.Instance.OnAutoPowerSaveChanged(isOn);
		OnChangeToggleColor(textPowerSaveOff, !isOn);
		OnChangeToggleColor(textPowerSaveOn, isOn);
	}

}
