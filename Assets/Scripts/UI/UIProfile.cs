using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIProfile : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textUserName;
	//[SerializeField] private TextMeshProUGUI textUserLevel;

	[SerializeField] private TextMeshProUGUI combatPower;

	//[SerializeField] private Button buffButton;
	[SerializeField]
	private UIManagementMain uiMain;

	private void Update()
	{
		textUserName.text = $"{GameManager.UserDB.userInfoContainer.userInfo.UserName}";
		//textUserLevel.text = $"{UserInfo.info.UserLv}";
		//combatPower.text = $"{VGameManager.UserDB.ToString()}";
	}

	private void OnEnable()
	{
		//buffButton.onClick.RemoveAllListeners();
		//buffButton.onClick.AddListener(OpenBuffPage);
	}

	private void OpenBuffPage()
	{
		UIController.it.ShowBuffPage();
	}

	public void OnClickMain()
	{
		GameUIManager.it.OnClose();
		uiMain.OnUpdate();
	}
}
