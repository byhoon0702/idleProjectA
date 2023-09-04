using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdRewardChest : MonoBehaviour
{
	[SerializeField] private UIAdRewardPage rewardPage;

	[SerializeField] private UIAdRewardChestItem goldChest;

	public void Init()
	{
		PlatformManager.UserDB.inventory.SelectAdsRewardChest();
		PlatformManager.UserDB.inventory.SelectRewardChest?.SetEvent(() => { goldChest.Show(); });
	}

	public void Switch(bool isOn)
	{
		gameObject.SetActive(isOn);
	}

	public void OnUpdate(float time)
	{
		PlatformManager.UserDB.inventory.SelectRewardChest?.OnUpdate(time);
	}

	public void OpenPopup()
	{
		rewardPage.gameObject.SetActive(true);
		rewardPage.ShowPage();
	}
}
