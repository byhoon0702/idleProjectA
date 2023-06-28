using System.Collections;
using System.Collections.Generic;
using RuntimeData;
using UnityEngine;

public class UIRewardLog : MonoBehaviour
{
	[SerializeField] private UILogItem[] uiLogItems;

	private int currentIndex = 0;


	public void ShowLog(RewardInfo reward, IdleNumber _count)
	{
		UILogItem item = GetCurrentLogItem();
		item.ShowLog(reward, _count);
		item.transform.SetAsFirstSibling();
	}

	private UILogItem GetCurrentLogItem()
	{
		currentIndex = currentIndex % uiLogItems.Length;
		UILogItem item = uiLogItems[currentIndex];

		currentIndex++;
		return item;
	}
}
