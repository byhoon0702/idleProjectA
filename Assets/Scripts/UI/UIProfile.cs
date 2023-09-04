using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIProfile : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI textUserName;

	[SerializeField] private TextMeshProUGUI combatPower;

	[SerializeField] private UIPopupProfile uiMain;

	private float time = 0;
	private void Update()
	{
		time += Time.deltaTime;
		if (time >= 0.5f)
		{
			OnupdateAttackPower();
			textUserName.text = $"{PlatformManager.UserDB.userInfoContainer.userInfo.UserName}";
			time = 0;
		}
	}

	public void OnupdateAttackPower()
	{
		IdleNumber total = PlatformManager.UserDB.UserStats.GetTotalPower();
		combatPower.text = total.ToString();
	}

	public void OnClickMain()
	{
		GameUIManager.it.Close();
		uiMain.OnUpdate();
	}
}
