using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAdRewardPage : MonoBehaviour
{
	[SerializeField] private Button freeButton;
	[SerializeField] private Button showAdButton;
	[SerializeField] private Button buyPacakgeButton;

	[SerializeField] private Button closeButton;
	[SerializeField] private TextMeshProUGUI itemCount;

	[Space]
	[SerializeField] private GameObject goldIcon;
	[SerializeField] private GameObject diaIcon;

	[Space]
	[SerializeField] private GameObject FreeButtonObject;
	[SerializeField] private GameObject ShowAdButtonObject;
	[SerializeField] private GameObject BuyPackageButtonObject;

	private ItemData rewardItemData;
	private IdleNumber count;

	private void OnEnable()
	{
		freeButton.onClick.RemoveAllListeners();
		freeButton.onClick.AddListener(OnClickFreeButton);

		showAdButton.onClick.RemoveAllListeners();
		showAdButton.onClick.AddListener(OnClickShowAdButton);

		buyPacakgeButton.onClick.RemoveAllListeners();
		buyPacakgeButton.onClick.AddListener(OnClickBuyPackageButton);

		closeButton.onClick.RemoveAllListeners();
		closeButton.onClick.AddListener(OnClickCloseButton);
	}

	public void ShowPage(int _rewardTid, IdleNumber _count)
	{
		gameObject.SetActive(true);

		ItemData rewardItemData = DataManager.GetFromAll<ItemData>(_rewardTid);
		count = _count;

		itemCount.text = count.ToString();

		//bool isGold = rewardItemData.tid == Inventory.it.GoldTid;
		//bool isDia = rewardItemData.tid == Inventory.it.DiaTid;

		//goldIcon.SetActive(isGold);
		//diaIcon.SetActive(isDia);

		ShowFreeReward();
	}

	// 광고무료 패키지를 구매했는지 체크
	private void ShowFreeReward()
	{
		FreeButtonObject.SetActive(true);
		ShowAdButtonObject.SetActive(false);
		BuyPackageButtonObject.SetActive(false);
	}

	private void ShowAdReward()
	{
		FreeButtonObject.SetActive(false);
		ShowAdButtonObject.SetActive(true);
		BuyPackageButtonObject.SetActive(true);
	}

	private void OnClickFreeButton()
	{

	}

	private void OnClickShowAdButton()
	{

	}

	private void OnClickBuyPackageButton()
	{

	}

	private void OnClickCloseButton()
	{
		gameObject.SetActive(false);
	}
}
