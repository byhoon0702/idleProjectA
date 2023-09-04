using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class UITopMenu : UIBase
{

	[SerializeField] private Toggle toggleOnOff;
	[SerializeField] private GameObject menuPanel;
	protected override void OnEnable()
	{

	}
	protected override void OnDisable()
	{

	}
	protected override void OnClose()
	{
		toggleOnOff.SetIsOnWithoutNotify(false);
		menuPanel.SetActive(false);
	}

	public void OnValueChange(bool toggle)
	{
		if (toggle)
		{
			GameUIManager.it.uIClosables.Push(this);

		}

		menuPanel.SetActive(toggle);
	}

	public void OnClickSave()
	{
		RemoteConfigManager.Instance.CloudSave(true);
	}

	public void OnClickMail()
	{

	}

	public void OnClickAttendance()
	{
		GameUIManager.it.uiController.ShowAttendance();
	}

	public void OnClickQuest()
	{
		GameUIManager.it.uiController.ShowQuest();
	}

	public void OnClickCollection()
	{
		GameUIManager.it.uiController.ShowCollection();
	}

	public void OnClickSettings()
	{

	}
	public void OnClickCommunity()
	{

	}
	public void OnClickRanking()
	{

	}
	public void OnClickPowerSaveMode()
	{
		if (StageManager.it.CurrentStage.StageType != StageType.Normal)
		{
			return;
		}
		GameManager.it.SleepMode.Open();
	}
}
