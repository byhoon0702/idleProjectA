using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Authentication;
using TMPro;
using System.Text.RegularExpressions;

public class UIPopupNicknameChange : UIBase
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private TextMeshProUGUI textDescription;
	[SerializeField] private TMP_InputField inputField;

	[SerializeField] private UIEconomyButton button;
	[SerializeField] private Button buttonFree;


	void Awake()
	{
		button.SetButtonEvent(null, OnClickOk);
		buttonFree.SetButtonEvent(OnClickOk);
	}
	bool isChanged;
	protected override void OnActivate()
	{
		var currency = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.DIA);
		int cost = PlatformManager.CommonData.NickNameChangeCost;
		button.enabled = true;
		button.SetButton(currency.IconImage, $"{cost}", currency.Value >= cost);
		button.SetLabel(PlatformManager.Language["str_ui_nickname_change"]);
		inputField.text = "";

		isChanged = PlatformManager.UserDB.userInfoContainer.NameChanged;
		if (isChanged)
		{
			buttonFree.gameObject.SetActive(false);
			button.gameObject.SetActive(true);
			textDescription.text = PlatformManager.Language["str_ui_nickname_description"];
		}
		else
		{
			buttonFree.gameObject.SetActive(true);
			button.gameObject.SetActive(false);
			textDescription.text = PlatformManager.Language["str_ui_nickname_description_first"];
		}
	}

	public async void OnClickOk()
	{
		var currency = PlatformManager.UserDB.inventory.FindCurrency(CurrencyType.DIA);
		if (isChanged && currency.Check(PlatformManager.CommonData.NickNameChangeCost) == false)
		{
			ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_warn_lack_of_currency"]);
			return;
		}

		string nickName = inputField.text;

		if (nickName.Equals(PlatformManager.UserDB.userInfoContainer.userInfo.UserName))
		{
			textDescription.text = PlatformManager.Language["str_ui_warn_same_nickname"];
			return;
		}
		if (nickName.NickNameValidate(out string message) == false)
		{
			textDescription.text = message;
			return;
		}

		button.enabled = false;
		PlatformManager.UserDB.userInfoContainer.NameChange(nickName);
		PlatformManager.UserDB.userInfoContainer.NameChanged = true;
		await PlatformManager.RemoteSave.CloudSaveAsync(false);
		currency.Pay(PlatformManager.CommonData.NickNameChangeCost);
		await AuthenticationService.Instance.UpdatePlayerNameAsync(nickName);

		if (this == null)
		{
			return;
		}
		ToastUI.Instance.Enqueue(PlatformManager.Language["str_ui_nickname_change_success"]);

		var profile = GameObject.FindObjectOfType<UIPopupProfile>();
		if (profile != null)
		{
			profile.UpdateUnitInfo();
		}
		gameObject.SetActive(false);
	}
}
