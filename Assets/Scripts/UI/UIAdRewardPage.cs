using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAdRewardPage : UIBase
{
	[SerializeField] private Button freeButton;
	[SerializeField] private Button showAdButton;
	[SerializeField] private TextMeshProUGUI textAdButton;

	[SerializeField] private Button buyPacakgeButton;

	[SerializeField] private Button closeButton;
	[SerializeField] private TextMeshProUGUI itemCount;
	[SerializeField] private Image imageIcon;

	[Space]
	[SerializeField] private GameObject FreeButtonObject;
	[SerializeField] private GameObject ShowAdButtonObject;
	[SerializeField] private GameObject BuyPackageButtonObject;

	private IdleNumber count;

	protected override void OnEnable()
	{
		base.OnEnable();
		freeButton.SetButtonEvent(OnClickFreeButton);
		showAdButton.SetButtonEvent(OnClickShowAdButton);
		buyPacakgeButton.SetButtonEvent(OnClickBuyPackageButton);
		closeButton.SetButtonEvent(Close);
	}

	public void ShowPage()
	{
		gameObject.SetActive(true);


		var reward = PlatformManager.UserDB.inventory.SelectRewardChest.MakeRewardInfo();

		//textAdButton.text = $"{PlatformManager.UserDB.inventory.SelectRewardChest.ViewCount}/{PlatformManager.UserDB.inventory.SelectRewardChest.rawData.dailyViewCount}";
		imageIcon.sprite = reward.iconImage;
		itemCount.text = $"+{reward.fixedCount.ToString()}";

		FreeButtonObject.SetActive(false);
		ShowAdButtonObject.SetActive(true);
		BuyPackageButtonObject.SetActive(true);
	}

	// 광고무료 패키지를 구매했는지 체크
	private void OnClickFreeButton()
	{
		Close();
		PlatformManager.UserDB.inventory.SelectRewardChest.GetReward();
	}

	private void OnClickShowAdButton()
	{
		Close();
		PlatformManager.UserDB.inventory.SelectRewardChest.Watch();
	}

	private void OnClickBuyPackageButton()
	{
		Close();

		UIController.it.BottomMenu.ShopToggle.isOn = true;
	}
}
