using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIStageFail : UIStageResult
{
	[SerializeField] private TextMeshProUGUI textTitle;
	[SerializeField] private Button buttonOk;
	[SerializeField] private UITextMeshPro textButton;
	[SerializeField] private Button buttonHeroNavigation;
	[SerializeField] private Button buttonEquipNavigation;
	[SerializeField] private Button buttonGachaNavigation;
	[SerializeField] private Button buttonPetNavigation;

	float autoClickTime;
	float autoClickTimer;
	private RuntimeData.StageInfo currentStage;
	private void Awake()
	{
		buttonOk.SetButtonEvent(Close);
		buttonHeroNavigation.SetButtonEvent(OnClickNavigateHero);
		buttonEquipNavigation.SetButtonEvent(OnClickNavigateEquip);
		buttonGachaNavigation.SetButtonEvent(OnClickNavigateGacha);
		buttonPetNavigation.SetButtonEvent(OnClickNavigatePet);

	}

	private void OnClickNavigateHero()
	{
		Close();
		UIController.it.BottomMenu.ToggleHero.isOn = true;
	}
	private void OnClickNavigateEquip()
	{
		Close();
		UIController.it.BottomMenu.ToggleEquipment.isOn = true;
	}
	private void OnClickNavigateGacha()
	{
		Close();
		UIController.it.BottomMenu.ToggleGacha.isOn = true;
	}
	private void OnClickNavigatePet()
	{
		Close();
		UIController.it.BottomMenu.TogglePet.isOn = true;
	}


	public override void Show(StageRule _rule)
	{
		base.Show(_rule);
		if (rule.isWin)
		{
			gameObject.SetActive(false);
			return;
		}
		currentStage = StageManager.it.CurrentStage;
		gameObject.SetActive(true);
		autoClickTimer = 15;
		autoClickTime = 0;
		textButton.SetKey("str_ui_ok").Append($" {autoClickTimer}초");


		bool heroOpen = PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.HERO);
		buttonHeroNavigation.gameObject.SetActive(heroOpen);

		bool equipOpen = PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.EQUIP);
		buttonEquipNavigation.gameObject.SetActive(equipOpen);

		bool gachaOpen = PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.GACHA);
		buttonGachaNavigation.gameObject.SetActive(gachaOpen);

		bool petOpen = PlatformManager.UserDB.contentsContainer.IsOpen(ContentType.PET);
		buttonPetNavigation.gameObject.SetActive(petOpen);
	}
	protected override void OnClose()
	{
		rule.End();
		gameObject.SetActive(false);
	}


	private void Update()
	{
		autoClickTime += Time.deltaTime;
		if (autoClickTime > autoClickTimer)
		{
			Close();
			autoClickTime = 0;
		}
		textButton.SetKey("str_ui_ok").Append($" {Mathf.RoundToInt(autoClickTimer - autoClickTime)}초");
	}
}
